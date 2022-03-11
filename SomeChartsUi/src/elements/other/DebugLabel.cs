using System.Text;
using SomeChartsUi.data;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;

namespace SomeChartsUi.elements.other;

public class DebugLabel : Label {
	public DebugLabel(ChartsCanvas c) : base("", c) => beforeRender += () => txt = GetString(this);

	private static string GetString(RenderableBase r) {
		StringBuilder sb = new();

		sb.Append($"render: {r.canvas.renderTime.TotalMilliseconds:00.00} ms\n");

		return sb.ToString();
	}
}