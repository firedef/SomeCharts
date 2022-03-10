using SomeChartsUi.backends;
using SomeChartsUi.ui.layers;

namespace SomeChartsUi.ui.canvas;

public class ChartCanvasRenderer {
	public readonly ChartsBackendBase backend;
	public readonly Dictionary<string, int> layerNames = new();
	public PostProcessor? postProcessor;

	public readonly List<CanvasLayer> layers = new();
	public ChartsCanvas owner;

	public ChartCanvasRenderer(ChartsCanvas owner, ChartsBackendBase backend) {
		this.owner = owner;
		this.backend = backend;
	}
}