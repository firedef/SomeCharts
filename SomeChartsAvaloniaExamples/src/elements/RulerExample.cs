using SomeChartsUi.elements;
using SomeChartsUi.elements.other;
using SomeChartsUiAvalonia.controls;

namespace SomeChartsAvaloniaExamples.elements; 

public class RulerExample {
	public static void Run() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaChartsCanvas canvas = AvaloniaRunUtils.AddCanvas();
			canvas.AddElement(new Ruler {orientation = Orientation.vertical});
		});
		
		AvaloniaRunUtils.RunAvalonia();
	}
}