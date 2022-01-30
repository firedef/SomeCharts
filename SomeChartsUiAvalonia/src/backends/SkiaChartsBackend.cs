using System;
using Avalonia.Platform;
using Avalonia.Skia;
using SkiaSharp;
using SkiaSharp.HarfBuzz;
using SomeChartsUi.backends;
using SomeChartsUi.themes.colors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils.rects;
using SomeChartsUi.utils.vectors;
using SomeChartsUiAvalonia.utils;

namespace SomeChartsUiAvalonia.backends; 

public class SkiaChartsBackend : ChartsBackendBase, IDisposable {
	private SKCanvas? _canvas;
	private SKPaint? _paint;
	
	public override unsafe void DrawMesh(float2* points, float2* uvs, color* colors, ushort* indexes, int vertexCount, int indexCount) {
		_canvas!.DrawVerticesUnsafe(SKBlendMode.Modulate, (SKPoint*)points, (SKPoint*)uvs, (SKColor*)colors, indexes, vertexCount, indexCount, _paint!);
	}
	public override unsafe void DrawMesh(float2[] points, float2[]? uvs, color[]? colors, ushort[] indexes) {
		fixed(float2* pointsPtr = points)
			fixed(float2* uvsPtr = uvs)
				fixed(color* colorsPtr = colors)
					fixed(ushort* indexesPtr = indexes)
						DrawMesh(pointsPtr, uvsPtr, colorsPtr, indexesPtr, points.Length, indexes.Length);
	}
	public override void DrawText(string text, float2 pos, color col, FontData font, float scale = 12) {
		if (string.IsNullOrEmpty(text)) return;
		
		// flip y axis of canvas back, so text will render not upside-down
		_canvas!.Scale(1,-1);
		pos.FlipY();

		_paint!.Color = col.sk();
		_canvas.DrawShapedText(SkiaFontFamilies.Get(font), text, pos.sk(), _paint);
	}
	public override void DrawRect(rect rectangle, color color) {
		_paint!.Color = color.sk();
		_canvas!.DrawRect(rectangle.sk(), _paint);
	}

	public void SetRenderingVariables(IDrawingContextImpl ctx) {
		SKCanvas canvas = ((ISkiaDrawingContextImpl)ctx).SkCanvas;
		
		// skia using flipped y axis, so flip back it
		canvas.Scale(1,-1);
		canvas.Translate(0, -owner.transform.screenBounds.height);

		_canvas?.Dispose();
		_canvas = canvas;
		_paint ??= new();
	}
	
	public SkiaChartsBackend(ChartsCanvas owner, ChartCanvasRenderer renderer) : base(owner, renderer) { }

	public void Dispose() {
		_canvas?.Dispose();
		_paint?.Dispose();
	}
}