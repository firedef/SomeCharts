using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.elements; 

public record RenderableTransform {
	public float3 position;
	public float3 scale;
	public float3 rotation;
	public TransformType type;

	public RenderableTransform(float3 position, float3 scale, float3 rotation, TransformType type = TransformType.worldSpace) {
		this.position = position;
		this.scale = scale;
		this.rotation = rotation;
		this.type = type;
	}
	
	public RenderableTransform(float3 position, float scale = 1, float rotation = 0, TransformType type = TransformType.worldSpace) {
		this.position = position;
		this.scale = scale;
		this.rotation = rotation;
		this.type = type;
	}

	public static RenderableTransform operator +(RenderableTransform a, RenderableTransform b) => new(a.position + b.position, a.scale * b.scale, a.rotation + b.rotation, a.type);
}

public enum TransformType : byte {
	/// coordinates from -∞ to +∞
	worldSpace = 0, 
	
	/// coordinates from 0 to screen width or height
	screenSpace = 1, 
	
	/// coordinates from 0 to 1
	viewportSpace = 2,
}