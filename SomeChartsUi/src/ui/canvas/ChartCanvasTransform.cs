using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.ui.canvas.animation;

namespace SomeChartsUi.ui.canvas;

public class ChartCanvasTransform {

	private TimeSpan _lastUpdate;
	public CanvasAnimVariable<float2> position = new(float2.zero, animationSpeed: .025f);
	public CanvasAnimVariable<float3> rotation = new(0);

	public rect screenBounds;
	public CanvasAnimVariable<float2> zoom = new(float2.one, animationSpeed: .025f);
	public rect worldBounds { get; private set; }

	public void Update() {
		TimeSpan now = DateTime.Now.TimeOfDay;
		float deltaTime = (float)(now - _lastUpdate).TotalMilliseconds;
		_lastUpdate = now;

		position.OnUpdate(deltaTime);
		zoom.OnUpdate(deltaTime);
		rotation.OnUpdate(deltaTime);

		worldBounds = screenBounds.ToWorld(position, zoom);
	}

	public void SetAnimToCurrent() {
		position.OnUpdate(10000);
		zoom.OnUpdate(10000);
		rotation.OnUpdate(10000);
	}

#region transform

	public void Translate(float2 v) => position.currentValue += v;
	public void Zoom(float2 v) => zoom.currentValue += v;
	public void Zoom(float v) => Zoom(new float2(v));
	public void Rotate(float3 v) => rotation.currentValue += v;
	public void Rotate(float v) => Rotate(new float3(0, 0, v));

	/// <summary>
	///     scale canvas at point
	/// </summary>
	/// <param name="v">scale</param>
	/// <param name="pivot">pivot in world-space coordinates</param>
	public void ZoomAt(float2 v, float2 pivot) {
		Zoom(v);
		Translate((pivot - position) * v);
	}

#endregion transform
}