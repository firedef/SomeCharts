using SomeChartsUi.themes.colors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.elements;

public abstract partial class RenderableBase {
	protected float2 canvasPosition => canvas.transform.position;
	protected float2 canvasZoom => canvas.transform.zoom;
	protected float canvasRotation => canvas.transform.rotation;

	protected ChartCanvasRenderer renderer => canvas.renderer;
	
	protected unsafe void DrawVertices(float2* points, float2* uvs, color* colors, ushort* indexes, int vertexCount, int indexCount) => 
		renderer.backend.DrawMesh(points, uvs, colors, indexes, vertexCount, indexCount);
	
	protected void DrawVertices(float2[] points, float2[]? uvs, color[]? colors, ushort[] indexes) => 
		renderer.backend.DrawMesh(points, uvs, colors, indexes);

	protected void DrawText(string txt, float2 pos, color col, FontData font, float scale = 12) =>
		renderer.backend.DrawText(txt, pos, col, font, scale);
}