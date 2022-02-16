using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.utils.vectors;
using SomeChartsUiAvalonia.utils;

namespace SomeChartsUiAvalonia.controls;

public partial class AvaloniaChartsCanvas {
	protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e) {
		stopRender = true;
		base.OnDetachedFromVisualTree(e);
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

		canvas.controller?.OnKey((keycode)e.Key, mods);
	}
	public override void Render(DrawingContext context) {
		canvas.transform.screenBounds = Bounds.ch();
		canvas.transform.Update();
		canvas.GetLayer("bg")!.background = theme.default0_ind;

		Stopwatch sw = Stopwatch.StartNew();
		context.Custom(new CustomAvaloniaRender(canvas, Bounds));
		canvas.renderTime = sw.Elapsed;
	}
}