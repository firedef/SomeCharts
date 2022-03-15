using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.layers.render;
using SomeChartsUi.utils.mesh;

namespace SomeChartsUi.elements.other;

public class TestRenderable : RenderableBase {
	public TestRenderable(ChartsCanvas owner) : base(owner) { }
	protected override void GenerateMesh() {
		const float s = 100;
		rect r = new(-s, -s, s * 2, s * 2);
		Vertex[] vertices = {
			new(r.leftBottom, float3.front, new(0, 0), color.red),
			new(r.leftTop, float3.front, new(0, 1), color.softRed),
			new(r.rightTop, float3.front, new(1, 1), color.blue),
			new(r.rightBottom, float3.front, new(1, 0), color.softBlue)
		};
		ushort[] indexes = {0, 1, 2, 0, 2, 3};
		mesh!.SetVertices(vertices);
		mesh.SetIndexes(indexes);
	}
	
	public override void Render(RenderLayerId pass) {
		if (!isTransparent && pass == RenderLayerId.opaque) DrawMesh(material);
		if (isTransparent && pass == RenderLayerId.transparent) DrawMesh(material);
	}
}