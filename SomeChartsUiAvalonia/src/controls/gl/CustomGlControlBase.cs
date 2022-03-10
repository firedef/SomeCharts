using System;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.OpenGL.Imaging;
using Avalonia.Threading;
using MathStuff;
using static Avalonia.OpenGL.GlConsts;

namespace SomeChartsUiAvalonia.controls.gl;

/// <summary>based on <see cref="OpenGlControlBase"/></summary>
public abstract class CustomGlControlBase : Control {
	protected IOpenGlBitmapAttachment? attachment;
	protected OpenGlBitmap? bitmap;
	protected int depthBuffer;
	protected PixelSize depthBufferSize;
	protected int frameBuffer;
	protected IGlContext? glContext;

	protected int screenfFrameBuffer;
	protected int screenTextureId;
	
	private bool _glFailed;
	private bool _isInitialized;

	public override void Render(DrawingContext context) {
		if (_glFailed) return;

		if (!_isInitialized) {
			if (!Init()) {
				_glFailed = true;
				GlInfo.CheckError("gl init");
			}
			using (glContext!.MakeCurrent()) {
				OnOpenGlInit(glContext.GlInterface, frameBuffer);
			}
		}

		using (glContext!.MakeCurrent()) {
			glContext.GlInterface.BindFramebuffer(GL_FRAMEBUFFER, frameBuffer);
			EnsureTextureAttachment();
			EnsureDepthBufferAttachment(glContext.GlInterface);

			OnOpenGlRender(glContext.GlInterface, frameBuffer);
			//GlInfo.gl!.GetIntegerv(GL_DRAW_FRAMEBUFFER, out int a);

			int w = (int)Bounds.Width;
			int h = (int)Bounds.Height;
			//GlInfo.gl.BindFramebuffer(GL_READ_FRAMEBUFFER, frameBuffer);
			//GlInfo.gl.BindFramebuffer(GL_DRAW_FRAMEBUFFER, screenfFrameBuffer);
			//GlInfo.gl.BlitFramebuffer(0, 0, w, h, 0, 0, w, h, GL_COLOR_BUFFER_BIT, GL_LINEAR);
//
			//GlInfo.gl.BindFramebuffer(GL_READ_FRAMEBUFFER, a);
			//GlInfo.gl.BindFramebuffer(GL_DRAW_FRAMEBUFFER, frameBuffer);

			//GlInfo.gl!.Finish();
			//GlInfo.gl.GetIntegerv(GL_FRAMEBUFFER_BINDING, out int oldFbo);
			//GlInfo.gl.GetIntegerv(GL_TEXTURE_BINDING_2D, out int oldTexture);
			//GlInfo.gl.GetIntegerv(GL_ACTIVE_TEXTURE, out int oldActive);
			//
			//GlInfo.gl.BindFramebuffer(GL_FRAMEBUFFER, screenfFrameBuffer);
			//GlInfo.gl.BindTexture(GL_TEXTURE_2D, screenTextureId);
			//GlInfo.gl.ActiveTexture(GL_TEXTURE0);
			//
			//GlInfo.gl.CopyTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, 0, 0, w, h);
//
			//GlInfo.gl.BindFramebuffer(GL_FRAMEBUFFER, oldFbo);
			//
			//OnOpenGlPostRender(glContext.GlInterface, frameBuffer);
			//GlInfo.gl.BindTexture(GL_TEXTURE_2D, oldTexture);
			//GlInfo.gl.ActiveTexture(oldActive);
                    
			GlInfo.gl!.Finish();
			GlInfo.gl!.GetIntegerv(GL_TEXTURE_BINDING_2D, out int tex);
			
			GlInfo.gl.BindFramebuffer(GL_READ_FRAMEBUFFER, frameBuffer);
			GlInfo.gl.BindFramebuffer(GL_DRAW_FRAMEBUFFER, screenfFrameBuffer);
			GlInfo.gl.BlitFramebuffer(0, 0, w, h, 0, 0, w, h, GL_COLOR_BUFFER_BIT, GL_LINEAR);
			//GlInfo.gl.BindFramebuffer(GL_FRAMEBUFFER, frameBuffer);
			//GlInfo.gl.CopyTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, 0, 0, w, h);

			GlInfo.gl.ActiveTexture(GL_TEXTURE0);
			GlInfo.gl!.BindTexture(GL_TEXTURE_2D, screenTextureId);
			GlInfo.gl.BindFramebuffer(GL_DRAW_FRAMEBUFFER, frameBuffer);
			
			OnOpenGlPostRender(glContext.GlInterface, frameBuffer);
			GlInfo.gl!.BindTexture(GL_TEXTURE_2D, tex);
			
			//GlInfo.gl.Flush();
			//GlInfo.gl!.BindTexture(GL_TEXTURE_2D, oldTexture);
			
			attachment!.Present();
		}

		context.DrawImage(bitmap, new(bitmap!.Size), Bounds);
		base.Render(context);
		
		Dispatcher.UIThread.Post(InvalidateVisual);
	}

	private bool Init() {
		_isInitialized = true;

		IPlatformOpenGlInterface? glInterface = GetGlInterface();
		if (glInterface == null) return false;

		if (!glInterface.CanShareContexts) {
			Console.WriteLine("Unable to initialize OpenGL: current platform does not support multithreaded context sharing");
			return false;
		}

		try {
			glContext = glInterface.CreateSharedContext();
		}
		catch (Exception e) {
			Console.WriteLine(e);
			return false;
		}

		GlInfo.gl = glContext.GlInterface;
		GlInfo.glExt = new(glContext.GlInterface);
		GlInfo.version = glContext.Version;

		try {
			PixelSize pixSize = GetPixelSize();
			bitmap = new(pixSize, GetDpi());
			if (!bitmap.SupportsContext(glContext)) {
				Console.WriteLine("Unable to initialize OpenGL: unable to create OpenGlBitmap: OpenGL context is not compatible");
				return false;
			}
		}
		catch (Exception e) {
			glContext.Dispose();
			glContext = null;
			Console.WriteLine(e);
			return false;
		}

		using (glContext.MakeCurrent()) {
			try {
				depthBufferSize = GetPixelSize();
				int[] arr = new int[1];
				glContext.GlInterface.GenFramebuffers(1, arr);
				frameBuffer = arr[0];
				glContext.GlInterface.BindFramebuffer(GL_FRAMEBUFFER, frameBuffer);

				EnsureDepthBufferAttachment(glContext.GlInterface);
				EnsureTextureAttachment();

				return true;
			}
			catch (Exception e) {
				Console.WriteLine(e);
				return false;
			}
		}
	}

	private void EnsureTextureAttachment() {
		GlInfo.gl!.BindFramebuffer(GL_FRAMEBUFFER, frameBuffer);
		if (bitmap != null && attachment != null && bitmap.PixelSize == GetPixelSize()) return;
		attachment?.Dispose();
		attachment = null;
		bitmap?.Dispose();
		bitmap = new(GetPixelSize(), GetDpi());
		attachment = bitmap.CreateFramebufferAttachment(glContext);

		// sorry for that, _texture field, using for post processing is private, and derived class is internal
		//FieldInfo? textureField = attachment.GetType().GetField("_texture", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		//if (textureField == null) {
		//	Console.WriteLine("cannot get field '_texture' from opengl bitmap attachment");
		//	_glFailed = true;
		//	return;
		//}

		int[] arr = {screenTextureId};
		if (screenTextureId != 0) GlInfo.gl.DeleteTextures(1, arr);
		GlInfo.gl.GenTextures(1, arr);
		screenTextureId = arr[0];

		if (screenfFrameBuffer == 0) {
			GlInfo.gl.GenFramebuffers(1, arr);
			screenfFrameBuffer = arr[0];
		}

		int format = GlInfo.version!.Value.Type == GlProfileType.OpenGLES ? GL_RGBA : GL_RGBA8;
		
		GlInfo.gl!.GetIntegerv(GL_TEXTURE_BINDING_2D, out int oldTexture);
		GlInfo.gl!.GetIntegerv(GL_FRAMEBUFFER_BINDING, out int oldFb);
		GlInfo.gl.BindTexture(GL_TEXTURE_2D, screenTextureId);
		GlInfo.gl.TexImage2D(GL_TEXTURE_2D, 0, format, (int) Bounds.Width, (int) Bounds.Height, 0, GL_RGB, GL_UNSIGNED_BYTE, IntPtr.Zero);
		GlInfo.gl.TexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		GlInfo.gl.TexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
		GlInfo.gl.TexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
		GlInfo.gl.TexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);

		GlInfo.gl.BindFramebuffer(GL_FRAMEBUFFER, screenfFrameBuffer);
		GlInfo.gl.FramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, screenTextureId, 0);
		GlInfo.gl.BindTexture(GL_TEXTURE_2D, oldTexture);
		
		GlInfo.gl.BindFramebuffer(GL_FRAMEBUFFER, oldFb);

		//GlInfo.gl.FramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, screenTextureId, 0);

		//screenTextureId = (int)textureField.GetValue(attachment)!;
	}

	private void EnsureDepthBufferAttachment(GlInterface gl) {
		PixelSize size = GetPixelSize();
		if (size == depthBufferSize && depthBuffer != 0)
			return;

		gl.GetIntegerv(GL_RENDERBUFFER_BINDING, out int oldRenderBuffer);
		if (depthBuffer != 0) gl.DeleteRenderbuffers(1, new[] {depthBuffer});

		int[] oneArr = new int[1];
		gl.GenRenderbuffers(1, oneArr);
		depthBuffer = oneArr[0];
		gl.BindRenderbuffer(GL_RENDERBUFFER, depthBuffer);
		gl.RenderbufferStorage(GL_RENDERBUFFER,
		                       GlInfo.version!.Value.Type == GlProfileType.OpenGLES ? GL_DEPTH_COMPONENT16 : GL_DEPTH_COMPONENT,
		                       size.Width, size.Height);
		gl.FramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_RENDERBUFFER, depthBuffer);
		gl.BindRenderbuffer(GL_RENDERBUFFER, oldRenderBuffer);
		depthBufferSize = size;
	}

	private PixelSize GetPixelSize() {
		double scaling = VisualRoot!.RenderScaling;
		return new(math.max(1, (int)(Bounds.Width * scaling)),
		           math.max(1, (int)(Bounds.Height * scaling)));
	}

	private Vector GetDpi() => new(96, 96);

	private void DoCleanup() {
		if (glContext == null) return;
		using (glContext.MakeCurrent()) {
			GlInterface? gl = glContext.GlInterface;
			gl.BindTexture(GL_TEXTURE_2D, 0);
			gl.BindFramebuffer(GL_FRAMEBUFFER, 0);
			gl.DeleteFramebuffers(1, new[] {frameBuffer});
			gl.DeleteRenderbuffers(1, new[] {depthBuffer});
			attachment?.Dispose();
			attachment = null;
			bitmap?.Dispose();
			bitmap = null;

			try {
				if (!_isInitialized) return;
				_isInitialized = false;
				OnOpenGlDeinit(glContext.GlInterface, frameBuffer);
			}
			finally {
				glContext.Dispose();
				glContext = null;
			}
		}
	}
	protected virtual void OnOpenGlInit(GlInterface gl, int fb) { }
	protected virtual void OnOpenGlDeinit(GlInterface gl, int fb) { }
	protected virtual void OnOpenGlRender(GlInterface gl, int fb) { }
	protected virtual void OnOpenGlPostRender(GlInterface gl, int fb) { }

	private static IPlatformOpenGlInterface? GetGlInterface() => AvaloniaLocator.Current.GetService<IPlatformOpenGlInterface>();

	protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e) {
		DoCleanup();
		base.OnDetachedFromVisualTree(e);
	}
}