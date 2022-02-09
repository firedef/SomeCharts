using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;
using SomeChartsUi.elements;
using SomeChartsUi.elements.other;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.canvas.controls;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.layers;
using SomeChartsUi.utils;
using SomeChartsUi.utils.vectors;
using SomeChartsUiAvalonia.backends;
using SomeChartsUiAvalonia.utils;

namespace SomeChartsUiAvalonia.controls;

public class AvaloniaChartsCanvas : Panel {
	public ChartsCanvas canvas = CreateCanvas();
	
	public TimeSpan zoomUpdateTime;
	public TimeSpan panUpdateTime;
	public int updateInterval_hover = 16;
	public int updateInterval = 200;
	public string chartName = "???";
	public bool stopRender;
	private Timer? _updateTimer;
	private TimeSpan _prevUpdTime;

	public IPointer? pointer;
	
	private static ChartsCanvas CreateCanvas() {
		ChartsCanvas canvas = new(new SkiaChartsBackend());
		canvas.AddLayer("bg");
		canvas.AddLayer("normal");
		canvas.AddLayer("top");

		return canvas;
	}

	public AvaloniaChartsCanvas() {
		_updateTimer = new(_ => Update(), null, 0, 10);
		canvas.controller = new AvaloniaCanvasUiController(canvas, this);
		Focusable = true;
		//canvas.GetLayer("bg")!.background = color.purple;
		
		AddElement(new TestRenderable());
		AddElement(new TestRenderable() {transform = new(new(1000,0))});
		AddElement(new TestRenderable() {transform = new(new(0,-2000), 1, new float3(0,0,MathF.PI / 4))});
		
		AddElement(new Ruler() {orientation = Orientation.vertical});
		AddElement(new Ruler() {orientation = Orientation.horizontal, transform = new(new(-1000,0))});
	}

	protected override void OnPointerPressed(PointerPressedEventArgs e) {
		PointerPoint currentPoint = e.GetCurrentPoint(this);
		PointerButtons buttons = currentPoint.Properties.GetEnum();
		keymods mods = e.KeyModifiers.ch();

		MouseState s = new(currentPoint.Position.ch(), float2.zero, buttons, mods);

		canvas.controller?.OnMouseDown(s);
	}
	protected override void OnPointerMoved(PointerEventArgs e) {
		PointerPoint currentPoint = e.GetCurrentPoint(this);
		pointer = e.Pointer;
		PointerButtons buttons = currentPoint.Properties.GetEnum();
		keymods mods = e.KeyModifiers.ch();

		MouseState s = new(currentPoint.Position.ch(), float2.zero, buttons, mods);

		canvas.controller?.OnMouseMove(s);
	}
	protected override void OnPointerReleased(PointerReleasedEventArgs e) {
		PointerPoint currentPoint = e.GetCurrentPoint(this);
		PointerButtons buttons = currentPoint.Properties.GetEnum();
		keymods mods = e.KeyModifiers.ch();

		MouseState s = new(currentPoint.Position.ch(), float2.zero, buttons, mods);

		canvas.controller?.OnMouseUp(s);
	}
	protected override void OnPointerWheelChanged(PointerWheelEventArgs e) {
		PointerPoint currentPoint = e.GetCurrentPoint(this);
		PointerButtons buttons = currentPoint.Properties.GetEnum();
		keymods mods = e.KeyModifiers.ch();
		
		MouseState s = new(currentPoint.Position.ch(), e.Delta.ch(), buttons, mods);
		
		canvas.controller?.OnMouseScroll(s);
	}
	

	protected override void OnKeyUp(KeyEventArgs e) {
		keymods mods = e.KeyModifiers.ch();
		
		canvas.controller?.OnKey((keycode) e.Key, mods);
	}
	
	public void Rebuild() => InvalidateVisual();

	private void Update() {
		try {
			if (stopRender || !CheckUpdateDelay()) return;
			Rebuild();
			_prevUpdTime = DateTime.Now.TimeOfDay;
		}
		catch (Exception e) {
			// ignored
		}
	}

	private bool CheckUpdateDelay() {
		if (Environment.HasShutdownStarted) {
			_updateTimer = null;
			stopRender = true;
			return false;
		}

		var now = DateTime.Now.TimeOfDay;
		var maxDiff = TimeSpan.FromMilliseconds(IsPointerOver ? updateInterval_hover : updateInterval);
		return now - _prevUpdTime >= maxDiff;
	}

	public override void Render(DrawingContext context) {
		canvas.transform.screenBounds = Bounds.ch();
		canvas.transform.Update();
		canvas.GetLayer("bg")!.background = theme.default0_ind;
		context.Custom(new CustomRender(canvas, Bounds));
	}

	public SKImage RenderImage() => throw new NotImplementedException();
	// SKImageInfo info = new((int)Bounds.Width, (int)Bounds.Height);
	//
	// using var surf = SKSurface.Create(info);
	//
	// var canvas = surf.Canvas;
	// canvas.Scale(1, -1);
	// canvas.Translate(0, (float)-Bounds.Height);
	// renderer!.Render();
	// canvas.DrawBitmap(renderer.snapshot, renderer.bounds);
	// return surf.Snapshot();
	public void AddElement(RenderableBase el, string layer = "normal") {
		(canvas.GetLayer(layer) ?? canvas.GetLayer(1)).AddElement(el);
	}

	public void RemoveElement(RenderableBase el, string layer = "normal") {
		(canvas.GetLayer(layer) ?? canvas.GetLayer(1)).RemoveElement(el);
	}

	private class CustomRender : ICustomDrawOperation {
		private readonly ChartsCanvas _owner;
		public CustomRender(ChartsCanvas owner, Rect bounds) {
			_owner = owner;
			Bounds = bounds;
		}
		public Rect Bounds { get; }
		public void Dispose() { }
		public bool HitTest(Point p) => Bounds.Contains(p);
		public bool Equals(ICustomDrawOperation? other) => false;

		public void Render(IDrawingContextImpl context) {
			((SkiaChartsBackend)_owner.renderer.backend).SetRenderingVariables(context);
			//((ISkiaDrawingContextImpl) context).SkCanvas.Clear();
			foreach (CanvasLayer layer in _owner.renderer!.layers)
				layer.Render();
		}
	}
}