using System;
using Avalonia;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.layers;
using SomeChartsUiAvalonia.backends;

namespace SomeChartsUiAvalonia.controls.skia;

public class CustomAvaloniaRender : ICustomDrawOperation {
	private readonly ChartsCanvas _owner;
	public CustomAvaloniaRender(ChartsCanvas owner, Rect bounds) {
		_owner = owner;
		Bounds = bounds;
	}
	public Rect Bounds { get; }
	public void Dispose() { }
	public bool HitTest(Point p) => Bounds.Contains(p);
	public bool Equals(ICustomDrawOperation? other) => false;

	public void Render(IDrawingContextImpl context) {
		throw new NotImplementedException();
		// ((SkiaChartsBackend)_owner.renderer.backend).SetRenderingVariables(context);
		// foreach (CanvasLayer layer in _owner.renderer!.layers)
		// 	layer.Render();
	}
}