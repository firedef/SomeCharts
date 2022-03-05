using System;
using Avalonia.Platform;
using Avalonia.Skia;
using MathStuff;
using MathStuff.vectors;
using SkiaSharp;
using SkiaSharp.HarfBuzz;
using SomeChartsUi.backends;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;
using SomeChartsUiAvalonia.utils;

namespace SomeChartsUiAvalonia.backends;

public class SkiaChartsBackend : ChartsBackendBase, IDisposable {
	private SKCanvas? _canvas;
	private SKPaint? _paint;

	public void Dispose() {
		_paint?.Dispose();
	}

	//TODO: add 3D rotation
	private void ApplyTransform(RenderableTransform transform, bool flipY = false) {
		float2 scale = transform.scale;
		float2 position = transform.position;
		float3 rotation = transform.rotation;

		// apply canvas transform if world-space 
		if (transform.type == TransformType.worldSpace) {
			_canvas!.RotateRadians(owner.transform.rotation.animatedValue.z);
			float2 s = owner.transform.zoom.animatedValue;
			_canvas.Scale(s.sk());
			float2 p = owner.transform.position.animatedValue;
			if (flipY) p.FlipY();
			_canvas.Translate(p.sk());
		}

		// normalize coordinates if viewport-space
		if (transform.type == TransformType.viewportSpace) {
			position /= scale;
			scale *= owner.transform.screenBounds.widthHeight;
		}

		if (transform.type == TransformType.screenSpace) position /= scale;

		if (flipY) position.FlipY();

		// apply transform
		_canvas!.Scale(scale.sk());
		_canvas.Translate(position.sk());
		_canvas.RotateRadians(rotation.z);
	}

	public override void DrawText(string text, color col, FontData font, RenderableTransform transform) {
		if (string.IsNullOrEmpty(text)) return;

		// flip y axis of canvas back, so text will render not upside-down
		_canvas!.Save();
		_canvas!.Scale(1, -1);
		ApplyTransform(transform, true);

		_paint!.Color = col.sk();
		_paint.TextSize = 1;
		// _paint.TextAlign = SKTextAlign.Left;

		_canvas.RotateRadians(transform.rotation.z);
		_canvas.DrawShapedText(SkiaFontFamilies.Get(font), text, SKPoint.Empty, _paint);
		_canvas.Restore();
	}
	public override void ClearScreen(color col) {
		throw new NotImplementedException();
	}
	public override void DrawMesh(Mesh mesh, Material? material, RenderableTransform transform) {
		throw new NotImplementedException();
	}
	public override Texture CreateTexture(string path) => throw new NotImplementedException();

	public void SetRenderingVariables(IDrawingContextImpl ctx) {
		SKCanvas canvas = ((ISkiaDrawingContextImpl)ctx).SkCanvas;

		// skia using flipped y axis, so flip back it
		canvas.Scale(1, -1);
		canvas.Translate(0, -owner.transform.screenBounds.height);

		_canvas = canvas;
		_paint ??= new();

		_paint.SubpixelText = true;
		_paint.Color = SKColors.White;
		_paint.StrokeWidth = 2;
		_paint.IsAntialias = true;
	}
}