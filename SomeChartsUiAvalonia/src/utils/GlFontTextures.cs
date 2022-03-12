using System;
using FreeTypeSharp;
using FreeTypeSharp.Native;
using SomeChartsUi.ui.text;

namespace SomeChartsUiAvalonia.utils;

public class GlFontTextures : FontTextures {
	public FreeTypeFaceFacade face;

	public unsafe GlFontTextures(FreeTypeFaceFacade face, uint resolution) {
		this.face = face;
		this.resolution = resolution;
		
		charMap.Clear();
		uint gindex = 0;
		uint charcode = FT.FT_Get_First_Char(face.Face, out gindex);
		while (gindex != 0) {
			charMap.Add(charcode);
			charcode = FT.FT_Get_Next_Char(face.Face, charcode, out gindex);
		}
	}


	protected override unsafe (FontCharData ch, int atlas) Add(uint character) {
		FT.FT_Set_Pixel_Sizes(face.Face, 0, resolution);
		FT.FT_Load_Char(face.Face, character, FT.FT_LOAD_RENDER).CheckError();
		lineHeight = face.LineSpacing;

		uint charIndex = face.GetCharIndex(character);
		if (charIndex == 0) return (default, -1);

		FT.FT_Render_Glyph((IntPtr)face.GlyphSlot, FT_Render_Mode.FT_RENDER_MODE_SDF).CheckError();
		uint width = face.FaceRec->glyph->bitmap.width;
		uint height = face.FaceRec->glyph->bitmap.rows;


		for (int i = 0; i < atlases.Count; i++) {
			int index = Add_(atlases[i]);
			if (index == -1) continue;
			return (atlases[i].characters[index], i);
		}

		{
			FontTextureAtlas atlas = CreateAtlas();
			atlases.Add(atlas);
			int index = Add_(atlas);
			return (atlas.characters[index], atlases.Count - 1);
		}

		int Add_(FontTextureAtlas atlas) {
			int w = (int)width;
			int h = (int)height;
			(int index, int x, int y) = atlas.Pack(w, h, character);
			if (index == -1) return -1;

			void* bitmap = (void*)face.FaceRec->glyph->bitmap.buffer;
			atlas.WriteToTexture(bitmap, x, y, (int)width, (int)height, 0);
			
			//FT.FT_Done_Glyph(face.GlyphSlot->@internal);

			// for (int j = 0; j < 4; j++) {
			// 	if (j > 0) {
			// 		FT.FT_Set_Pixel_Sizes(face.Face, width>>1, height>>1);
			// 		//FT.FT_Load_Char(face.Face, character[0], FT.FT_LOAD_RENDER).CheckError();
			// 		FT.FT_Load_Char(face.Face, character[0], FT.FT_LOAD_RENDER).CheckError();
			// 		FT.FT_Render_Glyph((IntPtr)face.GlyphSlot, FT_Render_Mode.FT_RENDER_MODE_SDF).CheckError();
			// 	}
			// 	
			// 	width = face.FaceRec->glyph->bitmap.width;
			// 	height = face.FaceRec->glyph->bitmap.rows;
			// 	void* bitmap = (void*)face.FaceRec->glyph->bitmap.buffer;
			// 	atlas.WriteToTexture(bitmap, x>>j, y>>j, (int) width, (int) height, j);
			// 	// atlas.WriteToTexture(bitmap, x>>j, y>>j, w>>j, h>>j, j);
			// }

			return index;
		}
	}
	protected override FontTextureAtlas CreateAtlas() {
		GlFontTextureAtlas glFontTextureAtlas = new(this);
		glFontTextureAtlas.texture = glFontTextureAtlas.CreateTexture(1024, 1024);
		return glFontTextureAtlas;
	}
}