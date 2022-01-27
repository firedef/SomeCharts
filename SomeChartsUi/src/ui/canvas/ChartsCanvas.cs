using SomeChartsUi.backends;

namespace SomeChartsUi.ui.canvas; 

public class ChartsCanvas {
	public ChartCanvasTransform transform;
	public ChartCanvasRenderer renderer;

	public ChartsCanvas(ChartsBackendBase backend) {
		transform = new();
		renderer = new(this, backend);
	}
}