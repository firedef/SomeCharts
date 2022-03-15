using System;
using System.IO;
using Avalonia.Media.Imaging;
using Avalonia.OpenGL;
using Avalonia.Platform;
using MathStuff.vectors;
using SomeChartsUi.utils.shaders;

namespace SomeChartsUiAvalonia.impl.opengl;

public class GlTexture : Texture, IDisposable {
	public readonly WriteableBitmap? bitmap;
	public float2 bitmapSize;
	public int id;

	public GlTexture(string path) : base("") {
		using FileStream fs = new(path, FileMode.Open);
		bitmap = WriteableBitmap.Decode(fs);
		bitmapSize = new((float)bitmap.Size.Width, (float)bitmap.Size.Height);
	}

	public unsafe GlTexture(int width, int height, int format, int type) : base("") {
		bitmap = null;

		int i = 0;
		GlInfo.glExt!.GenTextures(1, &i);
		id = i;
		GlInfo.gl!.BindTexture(GlConsts.GL_TEXTURE_2D, id);

		SetWrap(TextureWrap.repeat);
		SetFilter(TextureFilter.linear);
		GlInfo.gl.TexImage2D(GlConsts.GL_TEXTURE_2D, 0, GlConsts.GL_RGB, width, height, 0, format, type, IntPtr.Zero);
		bitmapSize = new(width, height);
	}
	public override float2 size => bitmapSize;
	public void Dispose() {
		ReleaseUnmanagedResources();
		GC.SuppressFinalize(this);
	}

	public unsafe void TryLoad() {
		if (id != 0) return;
		int i = 0;
		GlInfo.glExt!.GenTextures(1, &i);
		id = i;
		GlInfo.gl!.BindTexture(GlConsts.GL_TEXTURE_2D, id);

		SetWrap(TextureWrap.repeat);
		SetFilter(TextureFilter.nearest);

		using ILockedFramebuffer lockedFramebuffer = bitmap!.Lock();
		IntPtr ptr = lockedFramebuffer.Address;

		(int type, int format, int internalFormat) = lockedFramebuffer.Format switch {
			PixelFormat.Rgb565 => (GlConsts.GL_UNSIGNED_SHORT_5_6_5, GlConsts.GL_RGB, GlConsts.GL_RGB),
			PixelFormat.Rgba8888 => (GlConsts.GL_UNSIGNED_INT_8_8_8_8, GlConsts.GL_RGBA, GlConsts.GL_RGBA),
			PixelFormat.Bgra8888 => (GlConsts.GL_UNSIGNED_INT_8_8_8_8, GlConsts.GL_BGRA, GlConsts.GL_RGBA),
			_ => throw new ArgumentOutOfRangeException()
		};

		GlInfo.gl.TexImage2D(GlConsts.GL_TEXTURE_2D, 0, internalFormat, (int) size.x, (int) size.y, 0, format, type, ptr);
	}

	public void Bind() {
		TryLoad();
		GlInfo.gl!.BindTexture(GlConsts.GL_TEXTURE_2D, id);
	}

	public static void SetWrap(TextureWrap v) {
		SetParameter(GlConsts.GL_TEXTURE_WRAP_S, (int)v);
		SetParameter(GlConsts.GL_TEXTURE_WRAP_T, (int)v);
	}

	public static void SetFilter(TextureFilter v) {
		SetParameter(GlConsts.GL_TEXTURE_MIN_FILTER, v == TextureFilter.nearest ? GlConsts.GL_NEAREST : GlConsts.GL_LINEAR);
		SetParameter(GlConsts.GL_TEXTURE_MAG_FILTER, (int)v);
	}

	public static void SetParameter(int name, int v) => GlInfo.gl!.TexParameteri(GlConsts.GL_TEXTURE_2D, name, v);

	private unsafe void ReleaseUnmanagedResources() {
		bitmap?.Dispose();
		int i = id;
		GlInfo.glExt!.DeleteTextures(1, &i);
	}

	public override string ToString() => $"gl texture: #{id}";
}

public enum TextureWrap {
	repeat = GlConsts.GL_REPEAT
}

public enum TextureFilter {
	nearest = GlConsts.GL_NEAREST,
	linear = GlConsts.GL_LINEAR
}