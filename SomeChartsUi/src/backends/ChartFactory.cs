using FreeTypeSharp;
using MathStuff.vectors;
using SomeChartsUi.ui;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.layers;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;

namespace SomeChartsUi.backends;

public abstract class ChartFactory {
	public ChartsCanvas owner = null!;

	public virtual Mesh CreateMesh() => throw new NotSupportedException();
	public virtual Shader CreateShader(string name, string vertex, string fragment) => new(name, vertex, fragment);
	public virtual CanvasLayer CreateLayer(string name) => new(owner, name);

	public abstract Texture CreateTexture(string path);
	public abstract Texture CreateTexture(float2 size);
	public abstract FontTextures CreateFontTextureAtlas(FreeTypeFaceFacade face, uint resolution = 32);

	public abstract Material CreateTextMaterial();
	public abstract PostProcessor CreatePostProcessor(Material mat);

	public abstract TextMesh CreateTextMesh(RenderableBase renderable);
}