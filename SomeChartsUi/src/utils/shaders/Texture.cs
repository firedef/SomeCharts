using MathStuff.vectors;

namespace SomeChartsUi.utils.shaders; 

public abstract class Texture {
	public abstract float2 size { get; }
	
	public Texture(string path) {}
}