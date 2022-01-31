using SomeChartsUi.backends;
using SomeChartsUi.ui.canvas.controls;
using SomeChartsUi.ui.layers;

namespace SomeChartsUi.ui.canvas; 

public class ChartsCanvas {
	public ChartCanvasTransform transform;
	public ChartCanvasRenderer renderer;

	public ChartCanvasControllerBase? controller;
	private List<CanvasLayer> layers => renderer.layers;
	private Dictionary<string, int> layerNames => renderer.layerNames;

	public ChartsCanvas(ChartsBackendBase backend) {
		transform = new();
		renderer = new(this, backend);
		backend.owner = this;
		backend.renderer = renderer;
	}

	public CanvasLayer AddLayer(string name) {
		CanvasLayer l = new(this, name);
		renderer.layerNames.Add(name, layers.Count);
		layers.Add(l);
		return l;
	}

	public void RemoveLayer(string name) {
		if (!layerNames.ContainsKey(name)) return;
		layers.RemoveAt(layerNames[name]);
		layerNames.Remove(name);
	}

	public CanvasLayer? GetLayer(string name) => layerNames.TryGetValue(name, out int i) ? layers[i] : null;
	public CanvasLayer GetLayer(int i) => layers[i];
}