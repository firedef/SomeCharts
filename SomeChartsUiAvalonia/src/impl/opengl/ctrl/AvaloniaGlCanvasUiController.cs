using System;
using Avalonia.Input;
using SomeChartsUi.ui;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.canvas.controls;
using SomeChartsUiAvalonia.controls.opengl;
using SomeChartsUiAvalonia.impl.opengl.backend;

namespace SomeChartsUiAvalonia.impl.opengl.ctrl;

public class AvaloniaGlCanvasUiController : CanvasUiControllerBase {
	public AvaloniaGlChartsCanvas avaloniaOwner;

	public AvaloniaGlCanvasUiController(ChartsCanvas owner, AvaloniaGlChartsCanvas avaloniaOwner) : base(owner) => this.avaloniaOwner = avaloniaOwner;

	protected override void Capture() => avaloniaOwner.pointer?.Capture(avaloniaOwner);
	protected override void ReleaseCapture() => avaloniaOwner.pointer?.Capture(null);
	protected override bool IsCaptured() => Equals(avaloniaOwner.pointer?.Captured, avaloniaOwner);
	protected override void SetCursor(string name) => avaloniaOwner.Cursor = Cursor.Parse(name);

	public override void OnKey(keycode key, keymods mods) {
		base.OnKey(key, mods);

		if (key == keycode.y) {
			PolygonMode mode = (PolygonMode)(((int)ChartsRenderSettings.polygonMode + 1) % 3);
			Console.WriteLine($"switch polygon mode to {mode}");
			ChartsRenderSettings.polygonMode = mode;
		}
		if (key == keycode.u) {
			bool v = !ChartsRenderSettings.useDefaultMat;
			Console.WriteLine($"use default material is set to {v}");
			ChartsRenderSettings.useDefaultMat = v;
		}
		if (key == keycode.I) {
			bool v = !ChartsRenderSettings.debugTextMat;
			Console.WriteLine($"use debug material is set to {v}");
			ChartsRenderSettings.debugTextMat = v;
		}
		if (key == keycode.p) {
			bool v = !GlChartsBackend.perspectiveMode;
			Console.WriteLine($"use perspective is set to {v}");
			GlChartsBackend.perspectiveMode = v;
		}
		if (key == keycode.l) {
			ChartsRenderSettings.textQuality = (ChartsRenderSettings.textQuality + 1) % 2;
			Console.WriteLine($"changed text quality: {ChartsRenderSettings.textQuality}");
		}
		if (key == keycode.k) {
			bool v = !ChartsRenderSettings.postProcessing;
			Console.WriteLine($"use post-processing is set to {v}");
			ChartsRenderSettings.postProcessing = v;
		}
		if (key == keycode.o) {
			float th = ChartsRenderSettings.textThickness;
			th += (mods & keymods.shift) != 0 ? -.01f : .01f;
			if (th > 1) th -= 1;
			if (th < 0) th += 1;
			Console.WriteLine($"changed font thickness: {th}");
			ChartsRenderSettings.textThickness = th;
		}
	}
}