using SomeChartsUi.ui.canvas.animation;
using SomeChartsUi.utils.rects;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.canvas; 

public class ChartCanvasTransform {
	public CanvasAnimVariable<float2> position = new(float2.zero, animationSpeed: .025f);
	public CanvasAnimVariable<float2> zoom = new(float2.one, animationSpeed: .025f);
	public CanvasAnimVariable<float> rotation = new(0);

	public rect screenBounds;
	public rect worldBounds { get; protected set; }

	private TimeSpan _lastUpdate;

	public void Update() {
		TimeSpan now = DateTime.Now.TimeOfDay;
		float deltaTime = (float) (now - _lastUpdate).TotalMilliseconds;
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
}