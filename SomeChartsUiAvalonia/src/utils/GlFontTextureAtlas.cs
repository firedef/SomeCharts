using System;
using Avalonia.OpenGL;
using FreeTypeSharp.Native;
using MathStuff.vectors;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils.shaders;
using SomeChartsUiAvalonia.controls.gl;

namespace SomeChartsUiAvalonia.utils; 

public class GlFontTextureAtlas : FontTextureAtlas {
	private GlFontTextures _owner;

	public GlFontTextureAtlas(GlFontTextures owner, int size = 1024) : base(size) {
		_owner = owner;
	}

	public override Texture CreateTexture(int width, int height) {
		GlInfo.glExt!.PixelStorei(GlConsts.GL_UNPACK_ALIGNMENT, 1);
		GlTexture tex = new(width, height, GlConsts.GL_RED, GlConsts.GL_UNSIGNED_BYTE);
		
		tex.Bind();
		//GlInfo.glExt!.GenerateMipmap(GlConsts.GL_TEXTURE_2D);

		return tex;
	}
	protected override unsafe void AddChar(int x, int y, int w, int h, string ch) {
		float advance = *(int*)&_owner.face.FaceRec->glyph->linearHoriAdvance / 65536f;
		
		float baseline = _owner.face.FaceRec->glyph->bitmap.rows - _owner.face.FaceRec->glyph->bitmap_top;
		Console.WriteLine(baseline);
		float2 size = new(w, h);
		FontCharData data = new(advance, baseline, new(x, y), size, ch);
		characters.Add(data);
	}
	
	public override unsafe void WriteToTexture(void* img, int x, int y, int width, int height, int level) {
		((GlTexture)texture).Bind();
		GlInfo.glExt!.PixelStorei(GlConsts.GL_UNPACK_ALIGNMENT, 1);
		GlInfo.glExt!.TexSubImage2D(GlConsts.GL_TEXTURE_2D, level, x, y, width, height, GlConsts.GL_RED, GlConsts.GL_UNSIGNED_BYTE, img);
		//GlInfo.glExt!.TexSubImage2D(GlConsts.GL_TEXTURE_2D, 1, x>>1, y>>1, width>>1, height>>1, GlConsts.GL_RED, GlConsts.GL_UNSIGNED_BYTE, img);
		//GlInfo.glExt.GenerateMipmap(GlConsts.GL_TEXTURE_2D);
	}
}