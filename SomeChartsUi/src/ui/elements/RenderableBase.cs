using SomeChartsUi.data;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.elements; 

/// <summary>
/// base class of all canvas elements
/// </summary>
public abstract partial class RenderableBase {
	public ChartsCanvas canvas { get; private set; } = null!;

	public ChartProperty<RenderableTransform> transform = new RenderableTransform(float2.zero);
	
	public void Render(ChartsCanvas owner) {
		canvas = owner;
		Render();
	}
	protected abstract void Render();
}