using MathStuff.vectors;
using SomeChartsUi.utils.shaders;

namespace SomeChartsUiAvalonia.impl.opengl.shaders; 

public class UberShaderSettings {
	public static GlShader shader => GlShaders.uber;
	public static UberShaderSettings current = new();

	public bool fxaa = true;
	public bool fxaa_showEdges = false;
	public float fxaa_lumaThreshold = .5f;
	public float fxaa_mulReduce = 1 / 8f;
	public float fxaa_minReduce = 1 / 128f;
	public float fxaa_maxSpan = 8;

	public bool bloom = false;
	public float bloom_brightness = 2;
	public float bloom_step = .0002f;
	public float2 bloom_scale = float2.one;

	public Material GenerateMaterial() {
		Material mat = new(shader);
		
		mat.SetProperty("enableFxaa", fxaa);
		mat.SetProperty("u_showEdges", fxaa_showEdges);
		mat.SetProperty("u_lumaThreshold", fxaa_lumaThreshold);
		mat.SetProperty("u_mulReduce", fxaa_mulReduce);
		mat.SetProperty("u_minReduce", fxaa_minReduce);
		mat.SetProperty("u_maxSpan", fxaa_maxSpan);
		
		mat.SetProperty("enableBloom", bloom);
		mat.SetProperty("brightness", bloom_brightness);
		mat.SetProperty("step", bloom_step);
		mat.SetProperty("scale", bloom_scale);

		return mat;
	}
}