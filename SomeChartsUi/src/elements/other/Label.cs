using SomeChartsUi.data;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.layers.render;
using SomeChartsUi.ui.text;

namespace SomeChartsUi.elements.other;

public class Label : RenderableBase {
	private readonly Font _font;
	private readonly TextMesh _textMesh;
	public indexedColor color = theme.default8_ind;
	public FontData font = new();
	public float textScale = 12;
	public string txt;

	public Label(string txt, ChartsCanvas c) : base(c) {
		this.txt = txt;
		_textMesh = canvas.factory.CreateTextMesh(this);
		uint resolution = 32;
		 _font = Font.LoadFromPath("data/FiraCode-VariableFont_wght.ttf", renderer.owner, resolution);
		 Font fallbackFont = Font.LoadFromPath("data/NotoSansJP-Regular.otf", renderer.owner, resolution);
		 _font.fallbacks.Add(fallbackFont);
	}

	protected override void GenerateMesh() {
		transform.RecalculateMatrix();
		_textMesh.UpdateTextMesh(txt, _font, textScale, color.GetColor(), transform);
	}

	public override void Render(RenderLayerId pass) {
		if (pass == RenderLayerId.ui) _textMesh.Draw();
	}
}