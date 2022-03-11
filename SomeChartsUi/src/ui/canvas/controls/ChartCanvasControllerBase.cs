using MathStuff.vectors;

namespace SomeChartsUi.ui.canvas.controls;

public abstract class ChartCanvasControllerBase : ICanvasUpdate, ICanvasControls {
	protected ChartsCanvas owner;

	protected ChartCanvasControllerBase(ChartsCanvas owner) => this.owner = owner;

	public virtual void OnMouseMove(MouseState state) { }
	public virtual void OnMouseDown(MouseState state) { }
	public virtual void OnMouseUp(MouseState state) { }
	public virtual void OnMouseScroll(MouseState state) { }
	public virtual void OnKey(keycode key, keymods mods) { }

	public abstract void OnUpdate(float deltatime);

	protected void Move(float2 dir) => owner.transform.position += dir;
	protected void SetPosition(float2 pos) => owner.transform.position.Set(pos);

	protected void Move(float radians, float amount) => owner.transform.position += float2.SinCos(radians, amount);

	protected void Zoom(float2 dir) => owner.transform.scale += dir;
	protected void SetZoom(float2 pos) => owner.transform.scale.Set(pos);

	protected void ResetZoomSqueeze() => SetZoom(owner.transform.scale.currentValue.avg);

	protected void ResetPosition() => SetPosition(0);
	protected void ResetZoom() => SetZoom(0);
	protected void ResetTransform() {
		ResetPosition();
		ResetZoom();
	}

	protected void SetRotation(float r) => owner.transform.rotation.currentValue = r;
	protected void Rotate(float r) => owner.transform.rotation.currentValue.z += r;
	protected void Rotate(float3 r) => owner.transform.rotation.currentValue += r;

	protected void UpdateTransformAnim() {
		owner.transform.position.OnUpdate(1000);
		owner.transform.scale.OnUpdate(1000);
		owner.transform.rotation.OnUpdate(1000);
	}
}