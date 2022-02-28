namespace SomeChartsUi.utils.shaders; 

public class Shader {
	public readonly string name;
	public readonly string vertexShaderSrc;
	public readonly string fragmentShaderSrc;

	public Shader(string name, string vertexShaderSrc, string fragmentShaderSrc) {
		this.name = name;
		this.vertexShaderSrc = vertexShaderSrc;
		this.fragmentShaderSrc = fragmentShaderSrc;
	}
}