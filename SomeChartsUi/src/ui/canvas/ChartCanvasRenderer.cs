using SomeChartsUi.backends;
using SomeChartsUi.ui.layers;

namespace SomeChartsUi.ui.canvas; 

public class ChartCanvasRenderer {
	public ChartsCanvas owner;
	public readonly ChartsBackendBase backend;
	
	public readonly List<CanvasLayer> layers = new();
	public readonly Dictionary<string, int> layerNames = new();

	public ChartCanvasRenderer(ChartsCanvas owner, ChartsBackendBase backend) {
		this.owner = owner;
		this.backend = backend;
	}
}