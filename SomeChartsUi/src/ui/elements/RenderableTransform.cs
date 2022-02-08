using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.elements; 

public class RenderableTransform {
	public float2 position;
	public float2 scale;
	public float3 rotation;
	public TransformType type;

	public RenderableTransform(float2 position, float2 scale, float3 rotation, TransformType type = TransformType.worldSpace) {
		this.position = position;
		this.scale = scale;
		this.rotation = rotation;
		this.type = type;
	}
	
	public RenderableTransform(float2 position, float scale = 1, float rotation = 0, TransformType type = TransformType.worldSpace) {
		this.position = position;
		this.scale = scale;
		this.rotation = rotation;
		this.type = type;
	}
}

public enum TransformType : byte {
	/// coordinates from -∞ to +∞
	worldSpace = 0, 
	
	/// coordinates from 0 to screen width or height
	screenSpace = 1, 
	
	/// coordinates from 0 to 1
	viewportSpace = 2,
}