using System;
using MathStuff.vectors;
using SomeChartsUi.data;
using SomeChartsUi.elements;
using SomeChartsUi.themes.themes;
using SomeChartsUiAvalonia.controls.opengl;

namespace SomeChartsAvaloniaExamples.elements; 

public static class HeatmapChartExample {
    public static void Run() {
		// call RunAfterStart before running avalonia
		// it will execute after application open
		// RunAvalonia() blocks current thread
		AvaloniaRunUtils.RunAfterStart(AddElements);
		AvaloniaRunUtils.RunAvalonia();
	}

	private static void AddElements() {
		// add Avalonia OpenGl canvas 
		AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();
		const int rulerOffset = 1_000_000; // ruler (grid) length
		
		// add horizontal ruler (grid)
		canvas.AddRuler(Orientation.horizontal, rulerOffset);
			
		// add vertical ruler (grid)
		canvas.AddRuler(Orientation.vertical, rulerOffset);

		const int length = 65536;
		IChart2DData<float> data = new FuncChart2DData<float>(HeatmapFunc, length);
		canvas.AddHeatmapChart(data, theme.globalTheme.goodGradient);

		canvas.UpdateUberPostProcessor();
	}
	
	private static float HeatmapFunc(int2 p) => Circle(p, 0.005f) * Circle(p, 0.004f) * Circle(p, 0.003f) * Circle(p, 0.002f);

	private static float Circle(int2 p, float s) => MathF.Sin(p.x * s) * MathF.Sin(p.y * s);
}