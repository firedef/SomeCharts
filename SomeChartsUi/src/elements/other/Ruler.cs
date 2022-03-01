using SomeChartsUi.data;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils;
using SomeChartsUi.utils.rects;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.elements.other;

//TODO: fix labels when using transform position
public class Ruler : RenderableBase {
	public Orientation orientation;
	public int length = 1000;

	public IChartManagedData<string>? names;

	public int skipLabels = 1;
	public bool useStyledText = false;
	
	public bool stickToScreen = true;
	public rect stickRange = new(0, 0, 0, 0);
	public float2 stickOffset = new(0, 0);

	public bool drawLines = true;
	public bool drawLabels = true;
	public indexedColor labelColor = theme.default8_ind;
	public indexedColor lineColor = theme.default1_ind;
	public float lineLength = float.MaxValue;

	public int downsampleMul = 4;
	public float scale = 100;

	public float thickness = 1;
	public bool screenSpaceThickness = true;
	public float fontSize = 10;
	public bool screenSpaceLabels = true;
	public FontData font = new();

	public override void GenerateMesh() {
		
	}

	// protected override void Render() {
	// 	float2 pos = float2.zero;
	// 	
	// 	float2 vec = (orientation & Orientation.vertical) != 0 ? new(0, 1) : new(1, 0);// vertical : horizontal
	//
	// 	RenderableTransform tr = transform.Get(this);
	// 	if (stickToScreen && tr.type == TransformType.worldSpace) {
	// 		pos.x = math.clamp(canvas.transform.worldBounds.left, stickRange.left, stickRange.right) + stickOffset.x;
	// 		pos.y = math.clamp(canvas.transform.worldBounds.bottom, stickRange.bottom, stickRange.top) + stickOffset.y;
	//
	// 		//if ((orientation & Orientation.vertical) == 0)
	// 		//	pos.y = MathF.Ceiling(pos.y / scale) * scale;
	// 		//else pos.x = MathF.Ceiling(pos.x / scale) * scale;
	// 	}
	//
	// 	{
	// 		int downsample = GetDownsampleY(downsampleMul);
	// 		float space = scale * (1 << downsample);
	// 		int count = length >> downsample;
	// 		
	// 		float2[] positions =  GetPositions(pos, space, count, orientation);
	// 		float scaleVal = 1 / (canvas.transform.zoom.animatedValue * vec).sum;
	// 		
	// 		if (drawLines) DrawStraightLines(positions, lineLength - (pos * vec.yx).sum, lineColor.GetColor(), screenSpaceThickness ? thickness * scaleVal : thickness, orientation);
	// 		if (drawLabels && names != null) {
	// 			(float s, int c) = GetStartCountIndexes(GetStartEndPos(pos, pos + count * space, orientation), space);
	// 			if (c < 1) return;
	// 			string[] txt = names!.GetValues((int)((s + (pos * vec).sum) / scale), c, downsample);
	// 			DrawText(txt, positions, font, labelColor.GetColor(), screenSpaceLabels ? fontSize * scaleVal : fontSize, skipLabels..);
	// 		}
	// 	}
	// }
	public Ruler(ChartsCanvas owner) : base(owner) { }
}