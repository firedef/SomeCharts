using System;
using SomeChartsUi.data;
using SomeChartsUi.elements;
using SomeChartsUi.elements.other;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils.vectors;
using SomeChartsUiAvalonia.controls;

namespace SomeChartsAvaloniaExamples.elements; 

public class RulerExample {
	public static void Run() {
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
}