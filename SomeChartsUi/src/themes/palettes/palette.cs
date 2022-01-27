using SomeChartsUi.themes.colors;

namespace SomeChartsUi.themes.palettes; 

public class palette {
	private readonly color[] _colors;
	public readonly int id;

	public palette(color[] colors, int id) {
		_colors = colors;
		this.id = id;
	}

	public color this[int i] => _colors[i % _colors.Length];
}