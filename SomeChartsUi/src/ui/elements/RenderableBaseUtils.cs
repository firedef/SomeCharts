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
		renderer.backend.DrawMesh(points, uvs, colors, indexes, vertexCount, indexCount, transform.Get(this));
	
	protected void DrawVertices(float2[] points, float2[]? uvs, color[]? colors, ushort[] indexes) => 
		renderer.backend.DrawMesh(points, uvs, colors, indexes, transform.Get(this));

	protected void DrawText(string txt, float2 pos, color col, FontData font, float scale = 12) =>
		renderer.backend.DrawText(txt, col, font, transform.Get(this) + new RenderableTransform(pos, scale, float3.zero));
	
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

	protected (float start, int count) GetStartCountIndexes((float start, float end) positions, float size) =>
		(MathF.Ceiling(positions.start / size) * size,
		(int)Math.Floor((positions.end - positions.start) / size) + 1);
	
	protected (float start, float end) GetStartEndPos(float2 startLim, float2 endLim, Orientation orientation) {
		float2 vec = (orientation & Orientation.vertical) != 0 ? new(0, 1) : new(1, 0);// vertical : horizontal
		return GetStartEndPos((startLim * vec).sum, (endLim * vec).sum, orientation);
	}
	
	protected (float start, float end) GetStartEndPos(float startLim, float endLim, Orientation orientation) {
		float2 s = 1/canvasZoom;
		RenderableTransform tr = transform.Get(this);
		if ((orientation & Orientation.vertical) != 0)
			return (orientation & Orientation.reversed) != 0 
				? (math.max(startLim, canvas.transform.worldBounds.top - tr.position.y), math.min(endLim, canvas.transform.worldBounds.bottom - tr.position.y)) 
				: (math.max(startLim, canvas.transform.worldBounds.bottom - tr.position.y), math.min(endLim, canvas.transform.worldBounds.top - tr.position.y));
		return (orientation & Orientation.reversed) != 0 
			? (math.max(startLim, canvas.transform.worldBounds.right - tr.position.x), math.min(endLim, canvas.transform.worldBounds.left - tr.position.x)) 
			: (math.max(startLim, canvas.transform.worldBounds.left - tr.position.x), math.min(endLim, canvas.transform.worldBounds.right - tr.position.x));
	}
	
	protected int GetDownsampleX(int downsampleMul, int sub = 2) => (int)math.max(math.log2(downsampleMul / canvas.transform.zoom.animatedValue.x), sub) - sub;
	protected int GetDownsampleY(int downsampleMul, int sub = 2) => (int)math.max(math.log2(downsampleMul / canvas.transform.zoom.animatedValue.y), sub) - sub;
}