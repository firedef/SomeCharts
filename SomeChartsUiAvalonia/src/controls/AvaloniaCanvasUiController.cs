using Avalonia.Input;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.canvas.controls;
using SomeChartsUiAvalonia.controls.skia;

namespace SomeChartsUiAvalonia.controls;

public class AvaloniaCanvasUiController : CanvasUiControllerBase {
	public AvaloniaChartsCanvas avaloniaOwner;

	public AvaloniaCanvasUiController(ChartsCanvas owner, AvaloniaChartsCanvas avaloniaOwner) : base(owner) => this.avaloniaOwner = avaloniaOwner;

	protected override void Capture() => avaloniaOwner.pointer?.Capture(avaloniaOwner);
	protected override void ReleaseCapture() => avaloniaOwner.pointer?.Capture(null);
	protected override bool IsCaptured() => Equals(avaloniaOwner.pointer?.Captured, avaloniaOwner);
	protected override void SetCursor(string name) => avaloniaOwner.Cursor = Cursor.Parse(name);
}