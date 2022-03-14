using FreeTypeSharp;
using FreeTypeSharp.Native;
using SomeChartsUi.ui.canvas;

namespace SomeChartsUi.ui.text;

public record Font(string name, bool isBold, bool isItalic, bool isExpanded, FontTextures textures, ChartsCanvas _owner) {
	private static List<Font> _loadedFonts = new();

	public string name = name;
	public bool isBold = isBold;
	public bool isExpanded = isExpanded;
	public bool isItalic = isItalic;
	public FontTextures textures = textures;
	public List<Font> fallbacks = new();
	private ChartsCanvas _owner = _owner;

	public static Font LoadFromPath(string path, ChartsCanvas canvas, uint resolution = 32) {
		string name = Path.GetFileName(path)[..^4];
		Font? existingFont = TryGetLoadedFont(name);
		if (existingFont != null) return existingFont;
		
		using FileStream fs = new(path, FileMode.Open, FileAccess.Read);
		FT.FT_New_Face(FreeType.ftLib.Native, path, 0, out IntPtr face).CheckError();
		FreeTypeFaceFacade faceF = new(FreeType.ftLib, face);

		Font font = new(name, false, false, false, canvas.factory.CreateFontTextureAtlas(faceF, resolution), canvas);
		_loadedFonts.Add(font);
		return font;
	}

	public static Font? TryGetLoadedFont(string name) => _loadedFonts.FirstOrDefault(v => v.name == name);

	public static Font? TryLoad(string name, ChartsCanvas canvas, uint resolution = 32) {
		Font? existingFont = TryGetLoadedFont(name);
		if (existingFont != null) return existingFont;
		
		string? path = Fonts.TryFind(name);
		return path == null ? null : LoadFromPath(path, canvas, resolution);
	}

	public Font WithFallback(Font fallback) {
		fallbacks.Add(fallback);
		return this;
	}
	
	public Font WithFallbacks(IEnumerable<Font> fallback) {
		fallbacks.AddRange(fallback);
		return this;
	}
	
	public Font WithFallbackPath(string path) {
		fallbacks.Add(LoadFromPath(path, _owner));
		return this;
	}
	
	public Font WithFallbackName(string name) {
		Font? fallback = TryLoad(name, _owner);
		if (fallback != null) fallbacks.Add(fallback);
		return this;
	}
}