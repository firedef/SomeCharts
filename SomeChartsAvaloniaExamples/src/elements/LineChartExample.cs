using System;
using MathStuff.vectors;
using SomeChartsUi.data;
using SomeChartsUi.elements;
using SomeChartsUi.elements.charts.line;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUiAvalonia.controls.opengl;
using SomeChartsUiAvalonia.impl.opengl.shaders;

namespace SomeChartsAvaloniaExamples.elements; 

public static class LineChartExample {
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

		// add multiple line charts
		int lineChartCount = 3;
		indexedColor[] lineColors = {theme.good_ind, theme.normal_ind, theme.bad_ind};
		for (int i = 0; i < lineChartCount; i++)
			AddLineChart(i, lineColors[i % lineColors.Length], canvas);
		

		// add post processing über shader
		UberShaderSettings.current.bloom = true;
		UberShaderSettings.current.bloom_brightness = 4;
		canvas.UpdateUberPostProcessor();
	}

	private static string GetHorizontalRulerLabels(int i) => i.ToString();
	private static string GetVerticalRulerLabels(int i) => (i * 100).ToString();

	private static void AddLineChart(int i, indexedColor color, AvaloniaGlChartsCanvas canvas) {
		// amount of points in current line
		// this value will not affect line length, because it`s using culling (generate mesh and render only visible parts)
		// you can also use collections by 'ArrayChartData<T>()' and 'CollectionChartData<T>()'
		const int lineLength = 81920;
		IChartData<float> data = new FuncChartData<float>(j => LineChartFunc(j, i * 10), lineLength);
		
		// colors of line
		// can be function/collection, like data source
		// indexed color used to handle different colors on different themes
		IChartData<indexedColor> colors = new ConstChartData<indexedColor>(color);

		LineChart chart = canvas.AddLineChart(data, colors);

		// set dynamic to true, so chart will update automatically
		chart.isDynamic = true;

		// line opacity
		// from 0 (invincible) to 1 (opaque)
		// if you don`t want to render lines, disable them by 'drawLines = false'
		chart.lineAlphaMul = 0.2f;

		// you can change 'updateFrameSkip' to specify update frequency
		// bigger value - less frequent updates but better performance
		// value '10' means it will re-generate mesh every 10 frames (6 times per second at 60 fps)
		// minimum is 0
		chart.updateFrameSkip = 10;
	}

	// LineChart data source
	// simple sinusoid
	private static float LineChartFunc(int index, float offset) => MathF.Sin(index * .1f + offset) * 1000;
}