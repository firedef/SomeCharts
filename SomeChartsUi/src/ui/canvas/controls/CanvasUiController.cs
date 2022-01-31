using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.canvas.controls;

public class CanvasUiController : ChartCanvasControllerBase {
	private float2 _start;
	private float2 _origin;

	public float zoomSpeed = .2f;

	public CanvasUiController(ChartsCanvas owner) : base(owner) { }

	public override void OnMouseMove(MouseState state) {
		float2 pointerPos = state.pos;
		pointerPos.FlipY();
		if (state.capture == null) return;

		float speed = 1;
		if ((state.modifiers & keymods.alt) != 0) speed = 4;

		float2 mov = (pointerPos - _start) / owner.transform.zoom.currentValue * speed + _origin;

		SetPosition(mov);

		_start = pointerPos;
		_origin = mov;
	}
	public override void OnMouseDown(MouseState state) {
		float2 pointerPos = state.pos;
		pointerPos.FlipY();
		state.capture = this;

		_start = pointerPos;

		_origin = owner.transform.position.currentValue;
		//Cursor = Cursor.Parse("Hand");
	}
	public override void OnMouseUp(MouseState state) {
		state.capture = null;
		//Cursor = Cursor.Default;
	}

	public override void OnMouseScroll(MouseState state) {
		float2 pointerPos = state.pos;
		pointerPos.FlipY();
		bool disableAnim = false;

		float2 zoomAdd = zoomSpeed * state.wheel.yy;

		if ((state.modifiers & keymods.shift) != 0) zoomAdd.x = 0;
		if ((state.modifiers & keymods.ctrl) != 0) zoomAdd.y = 0;
		if ((state.modifiers & keymods.alt) != 0) {
			zoomAdd.x *= 2;
			zoomAdd.y *= 2;
			disableAnim = true;
		}

		float2 oldScale = owner.transform.zoom.currentValue;
		float2 newScale = float2.Clamp(oldScale * (1 + zoomAdd), .001f, 1.5f);
		// float2 newScale = new(Math.Clamp(oldScale.x * (1 + zoomAdd.x), .001f, 1.5f), Math.Clamp(oldScale.y * (1 + zoomAdd.y), .001f, 1.5f));
		zoomAdd = 1 - newScale / oldScale;

		SetZoom(newScale);
		float2 posOffset = new(pointerPos.x * zoomAdd.x / newScale.x,
		                       (owner.transform.screenBounds.height + pointerPos.y) * zoomAdd.y / newScale.y);

		Move(posOffset);

		if (disableAnim) owner.transform.SetAnimToCurrent();
	}

	public override void OnUpdate(float deltatime) { }

	public override void OnKey(keycode key, keymods mods) {
		if (key == keycode.e) Rotate(.1f);
		if (key == keycode.q) Rotate(-.1f);
		
		if (key == keycode.w) Move(new(+000,+100));
		if (key == keycode.s) Move(new(+000,-100));
		if (key == keycode.d) Move(new(+100,+000));
		if (key == keycode.a) Move(new(-100,+000));
	}
}