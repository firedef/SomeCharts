using FreeTypeSharp.Native;
using SomeChartsUi.ui.canvas;

namespace SomeChartsUi.ui.text; 

public class FontFamily {
	public List<FontFamilyItem> fonts = new();
}

public class FontFamilyItem {
	private Font? _font;

	public string name;
	public string path;

	public FontFamilyItem(string name, string path) {
		this.name = name;
		this.path = path;
	}

	public Font GetFont(ChartsCanvas canvas) {
		if (_font != null) return _font;
		_font = Font.LoadFromPath(path, canvas);
		
		return _font;
	}
}