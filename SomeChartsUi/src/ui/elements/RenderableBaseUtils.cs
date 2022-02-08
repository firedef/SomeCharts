using System.Runtime.CompilerServices;
using SomeChartsUi.elements;
using SomeChartsUi.themes.colors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.elements;

public abstract partial class RenderableBase {
	protected float2 canvasPosition => canvas.transform.position;
	protected float2 canvasZoom => canvas.transform.zoom;
	protected float3 canvasRotation => canvas.transform.rotation;

	protected ChartCanvasRenderer renderer => canvas.renderer;
	
	protected unsafe void DrawVertices(float2* points, float2* uvs, color* colors, ushort* indexes, int vertexCount, int indexCount) => 
		renderer.backend.DrawMesh(points, uvs, colors, indexes, vertexCount, indexCount, transform);
	
	protected void DrawVertices(float2[] points, float2[]? uvs, color[]? colors, ushort[] indexes) => 
		renderer.backend.DrawMesh(points, uvs, colors, indexes, transform);

	protected void DrawText(string txt, float2 pos, color col, FontData font, float scale = 12) =>
		renderer.backend.DrawText(txt, col, font, transform);
	
	protected static float2 Rot90DegFastWithLen(float2 p, float len) {
		len *= FastInverseSquareRoot(p.x * p.x + p.y * p.y);
		return new(-p.y * len, p.x * len);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static unsafe float FastInverseSquareRoot(float num) {
		float x2 = num * .5f;
		float y = num;
		long i = *(long*)&y;
		i = 0x5f3759df - (i >> 1);
		y = *(float*)&i;
		y *= 1.5f - x2 * y * y;
		return y;
	}

	protected (float start, float end) GetStartEndPos(float startLim, float endLim, Orientation orientation) {
		float2 s = 1/canvasZoom;
		if ((orientation & Orientation.vertical) != 0)
			return (orientation & Orientation.reversed) != 0 
				? (math.max(startLim, canvas.transform.worldBounds.top - transform.position.y), math.min(endLim, canvas.transform.worldBounds.bottom - transform.position.y)) 
				: (math.max(startLim, canvas.transform.worldBounds.bottom - transform.position.y), math.min(endLim, canvas.transform.worldBounds.top - transform.position.y));
		return (orientation & Orientation.reversed) != 0 
			? (math.max(startLim, canvas.transform.worldBounds.right - transform.position.x), math.min(endLim, canvas.transform.worldBounds.left - transform.position.x)) 
			: (math.max(startLim, canvas.transform.worldBounds.left - transform.position.x), math.min(endLim, canvas.transform.worldBounds.right - transform.position.x));
	}
	
	protected int GetDownsampleX(int downsampleMul, int sub = 2) => (int)math.max(math.log2(downsampleMul / canvas.transform.zoom.animatedValue.x), sub) - sub;
	protected int GetDownsampleY(int downsampleMul, int sub = 2) => (int)math.max(math.log2(downsampleMul / canvas.transform.zoom.animatedValue.y), sub) - sub;
}