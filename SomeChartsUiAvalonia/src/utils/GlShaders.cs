namespace SomeChartsUiAvalonia.utils;

public static class GlShaders {
	public static readonly GlShader basic = GlShader.LoadFrom("basic", "data/shaders/basic.vert", "data/shaders/basic.frag");
	public static readonly GlShader basicTextured = GlShader.LoadFrom("basicTextured", "data/shaders/basicTextured.vert", "data/shaders/basicTextured.frag");
	public static readonly GlShader fxaa = GlShader.LoadFrom("fxaa", "data/shaders/fxaa.vert", "data/shaders/fxaa.frag");
	public static readonly GlShader bloom = GlShader.LoadFrom("bloom", "data/shaders/bloom.vert", "data/shaders/bloom.frag");
	public static readonly GlShader basicText = GlShader.LoadFrom("text", "data/shaders/text.vert", "data/shaders/text.frag");
	public static readonly GlShader diffuse = GlShader.LoadFrom("diffuse", "data/shaders/diffuse.vert", "data/shaders/diffuse.frag");
}