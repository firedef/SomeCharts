using FreeTypeSharp;
using FreeTypeSharp.Native;
using SomeChartsUi.ui.canvas;

namespace SomeChartsUi.ui.text;

public class Font {
	public string family;
	public bool isBold;
	public bool isExpanded;
	public bool isItalic;
	public FontTextures textures;

	public Font(string family, bool isBold, bool isItalic, bool isExpanded, FontTextures textures) {
		this.family = family;
		this.isBold = isBold;
		this.isItalic = isItalic;
		this.isExpanded = isExpanded;
		this.textures = textures;
	}

	public static Font LoadFromPath(string path, ChartsCanvas canvas, uint resolution = 32) {
		using FileStream fs = new(path, FileMode.Open);
		FT.FT_New_Face(FreeType.ftLib.Native, path, 0, out IntPtr face).CheckError();
		FreeTypeFaceFacade faceF = new(FreeType.ftLib, face);

		return new("todo", false, false, false, canvas.factory.CreateFontTextureAtlas(faceF, resolution));
	}
}