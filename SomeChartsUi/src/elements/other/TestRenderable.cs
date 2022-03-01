using SomeChartsUi.themes.colors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.rects;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.elements.other; 

public class TestRenderable : RenderableBase {
	public override void GenerateMesh() {
		const float s = 100; 
		rect r = new(-s, -s, s*2, s*2);
		Vertex[] vertices = {
			new(r.leftBottom,		new(0, 0), color.red),
			new(r.leftTop,		new(0, 1), color.softRed),
			new(r.rightTop,		new(1, 1), color.blue),
			new(r.rightBottom,	new(1, 0), color.softBlue),
		};
		ushort[] indexes = {0, 1, 2, 0, 2, 3};
		mesh!.SetVertices(vertices);
		mesh.SetIndexes(indexes);
	}
	// protected override void Render() {
	// 	rect r = new(-100, -100, 200, 200);
	// 	float2[] points = {r.leftBottom, r.leftTop, r.rightTop, r.rightBottom};
	// 	color[] colors = {color.red, color.softRed, color.blue, color.softBlue};
	// 	ushort[] indexes = {0, 1, 2, 0, 2, 3};
	// 	DrawVertices(points, null, colors, indexes);
	// 	//Console.WriteLine("render");
	// }
	public TestRenderable(ChartsCanvas owner) : base(owner) { }
}