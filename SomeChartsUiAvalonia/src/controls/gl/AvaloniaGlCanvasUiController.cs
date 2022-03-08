using System;
using Avalonia.Input;
using SomeChartsUi.ui;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.canvas.controls;
using SomeChartsUiAvalonia.backends;

namespace SomeChartsUiAvalonia.controls.gl; 

public class AvaloniaGlCanvasUiController : CanvasUiControllerBase {
	public AvaloniaGlChartsCanvas avaloniaOwner;

	public AvaloniaGlCanvasUiController(ChartsCanvas owner, AvaloniaGlChartsCanvas avaloniaOwner) : base(owner) => this.avaloniaOwner = avaloniaOwner;

	protected override void Capture() => avaloniaOwner.pointer?.Capture(avaloniaOwner);
	protected override void ReleaseCapture() => avaloniaOwner.pointer?.Capture(null);
	protected override bool IsCaptured() => Equals(avaloniaOwner.pointer?.Captured, avaloniaOwner);
	protected override void SetCursor(string name) => avaloniaOwner.Cursor = Cursor.Parse(name);

	public override void OnKey(keycode key, keymods mods) {
		base.OnKey(key, mods);

		if (key == keycode.y) AvaloniaGlChartsCanvas.polygonMode = (PolygonMode)(((int)AvaloniaGlChartsCanvas.polygonMode + 1) % 3);
		if (key == keycode.u) AvaloniaGlChartsCanvas.useDefaultMat = !AvaloniaGlChartsCanvas.useDefaultMat;
		if (key == keycode.I) AvaloniaGlChartsCanvas.debugTextMat = !AvaloniaGlChartsCanvas.debugTextMat;
		if (key == keycode.p) GlChartsBackend.perspectiveMode = !GlChartsBackend.perspectiveMode;
		if (key == keycode.o) {
			float th = AvaloniaGlChartsCanvas.textThickness;
			th += (mods & keymods.shift) != 0 ? -.01f : .01f;
			if (th > 1) th -= 1;
			if (th < 0) th += 1;
			Console.WriteLine(th);
			AvaloniaGlChartsCanvas.textThickness = th;
		}
	}
}