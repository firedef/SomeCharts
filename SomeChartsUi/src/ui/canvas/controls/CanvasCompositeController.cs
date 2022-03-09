namespace SomeChartsUi.ui.canvas.controls;

public class CanvasCompositeController : ChartCanvasControllerBase {
	public List<CanvasCompositeController> controllers = new();

	public CanvasCompositeController(ChartsCanvas owner) : base(owner) { }

	public override void OnUpdate(float deltatime) {
		foreach (CanvasCompositeController ctrl in controllers)
			ctrl.OnUpdate(deltatime);
	}

	public override void OnMouseMove(MouseState state) {
		foreach (CanvasCompositeController ctrl in controllers)
			ctrl.OnMouseMove(state);
	}
	public override void OnMouseDown(MouseState state) {
		foreach (CanvasCompositeController ctrl in controllers)
			ctrl.OnMouseDown(state);
	}
	public override void OnMouseUp(MouseState state) {
		foreach (CanvasCompositeController ctrl in controllers)
			ctrl.OnMouseUp(state);
	}
	public override void OnMouseScroll(MouseState state) {
		foreach (CanvasCompositeController ctrl in controllers)
			ctrl.OnMouseScroll(state);
	}
	public override void OnKey(keycode key, keymods mods) {
		foreach (CanvasCompositeController ctrl in controllers)
			ctrl.OnKey(key, mods);
	}
}