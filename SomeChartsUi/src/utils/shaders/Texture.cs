using MathStuff.vectors;

namespace SomeChartsUi.utils.shaders;

public abstract class Texture {

	public Texture(string path) { }
	public abstract float2 size { get; }
}