using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.data;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.layers.render;
using SomeChartsUi.ui.text;

namespace SomeChartsUi.elements.other;

//TODO: fix labels when using transform position
public class Ruler : RenderableBase {

	public int downsampleMul = 4;
	public bool drawLabels = true;

	public bool drawLines = true;
	public FontData font = new();
	public float fontSize = 16;
	public indexedColor labelColor = theme.default8_ind;
	public int length = 1000;
	public indexedColor lineColor = theme.default1_ind;
	public float lineLength = float.MaxValue;

	public IChartManagedData<string>? names;
	public Orientation orientation;
	public float scale = 100;
	public bool screenSpaceLabels = true;
	public bool screenSpaceThickness = true;

	public int skipLabels = 1;
	public float2 stickOffset = new(0, 0);
	public rect stickRange = new(0, 0, 0, 0);

	public bool stickToScreen = true;

	public float thickness = 1;
	public bool useStyledText = false;

	private readonly TextMesh _textMesh;
	private Font _font;
	
	public Ruler(ChartsCanvas owner) : base(owner) {
		_textMesh = owner.factory.CreateTextMesh(this);
		uint resolution = 32;
		_font = Font.LoadFromPath("data/FiraCode-VariableFont_wght.ttf", renderer.owner, resolution);
		Font fallbackFont = Font.LoadFromPath("data/NotoSansJP-Regular.otf", renderer.owner, resolution);
		_font.fallbacks.Add(fallbackFont);

		updateFrameSkip = 1;
	}

	protected override void GenerateMesh() {
		mesh!.Clear();
		float2 pos = float2.zero;
		float2 vec = (orientation & Orientation.vertical) != 0 ? new(0, 1) : new(1, 0);// vertical : horizontal
		if (stickToScreen && transform.type == TransformType.worldSpace) {
			pos.x = math.clamp(canvas.transform.worldBounds.left, stickRange.left, stickRange.right) + stickOffset.x;
			pos.y = math.clamp(canvas.transform.worldBounds.bottom, stickRange.bottom, stickRange.top) + stickOffset.y;
			//if ((orientation & Orientation.vertical) == 0)
			//	pos.y = MathF.Ceiling(pos.y / scale) * scale;
			//else pos.x = MathF.Ceiling(pos.x / scale) * scale;
		}
		{
			int downsample = GetDownsampleY(downsampleMul);
			float space = scale * (1 << downsample);
			int count = length >> downsample;

			float2[] positions = GetPositions(pos, space, count, orientation);
			float scaleVal = 1 / (canvas.transform.scale.animatedValue * vec).sum;

			if (drawLines) AddStraightLines(mesh!, positions, lineLength - (pos * vec.yx).sum, lineColor.GetColor(), screenSpaceThickness ? thickness * scaleVal : thickness, orientation, -.01f);
			if (drawLabels && names != null) {
				(float s, int c) = GetStartCountIndexes(GetStartEndPos(pos, pos + count * space, orientation), space);
				if (c < 1) return;
				string[] txt = names!.GetValues((int)((s + (pos * vec).sum) / scale), c, downsample);
				//DrawText(txt, positions, font, labelColor.GetColor(), screenSpaceLabels ? fontSize * scaleVal : fontSize, skipLabels..);
				_textMesh.ClearMeshes();
				for (int i = 0; i < txt.Length; i++) {
					_textMesh.GenerateMesh(txt[i], _font, screenSpaceLabels ? fontSize * scaleVal : fontSize, labelColor.GetColor(), new(positions[i]));
				}
				
			}
			
			mesh.OnModified();
		}
	}

	public override void Render(RenderLayerId pass) {
		if (pass == RenderLayerId.opaque) DrawMesh(material);
		if (pass == RenderLayerId.ui) _textMesh.Draw();
	}
}