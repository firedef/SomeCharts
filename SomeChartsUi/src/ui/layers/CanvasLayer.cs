using SomeChartsUi.themes.colors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;

namespace SomeChartsUi.ui.layers; 

public class CanvasLayer {
	public readonly string name;
	public readonly ChartsCanvas owner;
	public indexedColor? background;
	public readonly List<RenderableBase> elements = new();

	public CanvasLayer(ChartsCanvas owner, string name) {
		this.owner = owner;
		this.name = name;
	}

	public void AddElement(RenderableBase r) => elements.Add(r);
	public void RemoveElement(RenderableBase r) => elements.Remove(r);

	public void Render() {
		if (background != null) owner.renderer.backend.ClearScreen(background.Value.GetColor());
		foreach (RenderableBase element in elements) {
			element.Render();
		}
	}
}