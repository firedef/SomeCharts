using System;
using Avalonia.Platform;
using Avalonia.Skia;
using SkiaSharp;
using SkiaSharp.HarfBuzz;
using SomeChartsUi.backends;
using SomeChartsUi.themes.colors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils;
using SomeChartsUi.utils.rects;
using SomeChartsUi.utils.vectors;
using SomeChartsUiAvalonia.utils;

namespace SomeChartsUiAvalonia.backends; 

public class SkiaChartsBackend : ChartsBackendBase, IDisposable {
	private SKCanvas? _canvas;
	private SKPaint? _paint;
	
	public override unsafe void DrawMesh(float2* points, float2* uvs, color* colors, ushort* indexes, int vertexCount, int indexCount, RenderableTransform transform) {
		_canvas!.Save();
		_canvas.Translate(transform.position.sk());
		_canvas.Scale(transform.scale.sk());
		_canvas.RotateRadians(transform.rotation);
		_canvas.DrawVerticesUnsafe(SKBlendMode.Modulate, (SKPoint*)points, (SKPoint*)uvs, (SKColor*)colors, indexes, vertexCount, indexCount, _paint!);
		_canvas.Restore();
	}
	public override unsafe void DrawMesh(float2[] points, float2[]? uvs, color[]? colors, ushort[] indexes, RenderableTransform transform) {
		fixed(float2* pointsPtr = points)
			fixed(float2* uvsPtr = uvs)
				fixed(color* colorsPtr = colors)
					fixed(ushort* indexesPtr = indexes)
						DrawMesh(pointsPtr, uvsPtr, colorsPtr, indexesPtr, points.Length, indexes.Length, transform);
	}
	public override void DrawText(string text, color col, FontData font, RenderableTransform transform) {
		if (string.IsNullOrEmpty(text)) return;
		
		// flip y axis of canvas back, so text will render not upside-down
		_canvas!.Scale(1,-1);
		float2 pos = transform.position;
		pos.FlipY();

		_paint!.Color = col.sk();
		_paint.TextSize = transform.scale.x;
		_paint.TextScaleX = transform.scale.y / transform.scale.x;
		
		_canvas!.Save();
		_canvas.RotateRadians(transform.rotation);
		_canvas.DrawShapedText(SkiaFontFamilies.Get(font), text, pos.sk(), _paint);
		_canvas.Restore();
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
		
		//TODO: calculate scale after translation
		canvas.Scale(owner.transform.zoom.animatedValue.sk());
		// canvas.Scale(MathF.Sin(owner.transform.rotation.animatedValue.x + MathF.PI / 2), MathF.Sin(owner.transform.rotation.animatedValue.y + MathF.PI / 2));
		canvas.Translate(owner.transform.position.animatedValue.sk());
		canvas.RotateRadians(owner.transform.rotation.animatedValue.z);
		// SKMatrix matrix = SKMatrix.CreateScale()

		_canvas = canvas;
		_paint ??= new();
		
		_paint.SubpixelText = true;
		_paint.Color = SKColors.White;
		_paint.StrokeWidth = 2;
		_paint.IsAntialias = true;
	}

	public void Dispose() {
		_paint?.Dispose();
	}
}