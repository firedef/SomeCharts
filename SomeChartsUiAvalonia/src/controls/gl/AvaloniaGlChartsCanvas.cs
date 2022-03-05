using System.Diagnostics;
using Avalonia;
using Avalonia.Input;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Threading;
using MathStuff.vectors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.layers;
using SomeChartsUiAvalonia.backends;
using SomeChartsUiAvalonia.utils;
using static Avalonia.OpenGL.GlConsts;

namespace SomeChartsUiAvalonia.controls.gl;

public enum PolygonMode {
	fill,
	line,
	points
}

public class AvaloniaGlChartsCanvas : OpenGlControlBase {

	public static PolygonMode polygonMode = PolygonMode.fill;
	public readonly ChartsCanvas canvas = CreateCanvas();

	/// <summary>name of current canvas</summary>
	public string canvasName = "???";

	/// <summary>pointer (mouse) instance</summary>
	public IPointer? pointer;

	/// <summary>pause redraw loop</summary>
	public bool stopRender;

	public AvaloniaGlChartsCanvas() {
		//_updateTimer = new(_ => Update(), null, 0, 10);
		canvas.controller = new AvaloniaGlCanvasUiController(canvas, this);
		Focusable = true;
		GlInfo.version = GlVersion;
	}

	private static ChartsCanvas CreateCanvas() {
		ChartsCanvas canvas = new(new GlChartsBackend());
		canvas.AddLayer("bg");
		canvas.AddLayer("normal");
		canvas.AddLayer("top");

		return canvas;
	}

	/// <summary>add element to layer</summary>
	public void AddElement(RenderableBase el, string layer = "normal") => (canvas.GetLayer(layer) ?? canvas.GetLayer(1)).AddElement(el);
	/// <summary>remove element from layer</summary>
	public void RemoveElement(RenderableBase el, string layer = "normal") => (canvas.GetLayer(layer) ?? canvas.GetLayer(1)).RemoveElement(el);


	protected override void OnOpenGlInit(GlInterface glInterface, int fb) {
		GlInfo.glExt = new(glInterface);
		GlInfo.gl = glInterface;
		GlInfo.version = GlVersion;
	}

	protected override void OnOpenGlDeinit(GlInterface gl, int fb) {
		// Unbind everything
		gl.BindBuffer(GL_ARRAY_BUFFER, 0);
		gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
		GlInfo.glExt!.BindVertexArray(0);
		gl.UseProgram(0);
	}

	protected override void OnOpenGlRender(GlInterface gl, int fb) {
		gl.Enable(GL_DEPTH_TEST);
		gl.Enable(GL_MULTISAMPLE);
		switch (polygonMode) {
			case PolygonMode.fill:
				GlInfo.glExt!.PolygonMode(GL_FRONT_AND_BACK, GL_FILL);
				break;
			case PolygonMode.line:
				GlInfo.glExt!.PolygonMode(GL_FRONT_AND_BACK, GL_LINE);
				break;
			case PolygonMode.points:
				GlInfo.glExt!.PolygonMode(GL_FRONT_AND_BACK, GL_POINT);
				break;
		}
		//gl.Enable(GL_CULL_FACE);
		//_glExtras.CullFace(GL_FRONT);

		gl.Viewport(0, 0, (int)Bounds.Width, (int)Bounds.Height);

		canvas.transform.screenBounds = Bounds.ch();
		canvas.transform.Update();
		canvas.GetLayer("bg")!.background = theme.default0_ind;

		GlInfo.CheckError("before render");
		Stopwatch sw = Stopwatch.StartNew();
		foreach (CanvasLayer layer in canvas.renderer!.layers)
			layer.Render();
		GlInfo.CheckError("end");

		Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Render);
		canvas.renderTime = sw.Elapsed;
	}


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
}