using MathStuff.vectors;
using SomeChartsUi.data;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.text;

namespace SomeChartsUi.elements.other; 

public class Label : RenderableBase {
	public FontData font = new();
	public ChartProperty<float> textScale = 12;
	public ChartProperty<string> txt;
	public indexedColor color = theme.default8_ind;

	public Label(ChartProperty<string> txt, ChartsCanvas c) : base(c) {
		this.txt = txt;
	}
	
	public Label(string txt, ChartsCanvas c) : this(new ChartPropertyValue<string>(txt), c) { }

	public override void GenerateMesh() {
		
	}

	protected override void AfterDraw() {
		base.AfterDraw();
		DrawText(txt.Get(this), float2.zero, color.GetColor(), new());
	}
	// protected override void Render() {
	// 	string text = txt.Get(this);
	// 	DrawText(text, float2.zero, color.GetColor(), font, textScale.Get(this));
	// }
}