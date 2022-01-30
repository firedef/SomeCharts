using SomeChartsUi.backends;

namespace SomeChartsUi.ui.canvas; 

public class ChartCanvasRenderer {
	public ChartsCanvas owner;
	public ChartsBackendBase backend;
	
	

	public ChartCanvasRenderer(ChartsCanvas owner, ChartsBackendBase backend) {
		this.owner = owner;
		this.backend = backend;
	}
}