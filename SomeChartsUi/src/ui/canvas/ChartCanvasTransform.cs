using System.Numerics;
using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.ui.canvas.animation;

namespace SomeChartsUi.ui.canvas;

public class ChartCanvasTransform {
	private TimeSpan _lastUpdate;
	
	public CanvasAnimVariable<float2> position = new(float2.zero, animationSpeed: .025f);
	public CanvasAnimVariable<float3> rotation = new(0);
	public CanvasAnimVariable<float2> scale = new(float2.one, animationSpeed: .025f);

	public Matrix4x4 projectionMatrix;
	public Matrix4x4 viewMatrix;

	public rect screenBounds;
	public rect worldBounds { get; private set; }

	public void Update() {
		TimeSpan now = DateTime.Now.TimeOfDay;
		float deltaTime = (float)(now - _lastUpdate).TotalMilliseconds;
		_lastUpdate = now;

		position.OnUpdate(deltaTime);
		scale.OnUpdate(deltaTime);
		rotation.OnUpdate(deltaTime);

		worldBounds = screenBounds.ToWorld(position, scale);
		
		RecalculateMatrix();
	}

	public void SetAnimToCurrent() {
		position.OnUpdate(10000);
		scale.OnUpdate(10000);
		rotation.OnUpdate(10000);
	}

	public void RecalculateMatrix() {
		float z = 1 / scale.animatedValue.x;
		
		float3 camPos = new(position.animatedValue, z);
		projectionMatrix = Matrix4x4.CreateOrthographic(screenBounds.width * z, screenBounds.height * z, .001f, 10000);
		viewMatrix = Matrix4x4.CreateLookAt(new(-camPos.x, camPos.y, z), new(-camPos.x, camPos.y, 0), new(0, -1, 0));
		
		//TODO: fix perspective
		// if (perspectiveMode) {
		// 	projection = Matrix4x4.CreatePerspectiveFieldOfView(90 / 180f * MathF.PI, owner.transform.screenBounds.width / owner.transform.screenBounds.height, .001f, 10000f);
		// 	z *= 100;
		// }
	}

#region transform

	public void Translate(float2 v) => position.currentValue += v;
	public void Zoom(float2 v) => scale.currentValue += v;
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