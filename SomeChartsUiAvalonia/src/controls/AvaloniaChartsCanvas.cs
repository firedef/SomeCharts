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
using SomeChartsUi.elements.other;
using SomeChartsUi.themes.colors;
using SomeChartsUi.ui;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.canvas.controls;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.layers;
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
	
	private static ChartsCanvas CreateCanvas() {
		ChartsCanvas canvas = new(new SkiaChartsBackend());
		canvas.AddLayer("bg");
		canvas.AddLayer("normal");
		canvas.AddLayer("top");

		return canvas;
	}

	public AvaloniaChartsCanvas() {
		_updateTimer = new(_ => Update(), null, 0, 10);
		canvas.controller = new CanvasUiController(canvas);
		canvas.GetLayer("bg")!.background = color.purple;
		AddElement(new TestRenderable());
	}

	protected override void OnPointerPressed(PointerPressedEventArgs e) {
		PointerPoint currentPoint = e.GetCurrentPoint(this);

		PointerButtons buttons = default;
		if (currentPoint.Properties.IsLeftButtonPressed) buttons |= PointerButtons.left;
		if (currentPoint.Properties.IsRightButtonPressed) buttons |= PointerButtons.right;
		if (currentPoint.Properties.IsMiddleButtonPressed) buttons |= PointerButtons.middle;
		if (currentPoint.Properties.IsXButton1Pressed) buttons |= PointerButtons.forward;
		if (currentPoint.Properties.IsXButton2Pressed) buttons |= PointerButtons.backward;

		keymods mods = default;
		if ((e.KeyModifiers & KeyModifiers.Shift) != 0) mods |= keymods.shift;
		if ((e.KeyModifiers & KeyModifiers.Control) != 0) mods |= keymods.ctrl;
		if ((e.KeyModifiers & KeyModifiers.Alt) != 0) mods |= keymods.alt;
		if ((e.KeyModifiers & KeyModifiers.Meta) != 0) mods |= keymods.super;

		MouseState s = new(currentPoint.Position.ch(), float2.zero, buttons, mods, currentPoint.Pointer.Captured);

		canvas.controller?.OnMouseDown(s);
	}
	protected override void OnPointerMoved(PointerEventArgs e) {
		PointerPoint currentPoint = e.GetCurrentPoint(this);

		PointerButtons buttons = default;
		if (currentPoint.Properties.IsLeftButtonPressed) buttons |= PointerButtons.left;
		if (currentPoint.Properties.IsRightButtonPressed) buttons |= PointerButtons.right;
		if (currentPoint.Properties.IsMiddleButtonPressed) buttons |= PointerButtons.middle;
		if (currentPoint.Properties.IsXButton1Pressed) buttons |= PointerButtons.forward;
		if (currentPoint.Properties.IsXButton2Pressed) buttons |= PointerButtons.backward;

		keymods mods = default;
		if ((e.KeyModifiers & KeyModifiers.Shift) != 0) mods |= keymods.shift;
		if ((e.KeyModifiers & KeyModifiers.Control) != 0) mods |= keymods.ctrl;
		if ((e.KeyModifiers & KeyModifiers.Alt) != 0) mods |= keymods.alt;
		if ((e.KeyModifiers & KeyModifiers.Meta) != 0) mods |= keymods.super;

		MouseState s = new(currentPoint.Position.ch(), float2.zero, buttons, mods, currentPoint.Pointer.Captured);

		canvas.controller?.OnMouseMove(s);
	}
	protected override void OnPointerReleased(PointerReleasedEventArgs e) {
		PointerPoint currentPoint = e.GetCurrentPoint(this);

		PointerButtons buttons = default;
		if (currentPoint.Properties.IsLeftButtonPressed) buttons |= PointerButtons.left;
		if (currentPoint.Properties.IsRightButtonPressed) buttons |= PointerButtons.right;
		if (currentPoint.Properties.IsMiddleButtonPressed) buttons |= PointerButtons.middle;
		if (currentPoint.Properties.IsXButton1Pressed) buttons |= PointerButtons.forward;
		if (currentPoint.Properties.IsXButton2Pressed) buttons |= PointerButtons.backward;

		keymods mods = default;
		if ((e.KeyModifiers & KeyModifiers.Shift) != 0) mods |= keymods.shift;
		if ((e.KeyModifiers & KeyModifiers.Control) != 0) mods |= keymods.ctrl;
		if ((e.KeyModifiers & KeyModifiers.Alt) != 0) mods |= keymods.alt;
		if ((e.KeyModifiers & KeyModifiers.Meta) != 0) mods |= keymods.super;

		MouseState s = new(currentPoint.Position.ch(), float2.zero, buttons, mods, currentPoint.Pointer.Captured);

		canvas.controller?.OnMouseUp(s);
	}
	protected override void OnPointerWheelChanged(PointerWheelEventArgs e) {
		PointerPoint currentPoint = e.GetCurrentPoint(this);

		PointerButtons buttons = default;
		if (currentPoint.Properties.IsLeftButtonPressed) buttons |= PointerButtons.left;
		if (currentPoint.Properties.IsRightButtonPressed) buttons |= PointerButtons.right;
		if (currentPoint.Properties.IsMiddleButtonPressed) buttons |= PointerButtons.middle;
		if (currentPoint.Properties.IsXButton1Pressed) buttons |= PointerButtons.forward;
		if (currentPoint.Properties.IsXButton2Pressed) buttons |= PointerButtons.backward;

		keymods mods = default;
		if ((e.KeyModifiers & KeyModifiers.Shift) != 0) mods |= keymods.shift;
		if ((e.KeyModifiers & KeyModifiers.Control) != 0) mods |= keymods.ctrl;
		if ((e.KeyModifiers & KeyModifiers.Alt) != 0) mods |= keymods.alt;
		if ((e.KeyModifiers & KeyModifiers.Meta) != 0) mods |= keymods.super;

		MouseState s = new(currentPoint.Position.ch(), e.Delta.ch(), buttons, mods, currentPoint.Pointer.Captured);

		canvas.controller?.OnMouseScroll(s);
	}

	protected override void OnKeyUp(KeyEventArgs e) {
		// if (e.Key == Key.R) {
		// 	if ((e.KeyModifiers & KeyModifiers.Shift) != 0) ResetTransform();
		// 	else ResetXYZoom();
		// }
		//
		// if (e.Key == Key.T) ToggleTheme();
		//
		// if (e.Key == Key.P) {
		// 	UpdateTransformAnimation();
		// 	using var img = RenderImage();
		// 	using var data = img.Encode(SKEncodedImageFormat.Png, 100);
		//
		// 	using FileStream fs = new(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/SomeChartsSnapshot_{new Random().Next(100_000, 999_999):X}.png", FileMode.Create);
		// 	data.SaveTo(fs);
		// 	Console.WriteLine("aaaa");
		// }
		//
		// if (e.Key == Key.W) positionWithoutAnim.Y -= 100 / scale.Y;
		// if (e.Key == Key.S) positionWithoutAnim.Y += 100 / scale.Y;
		// if (e.Key == Key.A) positionWithoutAnim.X += 100 / scale.X;
		// if (e.Key == Key.D) positionWithoutAnim.X -= 100 / scale.X;
	}
	
	public void Rebuild() {
		canvas.transform.screenBounds = Bounds.ch();
		canvas.transform.Update();
		InvalidateVisual();
	}

	private void Update() {
		try {
			if (stopRender || !CheckUpdateDelay()) return;
			//UpdateAnim();
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
		//renderer!.bounds = Bounds;
		//renderer.position = position;
		//renderer.scale = scale;
		//renderer.theme = theme;
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