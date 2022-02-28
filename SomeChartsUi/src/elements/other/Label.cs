using SomeChartsUi.data;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.elements.other; 

public class Label : RenderableBase {
	public FontData font = new();
	public ChartProperty<float> textScale = 12;
	public ChartProperty<string> txt;
	public indexedColor color = theme.default8_ind;

	public Label(ChartProperty<string> txt) {
		this.txt = txt;
	}
	
	public Label(string txt) : this(new ChartPropertyValue<string>(txt)) { }

	public override void GenerateMesh() {
		
	}
	// protected override void Render() {
	// 	string text = txt.Get(this);
	// 	DrawText(text, float2.zero, color.GetColor(), font, textScale.Get(this));
	// }
}