using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.data;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;

namespace SomeChartsUi.elements.charts.scatter; 

public class ScatterChart : RenderableBase {
	private const int _bufferSize = 15000;
	
	public IChartData<float3> values;
	public IChartData<indexedColor> colors;
	public IChartData<ScatterShape>? shapes;

	public indexedColor boundsColor = theme.default2_ind;
	
	public float2 scale = 1000;
	public bool drawBounds = true;

	public ScatterChart(ChartsCanvas owner) : base(owner) { }
	
	protected override unsafe void GenerateMesh() {
		mesh!.Clear();
		
		int len = values.GetLength();
		if (len < 1) return;

		int bufferLen = math.min(_bufferSize, len);
		float3* bufferValues = stackalloc float3[bufferLen];
		indexedColor* bufferColors = stackalloc indexedColor[bufferLen];

		int vCount = (bufferLen + 4) * 4;
		int iCount = (bufferLen + 4) * 6;
		
		values.GetValues(0, bufferLen, 0, bufferValues);
		colors.GetValues(0, bufferLen, 0, bufferColors);
		
		mesh.vertices.EnsureCapacity(vCount);
		mesh.indexes.EnsureCapacity(iCount);

		for (int i = 0; i < bufferLen; i++) {
			float3 element = bufferValues[i];
			color col = bufferColors[i].GetColor();

			float s = element.z * .5f;
			mesh.AddRect(
				new(element.x - s, element.y - s), 
				new(element.x - s, element.y + s),
				new(element.x + s, element.y + s),
				new(element.x + s, element.y - s),
				col );
		}

		if (drawBounds) {
			float thickness = 10;
			AddLine(mesh, new(0,0), new(0,scale.y), thickness, boundsColor.GetColor());
			AddLine(mesh, new(0,scale.y), new(scale.x,scale.y), thickness, boundsColor.GetColor());
			AddLine(mesh, new(scale.x,scale.y), new(scale.x,0), thickness, boundsColor.GetColor());
			AddLine(mesh, new(scale.x,0), new(0,0), thickness, boundsColor.GetColor());
		}
		
		mesh.OnModified();
	}
}