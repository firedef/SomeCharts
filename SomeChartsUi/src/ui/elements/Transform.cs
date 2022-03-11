using System.Numerics;
using MathStuff.vectors;

namespace SomeChartsUi.ui.elements;

public record Transform {
	public float3 position;
	public float3 rotation;
	public float3 scale;
	public Matrix4x4 modelMatrix { get; protected set; } = Matrix4x4.Identity;
	public TransformType type;

	public Transform(float3 position, float3 scale, float3 rotation, TransformType type = TransformType.worldSpace) {
		this.position = position;
		this.scale = scale;
		this.rotation = rotation;
		this.type = type;
	}

	public Transform(float3 position, float scale = 1, float rotation = 0, TransformType type = TransformType.worldSpace) {
		this.position = position;
		this.scale = scale;
		this.rotation = rotation;
		this.type = type;
	}

	public static Transform operator +(Transform a, Transform b) => new(a.position + b.position, a.scale * b.scale, a.rotation + b.rotation, a.type);

	/// <summary>recalculate model matrix <br/><br/>does not using transform type</summary>
	public void RecalculateMatrix() {
		Matrix4x4 rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(rotation.x, rotation.y, rotation.z);
		Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(new Vector3(-scale.x, scale.y, scale.z));
		Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(new(-position.x, -position.y, position.z));
		modelMatrix = rotationMatrix * scaleMatrix * translationMatrix;
	}

	public void SetPosition(float3 v) {
		if (position == v) return;
		position = v;
		RecalculateMatrix();
	}
	
	public void SetRotation(float3 v) {
		if (rotation == v) return;
		rotation = v;
		RecalculateMatrix();
	}
	
	public void SetScale(float3 v) {
		if (scale == v) return;
		scale = v;
		RecalculateMatrix();
	}
	
	public void SetRotation(float v) => SetRotation(new float3(0, 0, v));
	public void SetScale(float v) => SetScale(new float3(v));
}

public enum TransformType : byte {
	/// coordinates from -∞ to +∞
	worldSpace = 0,

	/// coordinates from 0 to screen width or height
	screenSpace = 1,

	/// coordinates from 0 to 1
	viewportSpace = 2
}