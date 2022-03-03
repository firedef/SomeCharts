using Avalonia.Input;
using SomeChartsUi.ui;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.canvas.controls;
using SomeChartsUiAvalonia.backends;
using SomeChartsUiAvalonia.utils.collections;

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

		if (key == keycode.y) GlMesh.wireframeMode = !GlMesh.wireframeMode;
		if (key == keycode.p) GlChartsBackend.perspectiveMode = !GlChartsBackend.perspectiveMode;
	}
}