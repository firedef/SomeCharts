using System;
using FreeTypeSharp;
using JetBrains.Annotations;
using MathStuff.vectors;
using SomeChartsUi.backends;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;
using SomeChartsUiAvalonia.utils;
using SomeChartsUiAvalonia.utils.collections;

namespace SomeChartsUiAvalonia.backends; 

public class GlChartFactory : ChartFactory {
	public override Mesh CreateMesh() => new GlMesh();
	public override Shader CreateShader(string name, string vertex, string fragment) => new GlShader(name, vertex, fragment);
	public override Texture CreateTexture(string path) => new GlTexture(path);
	public override Texture CreateTexture(float2 size) => throw new NotImplementedException();
	public override unsafe FontTextures CreateFontTextureAtlas(FreeTypeFaceFacade face) => new GlFontTextures(face);
}