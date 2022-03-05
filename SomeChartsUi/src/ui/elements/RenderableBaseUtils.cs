using System.Buffers;
using System.Runtime.InteropServices;
using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.data;
using SomeChartsUi.elements;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils.shaders;

namespace SomeChartsUi.ui.elements;

public abstract partial class RenderableBase {
	protected float2 canvasPosition => canvas.transform.position;
	protected float2 canvasZoom => canvas.transform.zoom;
	protected float3 canvasRotation => canvas.transform.rotation;

	protected ChartCanvasRenderer renderer => canvas.renderer;
	
	// protected unsafe void DrawVertices(float2* points, float2* uvs, color* colors, ushort* indexes, int vertexCount, int indexCount) => 
	// 	renderer.backend.DrawMesh(points, uvs, colors, indexes, vertexCount, indexCount, transform.Get(this));
	//
	// protected void DrawVertices(float2[] points, float2[]? uvs, color[]? colors, ushort[] indexes) => 
	// 	renderer.backend.DrawMesh(points, uvs, colors, indexes, transform.Get(this));

	protected void DrawMesh(Material? material) {
		canvas.renderer.backend.DrawMesh(mesh!, material, transform);
	}

	protected void DrawText(string txt, float2 pos, color col, FontData font, float scale = 12) =>
		renderer.backend.DrawText(txt, col, font, transform + new RenderableTransform(pos, scale, float3.zero));

	/// <summary>rotate vector by 90 degrees and set length</summary>
	protected static float2 Rot90DegFastWithLen(float2 p, float len) {
		len *= FastInverseSquareRoot(p.x * p.x + p.y * p.y);
		return new(-p.y * len, p.x * len);
	}

	/// <summary>1 / sqrt(num)</summary>
	protected static unsafe float FastInverseSquareRoot(float num) {
		float x2 = num * .5f;
		float y = num;
		long i = *(long*)&y;
		i = 0x5f3759df - (i >> 1);
		y = *(float*)&i;
		y *= 1.5f - x2 * y * y;
		return y;
	}

	/// <summary>get start index and count with 2D frustum culling</summary>
	protected (float start, int count) GetStartCountIndexes((float start, float end) positions, float size) => 
		(MathF.Ceiling(positions.start / size) * size,
		(int)Math.Floor((positions.end - positions.start) / size) + 1);
	
	/// <summary>clamp start and end positions to screen bounds</summary>
	protected (float start, float end) GetStartEndPos(float2 startLim, float2 endLim, Orientation orientation) {
		float2 vec = GetOrientationVector(orientation);
		return GetStartEndPos((startLim * vec).sum, (endLim * vec).sum, orientation);
	}

	/// <summary>clamp start and end positions to screen bounds</summary>
	protected (float start, float end) GetStartEndPos(float startLim, float endLim, Orientation orientation) {
		float2 s = 1/canvasZoom;
		RenderableTransform tr = transform;
		if ((orientation & Orientation.vertical) != 0)
			return (orientation & Orientation.reversed) != 0 
				? (math.max(startLim, canvas.transform.worldBounds.top - tr.position.y), math.min(endLim, canvas.transform.worldBounds.bottom - tr.position.y)) 
				: (math.max(startLim, canvas.transform.worldBounds.bottom - tr.position.y), math.min(endLim, canvas.transform.worldBounds.top - tr.position.y));
		return (orientation & Orientation.reversed) != 0 
			? (math.max(startLim, canvas.transform.worldBounds.right - tr.position.x), math.min(endLim, canvas.transform.worldBounds.left - tr.position.x)) 
			: (math.max(startLim, canvas.transform.worldBounds.left - tr.position.x), math.min(endLim, canvas.transform.worldBounds.right - tr.position.x));
	}

	/// <summary>get preferred downsample for element</summary>
	protected int GetDownsampleX(float downsampleMul, int sub = 2) => (int)math.max(math.log2(downsampleMul / canvas.transform.zoom.animatedValue.x), sub) - sub;
	/// <summary>get preferred downsample for element</summary>
	protected int GetDownsampleY(float downsampleMul, int sub = 2) => (int)math.max(math.log2(downsampleMul / canvas.transform.zoom.animatedValue.y), sub) - sub;
	/// <summary>get preferred downsample for element</summary>
	protected int GetDownsample(Orientation orientation, float downsampleMul, int sub = 2) => (orientation & Orientation.vertical) != 0 
		? GetDownsampleY(downsampleMul, sub) 
		: GetDownsampleX(downsampleMul, sub);

	/// <summary>(0,1) for vertical and (1,0) for horizontal</summary>
	protected static float2 GetOrientationVector(Orientation orientation) => (orientation & Orientation.vertical) != 0 ? new(0, 1) : new(1, 0);// vertical : horizontal

	protected static T[] Rent<T>(int size) => ArrayPool<T>.Shared.Rent(size);
	protected static unsafe T* RentMem<T>(int size) where T : unmanaged => (T*) NativeMemory.Alloc((nuint)((long)size * sizeof(T)));
	
	protected static void Free<T>(T[] arr) => ArrayPool<T>.Shared.Return(arr);
	protected static unsafe void FreeMem<T>(T* arr) where T : unmanaged => NativeMemory.Free(arr);

	public RenderableBase WithPosition(float2 v) {
		((ChartPropertyValue<RenderableTransform>)transform).value.position = v;
		return this;
	}
	
	public RenderableBase WithScale(float2 v) {
		((ChartPropertyValue<RenderableTransform>)transform).value.scale = v;
		return this;
	}
	
	public RenderableBase WithRotation(float v) {
		((ChartPropertyValue<RenderableTransform>)transform).value.rotation = new(0,0,v);
		return this;
	}
}