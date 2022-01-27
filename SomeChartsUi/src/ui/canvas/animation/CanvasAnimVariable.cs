using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.canvas.animation; 

public class CanvasAnimVariable<T> : ICanvasUpdate where T : unmanaged {
	public T currentValue;
	public T animatedValue;
	public float animationSpeed;

	public CanvasAnimVariable(T currentValue = default, T animatedValue = default, float animationSpeed = 1) {
		this.currentValue = currentValue;
		this.animatedValue = animatedValue;
		this.animationSpeed = animationSpeed;
	}

	public void OnUpdate(float deltatime) {
		float t = Math.Clamp(animationSpeed * deltatime, 0, 1);
		animatedValue = Lerp(animatedValue, currentValue, t);
	}
	
	private static T Lerp(T a, T b, float t) => (dynamic)a * (1 - t) + (dynamic)b * t;
}