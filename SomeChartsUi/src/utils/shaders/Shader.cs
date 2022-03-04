namespace SomeChartsUi.utils.shaders; 

public class Shader {
	public readonly string name;
	public string vertexShaderSrc;
	public string fragmentShaderSrc;

	public ShaderUniform[] uniforms = Array.Empty<ShaderUniform>();

	public Shader(string name, string vertexShaderSrc, string fragmentShaderSrc) {
		this.name = name;
		this.vertexShaderSrc = vertexShaderSrc;
		this.fragmentShaderSrc = fragmentShaderSrc;
	}
}