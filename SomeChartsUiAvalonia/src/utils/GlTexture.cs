using System;
using System.Drawing.Drawing2D;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Imaging;
using Avalonia.Platform;
using SomeChartsUi.utils.shaders;
using SomeChartsUiAvalonia.controls.gl;

namespace SomeChartsUiAvalonia.utils; 

public class GlTexture : Texture, IDisposable {
	public readonly WriteableBitmap bitmap;
	public int id;

	public GlTexture(string path) : base("") {
		using FileStream fs = new(path, FileMode.Open);
		bitmap = WriteableBitmap.Decode(fs);
	}

	public unsafe void TryLoad() {
		if (id != 0) return;
		int i = 0;
		GlInfo.glExt!.GenTextures(1, &i);
		id = i;
		GlInfo.gl!.BindTexture(GlConsts.GL_TEXTURE_2D, id);
		
		SetWrap(TextureWrap.repeat);
		SetFilter(TextureFilter.nearest);
		
		using ILockedFramebuffer lockedFramebuffer = bitmap.Lock();
		IntPtr ptr = lockedFramebuffer.Address;

		(int type, int format) = lockedFramebuffer.Format switch {
			PixelFormat.Rgb565 => (GlConsts.GL_UNSIGNED_SHORT_5_6_5, GlConsts.GL_RGB),
			PixelFormat.Rgba8888 => (GlConsts.GL_UNSIGNED_INT_8_8_8_8, GlConsts.GL_RGBA),
			PixelFormat.Bgra8888 => (GlConsts.GL_UNSIGNED_INT_8_8_8_8, GlConsts.GL_BGRA),
			_ => throw new ArgumentOutOfRangeException()
		};
		
		GlInfo.gl!.TexImage2D(GlConsts.GL_TEXTURE_2D, 0, GlConsts.GL_RGB, (int)bitmap.Size.Width, (int)bitmap.Size.Height, 0, format, type, ptr);
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
		SetParameter(GlConsts.GL_TEXTURE_MIN_FILTER, (int) v);
		SetParameter(GlConsts.GL_TEXTURE_MAG_FILTER, (int)v);
	}

	public static void SetParameter(int name, int v) => GlInfo.gl!.TexParameteri(GlConsts.GL_TEXTURE_2D, name, v);

	private unsafe void ReleaseUnmanagedResources() {
		bitmap.Dispose();
		int i = id;
		GlInfo.glExt!.DeleteTextures(1, &i);
	}
	public void Dispose() {
		ReleaseUnmanagedResources();
		GC.SuppressFinalize(this);
	}
}

public enum TextureWrap {
	repeat = GlConsts.GL_REPEAT,
}

public enum TextureFilter {
	nearest = GlConsts.GL_NEAREST,
	linear = GlConsts.GL_LINEAR,
}