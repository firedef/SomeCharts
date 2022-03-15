using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.data;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.layers.render;
using SomeChartsUi.utils.mesh;

namespace SomeChartsUi.elements.charts.scatter; 

public class ScatterChart : RenderableBase {
	private const int _bufferSize = 15000;
	
	public IChartData<float3> values;
	public IChartData<indexedColor> colors;
	public IChartData<ScatterShape> shapes = new ConstChartData<ScatterShape>(ScatterShape.circle);

	public indexedColor boundsColor = theme.default2_ind;
	
	public float2 scale = 1000;
	public bool drawBounds = true;

	private Mesh noTextureMesh;

	public ScatterChart(ChartsCanvas owner) : base(owner) {
		noTextureMesh = owner.factory.CreateMesh();
	}
	
	protected override unsafe void GenerateMesh() {
		mesh!.Clear();
		
		int len = values.GetLength();
		if (len < 1) return;

		int bufferLen = math.min(_bufferSize, len);
		float3* bufferValues = stackalloc float3[bufferLen];
		indexedColor* bufferColors = stackalloc indexedColor[bufferLen];
		ScatterShape* bufferShapes = stackalloc ScatterShape[bufferLen];

		int vCount = bufferLen * 4;
		int iCount = bufferLen * 6;
		
		values.GetValues(0, bufferLen, 0, bufferValues);
		colors.GetValues(0, bufferLen, 0, bufferColors);
		shapes.GetValues(0, bufferLen, 0, bufferShapes);
		
		mesh.vertices.EnsureCapacity(vCount);
		mesh.indexes.EnsureCapacity(iCount);

		for (int i = 0; i < bufferLen; i++) {
			float3 element = bufferValues[i];
			color col = bufferColors[i].GetColor();
			ScatterShape shape = bufferShapes[i];
			
			rect uvs = new(0, 0, 1, 1);
			uvs.left = shape switch {
				ScatterShape.circle => 0,
				ScatterShape.triangle => 1,
				ScatterShape.quad => 2,
				_ => throw new ArgumentOutOfRangeException()
			};

			float s = element.z * .5f;
			mesh.AddRect(
				new(element.x - s, element.y - s), 
				new(element.x - s, element.y + s),
				new(element.x + s, element.y + s),
				new(element.x + s, element.y - s),
				col,
				uvs);
		}
		
		mesh.OnModified();
	}

	protected override void OnFrequentUpdate() {
		noTextureMesh.Clear();
		noTextureMesh.vertices.EnsureCapacity(4 * 4);
		noTextureMesh.indexes.EnsureCapacity(4 * 6);
		
		if (drawBounds) {
			float thickness = 2 / canvas.transform.scale.animatedValue.avg;
			AddLine(noTextureMesh, new(0,0), new(0,scale.y), thickness, boundsColor.GetColor());
			AddLine(noTextureMesh, new(0,scale.y), new(scale.x,scale.y), thickness, boundsColor.GetColor());
			AddLine(noTextureMesh, new(scale.x,scale.y), new(scale.x,0), thickness, boundsColor.GetColor());
			AddLine(noTextureMesh, new(scale.x,0), new(0,0), thickness, boundsColor.GetColor());
		}
		
		noTextureMesh.OnModified();
	}

	public override void Render(RenderLayerId pass) {
		if (pass == RenderLayerId.transparent) DrawMesh(material);
		if (pass == RenderLayerId.ui) DrawMesh(null, noTextureMesh);
	}
}