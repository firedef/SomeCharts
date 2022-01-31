using SomeChartsUi.themes.colors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils.rects;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.backends; 

/// <summary>
/// all positions and scales are in screen-space transform
/// </summary>
public abstract class ChartsBackendBase {
	public ChartsCanvas owner = null!;
	public ChartCanvasRenderer renderer = null!;

	public abstract unsafe void DrawMesh(float2* points, float2* uvs, color* colors, ushort* indexes, int vertexCount, int indexCount);
	public abstract void DrawMesh(float2[] points, float2[]? uvs, color[]? colors, ushort[] indexes);
	
	public abstract void DrawText(string text, float2 pos, color col, FontData font, float scale = 12);

	public abstract void DrawRect(rect rectangle, color color);
	
	
}