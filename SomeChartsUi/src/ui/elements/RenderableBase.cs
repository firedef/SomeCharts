using MathStuff.vectors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;

namespace SomeChartsUi.ui.elements; 

/// <summary>base class of all canvas elements</summary>
public abstract partial class RenderableBase {
	/// <summary>owner canvas</summary>
	public ChartsCanvas canvas { get; private set; }

	public Action beforeRender = () => {};

	/// <summary>mesh to be rendered</summary>
	public Mesh? mesh { get; protected set; }

	/// <summary>transform (position, scale and rotation) of current element</summary>
	public RenderableTransform transform = new(float2.zero);
	
	/// <summary>set to true if data updates frequently <br/>default is false <br/>affects on caching in some elements</summary>
	public bool isDynamic = false;

	public Material? material;

	public RenderableBase(ChartsCanvas owner) {
		canvas = owner;
		mesh = canvas.factory.CreateMesh();
	}
	
	public void Render() {
		beforeRender();
		if (CheckMeshForUpdate()) GenerateMesh();
		DrawMesh(material);
		AfterDraw();
	}
	//protected abstract void Render();

	protected virtual bool CheckMeshForUpdate() => isDynamic;

	public abstract void GenerateMesh();

	protected virtual void AfterDraw() {}

	protected virtual void Destroy() {
		mesh?.Dispose();
		mesh = null;
	}
}