using System;
using System.Runtime.InteropServices;
using SkiaSharp;

namespace SomeChartsUiAvalonia.backends;

public static unsafe class SkiaSharpApiUtils {
	[DllImport("libSkiaSharp", CallingConvention = CallingConvention.Cdecl)]
	public static extern void sk_canvas_draw_points(
		IntPtr canvasHandle,
		SKPointMode mode,
		IntPtr length,
		SKPoint* pointsPtr,
		IntPtr paintHandle);

	[DllImport("libSkiaSharp", CallingConvention = CallingConvention.Cdecl)]
	public static extern void sk_canvas_draw_vertices(
		IntPtr canvasHandle,
		IntPtr verticesHandle,
		SKBlendMode mode,
		IntPtr paintHandle);

	[DllImport("libSkiaSharp", CallingConvention = CallingConvention.Cdecl)]
	internal static extern IntPtr sk_vertices_make_copy(
		SKVertexMode vmode,
		int vertexCount,
		SKPoint* positions,
		SKPoint* texs,
		uint* colors,
		int indexCount,
		ushort* indices);

	[DllImport("libSkiaSharp", CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_refcnt_safe_unref(IntPtr refcnt);

	[DllImport("libSkiaSharp", CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_vertices_unref(IntPtr cvertices);

	public static SKShader CompileShader(string skslSource, out string errors) {
		SKRuntimeEffect effect = SKRuntimeEffect.Create(skslSource, out errors);

		SKShader sh = effect.ToShader(false);

		return sh;
	}
	public static SKRuntimeEffect CompileRuntimeEffect(string skslSource, out string errors) => SKRuntimeEffect.Create(skslSource, out errors);

	public static void DrawPointsUnsafe(this SKCanvas canvas, SKPointMode mode, SKPoint* ptr, int length, SKPaint paint) =>
		sk_canvas_draw_points(canvas.Handle, mode, (IntPtr)length, ptr, paint.Handle);

	public static void DrawVerticesUnsafe(this SKCanvas canvas, SKBlendMode mode, IntPtr vertices, SKPaint paint) =>
		sk_canvas_draw_vertices(canvas.Handle, vertices, mode, paint.Handle);

	public static void DrawVerticesUnsafe(this SKCanvas canvas, SKBlendMode mode, SKPoint* pos, SKColor* col, ushort* ind, int vCount, int indCount, SKPaint paint) {
		IntPtr vertices = CopyVertices(pos, null, col, ind, vCount, indCount);
		canvas.DrawVerticesUnsafe(mode, vertices, paint);
		sk_vertices_unref(vertices);
	}

	public static void DrawVerticesUnsafe(this SKCanvas canvas, SKBlendMode mode, SKPoint* pos, SKPoint* uvs, SKColor* col, ushort* ind, int vCount, int indCount, SKPaint paint) {
		IntPtr vertices = CopyVertices(pos, uvs, col, ind, vCount, indCount);
		canvas.DrawVerticesUnsafe(mode, vertices, paint);
		sk_vertices_unref(vertices);
	}

	public static IntPtr CopyVertices(SKPoint* pos, SKPoint* uvs, SKColor* col, ushort* ind, int vCount, int indCount) =>
		sk_vertices_make_copy(SKVertexMode.Triangles, vCount, pos, uvs, (uint*)col, indCount, ind);
}