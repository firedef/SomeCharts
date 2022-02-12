using SomeChartsUi.data;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils;
using SomeChartsUi.utils.rects;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.elements.other; 

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

	public int downsampleMul = 20;
	public float scale = 10;

	public float thickness = 1;
	public bool screenSpaceThickness = true;
	public float fontSize = 10;
	public bool screenSpaceLabels = true;
	public FontData font = new();
	
	protected override void Render() {
		float2 pos = float2.zero;
		
		if (stickToScreen && transform.type == TransformType.worldSpace) {
			pos.y = math.clamp(canvas.transform.worldBounds.bottom - transform.position.y, stickRange.bottom, stickRange.top) + stickOffset.x;
			pos.x = math.clamp(canvas.transform.worldBounds.left - transform.position.x, stickRange.left, stickRange.right) + stickOffset.y;
		}

		{
			int downsample = GetDownsampleY(downsampleMul);
			float space = scale * (1 << downsample);
			int count = length >> downsample;
			
			float2[] positions =  GetPositions(pos, space, count, orientation);
			float2 vec = (orientation & Orientation.vertical) != 0 ? new(0, 1) : new(1, 0);// vertical : horizontal
			float scaleVal = 1 / (canvas.transform.zoom.animatedValue * vec).sum;
			
			if (drawLines) DrawStraightLines(positions, lineLength - (pos * vec.yx).sum, lineColor.GetColor(), screenSpaceThickness ? thickness * scaleVal : thickness, orientation);
			if (drawLabels && names != null) {
				(float s, int c) = GetStartCountIndexes(GetStartEndPos(pos, pos + count * space, orientation), space);
				if (c < 1) return;
				string[] txt = names!.GetValues((int)(s / scale), c, downsample);
				DrawText(txt, positions, font, labelColor.GetColor(), screenSpaceLabels ? fontSize * scaleVal : fontSize, skipLabels..);
			}
		}
	}
}