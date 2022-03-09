using MathStuff;
using SomeChartsUi.themes.palettes;

namespace SomeChartsUi.themes.themes;

public partial record theme {
	public bool isDark;
	public List<palette> palettes;

	public void AddPalette(params color[] colors) => palettes.Add(new(colors, palettes.Count));

	public palette GetPalette(int i) => palettes[i % palettes.Count];
}