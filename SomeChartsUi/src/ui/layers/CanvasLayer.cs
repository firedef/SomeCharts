using SomeChartsUi.themes.colors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;

namespace SomeChartsUi.ui.layers; 

public class CanvasLayer {
	public ChartsCanvas owner;
	public color? background;
	public List<RenderableBase> elements = new();

	public CanvasLayer(ChartsCanvas owner) => this.owner = owner;

	public void AddElement(RenderableBase r) => elements.Add(r);
	public void RemoveElement(RenderableBase r) => elements.Remove(r);

	public void Render() {
		
	}
}