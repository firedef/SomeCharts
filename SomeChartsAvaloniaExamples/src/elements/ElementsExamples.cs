using System;
using SomeChartsUi.data;
using SomeChartsUi.elements;
using SomeChartsUi.elements.charts.line;
using SomeChartsUi.elements.other;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils;
using SomeChartsUi.utils.vectors;
using SomeChartsUiAvalonia.controls;
using SomeChartsUiAvalonia.controls.gl;
using SomeChartsUiAvalonia.controls.skia;

namespace SomeChartsAvaloniaExamples.elements; 

public static class ElementsExamples {
	public static void RunTest() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();

			TestRenderable r = new();
			canvas.AddElement(r);
			r.GenerateMesh();
			r.isDynamic = true;
		});
		
		AvaloniaRunUtils.RunAvalonia();
	}
	
	public static void RunRuler() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaChartsCanvas canvas = AvaloniaRunUtils.AddCanvas();
			canvas.AddElement(new Ruler {
				orientation = Orientation.vertical, 
				names = new FuncChartManagedData<string>(i => i.ToString(), -1), 
				stickRange = new(0, 0, 10_000, 0),
				length = 1_000,
				lineLength = 10_000,
			});
			canvas.AddElement(new Ruler {
				orientation = Orientation.horizontal, 
				names = new FuncChartManagedData<string>(i => i.ToString(), -1), 
				stickRange = new(0, 0, 0, 10_000),
				length = 1_000,
				lineLength = 10_000,
			});

			Func<RenderableBase, RenderableTransform> trFunc = _ => {
				float2 screenSize = canvas.screenSize;
				return new(screenSize- new float2(200,100), 1, float3.zero, TransformType.screenSpace);
			};
			canvas.AddElement(new DebugLabel() {textScale = 16, transform = trFunc}, "top");
		});
		
		AvaloniaRunUtils.RunAvalonia();
	}
	
	public static void RunLineChart() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaChartsCanvas canvas = AvaloniaRunUtils.AddCanvas();
			const int rulerOffset = 1_000_000;

			canvas.AddElement(new Ruler {
				drawLabels = true,
				orientation = Orientation.horizontal,
				length = rulerOffset,
				names = new FuncChartManagedData<string>(i => i.ToString(), 1),
				stickRange = new(0, 0, 0, rulerOffset),
			});
			
			canvas.AddElement(new Ruler {
				drawLabels = true,
				orientation = Orientation.vertical,
				length = rulerOffset,
				names = new FuncChartManagedData<string>(i => (i * 100).ToString(), 1),
				stickRange = new(0, 0, rulerOffset, 0),
			});
			
			IChartData<float> data = new FuncChartData<float>(i => MathF.Sin(i * .1f) * 1000, 2048);
			IChartData<indexedColor> colors = new ConstChartData<indexedColor>(new(theme.good_ind));
			canvas.AddElement(new LineChart(data, colors));
		});
		
		AvaloniaRunUtils.RunAvalonia();
	}

	public static void RunGl() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();
		});
		
		AvaloniaRunUtils.RunAvalonia();
	}
}