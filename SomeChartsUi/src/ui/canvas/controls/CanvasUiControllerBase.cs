using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.canvas.controls;

public abstract class CanvasUiControllerBase : ChartCanvasControllerBase {
	private float2 _start;
	private float2 _origin;

	public float zoomSpeed = .1f;

	public CanvasUiControllerBase(ChartsCanvas owner) : base(owner) { }

	protected abstract void Capture();
	protected abstract void ReleaseCapture();
	protected abstract bool IsCaptured();
	protected abstract void SetCursor(string name);

	//TODO: add rotation support
	public override void OnMouseMove(MouseState state) {
		float2 pointerPos = state.pos;
		pointerPos.FlipY();
		if (!IsCaptured()) return;

		float speed = 1;
		if ((state.modifiers & keymods.alt) != 0) speed = 4;

		float2 mov = (pointerPos - _start) / owner.transform.zoom.currentValue * speed;
		Move(mov);

		_start = pointerPos;
		_origin += mov;
	}
	public override void OnMouseDown(MouseState state) {
		float2 pointerPos = state.pos;
		pointerPos.FlipY();
		Capture();

		_start = pointerPos;

		_origin = owner.transform.position.currentValue;
		SetCursor("Hand");
	}
	public override void OnMouseUp(MouseState state) {
		ReleaseCapture();
		SetCursor("Arrow");
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
		zoomAdd = 1 - newScale / oldScale;
		
		pointerPos.y += owner.transform.screenBounds.height;
		SetZoom(newScale);
		float2 posOffset = pointerPos * zoomAdd / newScale;
		
		Move(posOffset);

		if (disableAnim) owner.transform.SetAnimToCurrent();
	}

	protected float2 ScreenToWorld(float2 screen) {
		float2 zoom = owner.transform.zoom.currentValue;
		float2 pos = owner.transform.position.currentValue;
		screen -= pos;
		screen /= zoom;
		return screen;
	}
	
	protected float2 ScreenToWorldFixedPos(float2 screen) {
		float2 zoom = owner.transform.zoom.currentValue;
		float2 pos = owner.transform.position.currentValue;
		screen /= zoom;
		return screen;
	}

	public override void OnUpdate(float deltatime) { }

	public override void OnKey(keycode key, keymods mods) {
		if (key == keycode.e) owner.GetLayer("normal")!.elements[0].transform.rotation.z += .1f;
		if (key == keycode.q) owner.GetLayer("normal")!.elements[0].transform.rotation.z -= .1f;
		
		if (key == keycode.x) owner.GetLayer("normal")!.elements[0].transform.rotation.x += .1f;
		if (key == keycode.z) owner.GetLayer("normal")!.elements[0].transform.rotation.x -= .1f;
		
		if (key == keycode.v) owner.GetLayer("normal")!.elements[0].transform.rotation.y += .1f;
		if (key == keycode.c) owner.GetLayer("normal")!.elements[0].transform.rotation.y -= .1f;
		// if (key == keycode.e) Rotate(.1f);
		// if (key == keycode.q) Rotate(-.1f);
		//
		// if (key == keycode.x) Rotate(new float3(.1f,0,0));
		// if (key == keycode.z) Rotate(new float3(-.1f,0,0));
		//
		// if (key == keycode.v) Rotate(new float3(0,.1f,0));
		// if (key == keycode.c) Rotate(new float3(0,.1f,0));
		
		if (key == keycode.w) Move(new(+000,+100));
		if (key == keycode.s) Move(new(+000,-100));
		if (key == keycode.d) Move(new(+100,+000));
		if (key == keycode.a) Move(new(-100,+000));
	}
}