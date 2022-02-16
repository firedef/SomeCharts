using SomeChartsUi.data;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.elements; 

/// <summary>base class of all canvas elements</summary>
public abstract partial class RenderableBase {
	/// <summary>owner canvas</summary>
	public ChartsCanvas canvas { get; private set; } = null!;

	/// <summary>transform (position, scale and rotation) of current element</summary>
	public ChartProperty<RenderableTransform> transform = new RenderableTransform(float2.zero);
	
	/// <summary>set to true if data updates frequently <br/>default is false <br/>affects on caching in some elements</summary>
	public bool isDynamic = false;
	
	public void Render(ChartsCanvas owner) {
		canvas = owner;
		Render();
	}
	protected abstract void Render();
}