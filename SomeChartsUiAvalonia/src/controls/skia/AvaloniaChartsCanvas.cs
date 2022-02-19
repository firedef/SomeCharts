using System;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using SkiaSharp;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils.vectors;
using SomeChartsUiAvalonia.backends;

namespace SomeChartsUiAvalonia.controls.skia;

public partial class AvaloniaChartsCanvas : Panel {
	/// <summary>time of latest redraw</summary>
	private TimeSpan _prevUpdTime;
	
	/// <summary>redraw loop timer</summary>
	// ReSharper disable once NotAccessedField.Local
	private Timer? _updateTimer;
	private readonly ChartsCanvas canvas = CreateCanvas();
	
	/// <summary>name of current canvas</summary>
	public string canvasName = "???";
	
	/// <summary>pointer (mouse) instance</summary>
	public IPointer? pointer;
	
	/// <summary>pause redraw loop</summary>
	public bool stopRender;
	
	/// <summary>delay (in ms) between canvas redraw (when app is not hovered by mouse)<br/>Avalonia deferred renderer have max framerate of 30</summary>
	public int updateInterval = 200;
	/// <summary>delay (in ms) between canvas redraw (when app is hovered by mouse)<br/>Avalonia deferred renderer have max framerate of 30</summary>
	public int updateInterval_hover = 16;

	/// <summary>time of latest canvas move</summary>
	public TimeSpan panUpdateTime;
	/// <summary>time of latest canvas zoom</summary>
	public TimeSpan zoomUpdateTime;

	public float2 screenSize => new((float)Bounds.Width, (float)Bounds.Height);

	public AvaloniaChartsCanvas() {
		_updateTimer = new(_ => Update(), null, 0, 10);
		canvas.controller = new AvaloniaCanvasUiController(canvas, this);
		Focusable = true;
	}

	private static ChartsCanvas CreateCanvas() {
		ChartsCanvas canvas = new(new SkiaChartsBackend());
		canvas.AddLayer("bg");
		canvas.AddLayer("normal");
		canvas.AddLayer("top");

		return canvas;
	}

	/// <summary>redraw canvas</summary>
	public void Rebuild() => InvalidateVisual();

	/// <summary>redraw loop function</summary>
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
		TimeSpan now = DateTime.Now.TimeOfDay;
		TimeSpan maxDiff = TimeSpan.FromMilliseconds(IsPointerOver ? updateInterval_hover : updateInterval);
		return now - _prevUpdTime >= maxDiff;
	}

	/// <summary>render to image</summary>
	public SKImage RenderImage() => throw new NotImplementedException();

	/// <summary>add element to layer</summary>
	public void AddElement(RenderableBase el, string layer = "normal") => (canvas.GetLayer(layer) ?? canvas.GetLayer(1)).AddElement(el);
	/// <summary>remove element from layer</summary>
	public void RemoveElement(RenderableBase el, string layer = "normal") => (canvas.GetLayer(layer) ?? canvas.GetLayer(1)).RemoveElement(el);
}