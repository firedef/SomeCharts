using System;
using FreeTypeSharp;
using MathStuff.vectors;
using SomeChartsUi.backends;
using SomeChartsUi.ui;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;
using SomeChartsUiAvalonia.impl.opengl.shaders;
using SomeChartsUiAvalonia.impl.opengl.text;

namespace SomeChartsUiAvalonia.impl.opengl.backend;

public class GlChartFactory : ChartFactory {
	public override Mesh CreateMesh() => new GlMesh();
	public override Shader CreateShader(string name, string vertex, string fragment) => new GlShader(name, vertex, fragment);
	public override Texture CreateTexture(string path) => new GlTexture(path);
	public override Texture CreateTexture(float2 size) => throw new NotImplementedException();
	public override FontTextures CreateFontTextureAtlas(FreeTypeFaceFacade face, uint resolution) => new GlFontTextures(face, resolution);
	public override Material CreateTextMaterial() => new(GlShaders.basicText);
	public override PostProcessor CreatePostProcessor(Material? mat) => new GlPostProcessor(owner) {material = mat};
	public override TextMesh CreateTextMesh(RenderableBase renderable) => new FreeTypeTextMesh(renderable);
}