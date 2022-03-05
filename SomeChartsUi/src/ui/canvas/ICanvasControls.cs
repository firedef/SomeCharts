using MathStuff.vectors;

namespace SomeChartsUi.ui.canvas;

//TODO: method arguments is a subject of change
public interface ICanvasControls {
	public void OnMouseMove(MouseState state) {}
	public void OnMouseDown(MouseState state) {}
	public void OnMouseUp(MouseState state) {}
	public void OnMouseScroll(MouseState state) {}
	public void OnKey(keycode key, keymods mods) {}
}

public class MouseState {
	public float2 pos;
	public float2 wheel;
	public PointerButtons buttons;
	public keymods modifiers;

	public MouseState(float2 pos, float2 wheel, PointerButtons buttons, keymods modifiers) {
		this.pos = pos;
		this.wheel = wheel;
		this.buttons = buttons;
		this.modifiers = modifiers;
	}
}