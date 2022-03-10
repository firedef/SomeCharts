using SomeChartsUi.data;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.text;

namespace SomeChartsUi.elements.other;

public class Label : RenderableBase {
	private readonly Font _font;
	private readonly TextMesh _textMesh;
	public indexedColor color = theme.default8_ind;
	public FontData font = new();
	public ChartProperty<float> textScale = 12;
	public ChartProperty<string> txt;

	public Label(ChartProperty<string> txt, ChartsCanvas c) : base(c) {
		this.txt = txt;
		_textMesh = new(this);
		uint resolution = 32;
		_font = Font.LoadFromPath("data/Comfortaa-VariableFont_wght.ttf", renderer.owner, resolution);
	}

	public Label(string txt, ChartsCanvas c) : this(new ChartPropertyValue<string>(txt), c) { }

	protected override void GenerateMesh() {
		_textMesh.UpdateTextMesh(txt.Get(this), _font, textScale.Get(this), MathStuff.color.softBlue, transform);
	}

	protected override void AfterDraw() {
		base.AfterDraw();
		_textMesh.Draw();
		//DrawText(txt.Get(this), float2.zero, color.GetColor(), new());
	}
	// protected override void Render() {
	// 	string text = txt.Get(this);
	// 	DrawText(text, float2.zero, color.GetColor(), font, textScale.Get(this));
	// }
}