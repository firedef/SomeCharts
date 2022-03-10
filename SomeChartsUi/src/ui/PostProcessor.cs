using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;

namespace SomeChartsUi.ui; 

public abstract class PostProcessor {
	public Mesh mesh;
	public ChartsCanvas owner;
	public Material? material;

	public PostProcessor(ChartsCanvas canvas) {
		owner = canvas;
		mesh = canvas.factory.CreateMesh();
		mesh.SetIndexes(new ushort[]{0,1,2,0,2,3});
		UpdateVertices();
	}

	public void UpdateVertices() {
		float2 pMin = float2.zero;
		float2 pMax = owner.transform.screenBounds.widthHeight;

		Vertex[] vertices = {
			new(new(pMin.x, pMin.y), float3.front, new(0, 0), color.white),
			new(new(pMin.x, pMax.y), float3.front, new(0, 1), color.white),
			new(new(pMax.x, pMax.y), float3.front, new(1, 1), color.white),
			new(new(pMax.x, pMin.y), float3.front, new(1, 0), color.white),
		};
		mesh.SetVertices(vertices);
		mesh.OnModified();
	}

	public abstract void Draw();
}