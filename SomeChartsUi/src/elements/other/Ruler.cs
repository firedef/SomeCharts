using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils;
using SomeChartsUi.utils.rects;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.elements.other; 

public class Ruler : RenderableBase {
	public Orientation orientation;
	public int length = 1000;

	public bool skipFirstLabel = true;
	public bool useStyledText = false;
	
	public bool stickToScreen = true;
	/// <summary>
	/// rect values is (range left, range bottom, range right, range top)
	/// </summary>
	public rect stickRange = new(0, 0, 0, 0);

	public bool drawLines = true;
	public bool drawLabels = true;
	public indexedColor labelColor = theme.default8_ind;
	public indexedColor lineColor = theme.default1_ind;
	public float lineLength = float.MaxValue;

	public int downsampleMul = 10;
	public float scale = 10;
	
	protected override void Render() {
		float2 pos = float2.zero;
		
		if (stickToScreen && transform.type == TransformType.worldSpace) {
			pos.y = math.clamp(canvas.transform.worldBounds.bottom - transform.position.y, stickRange.bottom, stickRange.top);
			pos.x = math.clamp(canvas.transform.worldBounds.left - transform.position.x, stickRange.left, stickRange.right);
		}

		if ((orientation & Orientation.vertical) != 0) {
			int downsample = GetDownsampleY(downsampleMul);
			float space = scale * (1 << downsample);
			int c = length >> downsample;
			DrawHorizontalLines(pos, space, lineLength, c, lineColor.GetColor(), 1 / canvas.transform.zoom.animatedValue.y);
			return;
		}
		{
			int downsample = GetDownsampleX(downsampleMul);
			float space = scale * (1 << downsample);
			int c = length >> downsample;
			DrawVerticalLines(pos, space, lineLength, c, lineColor.GetColor(), 1 / canvas.transform.zoom.animatedValue.x);
		}
	}
}