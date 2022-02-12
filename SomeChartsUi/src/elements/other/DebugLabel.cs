using System.Text;
using SomeChartsUi.data;
using SomeChartsUi.ui.elements;

namespace SomeChartsUi.elements.other; 

public class DebugLabel : Label {
	public DebugLabel() : base("") {
		txt = new ChartPropertyFunc<string>(GetString);
	}

	private static string GetString(RenderableBase r) {
		StringBuilder sb = new();

		sb.Append($"render: {r.canvas.renderTime.TotalMilliseconds:00.00} ms\n");
		
		return sb.ToString();
	}
}