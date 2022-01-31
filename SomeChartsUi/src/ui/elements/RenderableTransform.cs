using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.elements; 

public class RenderableTransform {
	public float2 position;
	public float2 scale;
	public float rotation;

	public RenderableTransform(float2 position, float2 scale, float rotation = 0) {
		this.position = position;
		this.scale = scale;
		this.rotation = rotation;
	}
	
	public RenderableTransform(float2 position, float scale = 1, float rotation = 0) {
		this.position = position;
		this.scale = scale;
		this.rotation = rotation;
	}
}