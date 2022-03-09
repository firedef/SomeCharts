using MathStuff;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;

namespace SomeChartsUi.backends;

/// <summary>
///     all positions and scales are in screen-space transform
/// </summary>
public abstract class ChartsBackendBase {
	public ChartsCanvas owner = null!;
	public ChartCanvasRenderer renderer = null!;

	public abstract void ClearScreen(color col);

	public abstract void DrawMesh(Mesh mesh, Material? material, RenderableTransform transform);
}