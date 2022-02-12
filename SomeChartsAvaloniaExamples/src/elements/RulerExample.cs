using SomeChartsUi.data;
using SomeChartsUi.elements;
using SomeChartsUi.elements.other;
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
		});
		
		AvaloniaRunUtils.RunAvalonia();
	}
}