using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.canvas;

//TODO: method arguments is a subject of change
public interface ICanvasControls {
	public void OnMouseMove(float2 pointerPos) {}
	public void OnMouseDown(PointerButtons buttons, keymods mods) {}
	public void OnMouseUp(PointerButtons buttons, keymods mods) {}
	public void OnKey(keycode key, keymods mods) {}
}