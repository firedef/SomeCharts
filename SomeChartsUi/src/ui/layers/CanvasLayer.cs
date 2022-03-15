using SomeChartsUi.themes.colors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.layers.render;

namespace SomeChartsUi.ui.layers;

public class CanvasLayer {
	public readonly List<RenderableBase> elements = new();
	public readonly string name;
	public readonly ChartsCanvas owner;
	public indexedColor? background;

	public CanvasLayer(ChartsCanvas owner, string name) {
		this.owner = owner;
		this.name = name;
	}

	public void AddElement(RenderableBase r) => elements.Add(r);
	public void RemoveElement(RenderableBase r) => elements.Remove(r);

	public void PreRender() {
		foreach (RenderableBase element in elements) element.PreRender();
	}
	
	public void PostRender() {
		foreach (RenderableBase element in elements) element.PostRender();
	}
	
	public void Render(RenderLayerId pass) {
		if (pass == RenderLayerId.opaque && background != null) owner.renderer.backend.ClearScreen(background.Value.GetColor());
		foreach (RenderableBase element in elements) element.Render(pass);
	}
}