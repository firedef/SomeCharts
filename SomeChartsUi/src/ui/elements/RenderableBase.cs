using MathStuff.vectors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.layers.render;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils.mesh.construction;

namespace SomeChartsUi.ui.elements;

/// <summary>base class of all canvas elements</summary>
public abstract partial class RenderableBase {
	/// <summary>action will happen every frame and before draw<br/><br/>
	/// <see cref="updateFrameSkip"/> will not affect on this</summary>
	public Action beforeRender = () => { };

	/// <summary>if object is dirty, mesh will re-generate at next frame</summary>
	public bool isDirty;

	/// <summary>set to true if data updates frequently <br/>default is false <br/>affects on caching in some elements</summary>
	public bool isDynamic = false;

	/// <summary>frame skip on dynamic update</summary>
	public int updateFrameSkip = 8;
	
	/// <summary>frame skip on dynamic update</summary>
	public int updateRareFrameSkip = 64;
	protected int framesCount;

	public bool isTransparent = false;

	/// <summary>material of mesh <br/><br/>if null, renderer will use basic material</summary>
	public Material? material;

	/// <summary>transform (position, scale and rotation) of current element</summary>
	public Transform transform = new(float2.zero);
	
	/// <summary>owner canvas</summary>
	public ChartsCanvas canvas { get; }

	/// <summary>mesh to be rendered</summary>
	public Mesh? mesh { get; protected set; }

	public RenderableBase(ChartsCanvas owner) {
		canvas = owner;
		mesh = canvas.factory.CreateMesh();
		
		// pick random frame offset, so objects added in one frame will not re-generate at the same time
		framesCount = MeshUtils.rnd.Next(100);
	}

	public void PreRender() {
		framesCount++;
		beforeRender();
		OnFrequentUpdate();
		if (framesCount % (updateRareFrameSkip + 1) == 0) OnRareUpdate();
		if (!CheckMeshForUpdate()) return;
		GenerateMesh();
		isDirty = false;
	}

	public abstract void Render(RenderLayerId pass);

	protected virtual bool CheckMeshForUpdate() => isDirty || isDynamic && framesCount % (updateFrameSkip + 1) == 0;

	/// <summary>re-generate mesh <br/><br/>mesh is always not-null, so don`t forget to clear it and call mesh.OnModified() when complete</summary>
	protected abstract void GenerateMesh();

	/// <summary>called every frame after render <br/><br/>usable for rendering multiple meshes, like <see cref="TextMesh"/></summary>
	public virtual void PostRender() { }

	/// <summary>called every frame after beforeRender()</summary>
	protected virtual void OnFrequentUpdate() {}
	
	/// <summary>called after OnFrequentUpdate and before GenerateMesh dynamically, using updateRareFrameSkip</summary>
	protected virtual void OnRareUpdate() {}

	protected virtual void Destroy() {
		mesh?.Dispose();
		mesh = null;
	}

	/// <summary>re-generate mesh at next frame</summary>
	public void MarkDirty() => isDirty = true;
}