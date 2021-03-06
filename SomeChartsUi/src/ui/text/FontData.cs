namespace SomeChartsUi.ui.text;

public class FontData {
	public string family;
	public bool isBold;
	public bool isExpanded;
	public bool isItalic;

	public FontData(string family = "Comfortaa", bool isBold = false, bool isItalic = false, bool isExpanded = false) {
		this.family = family;
		this.isBold = isBold;
		this.isItalic = isItalic;
		this.isExpanded = isExpanded;
	}
}