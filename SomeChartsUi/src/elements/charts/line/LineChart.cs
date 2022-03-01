using SomeChartsUi.data;
using SomeChartsUi.themes.colors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.elements.charts.line;

/// <summary>simple line chart with uniform values</summary>
public class LineChart : RenderableBase, IDownsample {
	public IChartData<float> values;
	public IChartData<indexedColor> colors;

	public Orientation orientation = Orientation.horizontal;
	public ChartProperty<float> lineThickness = new ChartPropertyFunc<float>(r => 1 / r.canvas.transform.zoom.animatedValue.x);
	public ChartProperty<float> pointThickness = new ChartPropertyFunc<float>(r => 2 / r.canvas.transform.zoom.animatedValue.x);
	public ChartProperty<float> lineAlphaMul = .5f;
	public float downsampleMultiplier { get; set; } = .5f;
	public float elementScale { get; set; } = 100;

	public LineChart(IChartData<float> values, IChartData<indexedColor> colors, ChartsCanvas c) : base(c) {
		this.values = values;
		this.colors = colors;
	}
	
	public LineChart(IChartData<float> values, indexedColor color, ChartsCanvas c) : this(values, new ConstChartData<indexedColor>(color), c) {}

	public override void GenerateMesh() {
		
	}
	// protected override unsafe void Render() {
	// 	int length = values.GetLength();
	// 	if (length < 1) return;
	// 	
	// 	int downsample = GetDownsample(orientation, downsampleMultiplier);
	// 	(float startPos, float endPos) culledPositions = GetStartEndPos(float2.zero, length * elementScale, orientation);
	// 	(float start, int count) = GetStartCountIndexes(culledPositions, elementScale * (1 << downsample));
	// 	int startIndex = (int)(start / elementScale);
	// 	
	// 	if (count <= 1) return;
	//
	// 	float2 vec = GetOrientationVector(orientation);
	// 	
	// 	// get line points
	// 	float2* linePoints = stackalloc float2[count];
	// 	float* pointHeightsStart = (float*)linePoints + (int) vec.x;
	// 	float* pointWidthStart = (float*)linePoints + (int) vec.y;
	// 	values.GetValuesWithStride(startIndex, count, downsample, pointHeightsStart, 2);
	// 	for (int i = 0; i < count; i++)
	// 		pointWidthStart[i << 1] = (startIndex + (i << downsample)) * elementScale;
	//
	// 	// get line colors
	// 	color* lineColors = stackalloc color[count];
	// 	colors.GetColors(startIndex, count, downsample, lineColors);
	// 	
	// 	DrawConnectedLines(linePoints, lineColors, lineThickness.Get(this), count - 1, lineAlphaMul.Get(this));
	// 	DrawPoints(linePoints, lineColors, pointThickness.Get(this), count - 1);
	// }
}