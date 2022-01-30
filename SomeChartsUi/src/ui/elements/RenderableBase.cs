using SomeChartsUi.ui.canvas;

namespace SomeChartsUi.ui.elements; 

/// <summary>
/// base class of all canvas elements
/// </summary>
public abstract partial class RenderableBase {
	protected ChartsCanvas canvas;
	
	public void Render(ChartsCanvas owner) {
		canvas = owner;
		Render();
	}
	protected abstract void Render();
}