using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.palettes;
using SomeChartsUi.utils;

namespace SomeChartsUi.themes.themes; 

public partial record theme {
	public static theme dark = GenerateDarkTheme(.4f);
	public static theme light = GenerateLightTheme(.95f);
	public static theme globalTheme = dark;
	
	private static theme GenerateDarkTheme(float accentHue, float defaultHue = .6f, int seed = 77) => GenerateDarkTheme(accentHue, defaultHue, GeneratePalettes(0, 1, .2f, .2f, .9f, .9f, seed));
	private static theme GenerateLightTheme(float accentHue, float defaultHue = .1f, int seed = 99) => GenerateLightTheme(accentHue, defaultHue, GeneratePalettes(0, 1, .8f, .8f, .5f, .5f, seed));
	
	private static theme GenerateDarkTheme(float accentHue, float defaultHue, List<palette> palettes) {
		theme t = new();

		color[] defaultColors = GenerateColors(defaultHue, .4f, .15f, defaultHue, .1f, 1f, 12);
		color[] accentColors = GenerateColors(accentHue, .5f, 1f, accentHue, .3f, .2f, 3);
		color[] commonColors = {"#9effad", "#ffe59e", "#ffa39e"};
		
		ApplyColors(t, defaultColors, accentColors, commonColors);
		t.palettes = palettes;

		return t;
	}
	
	private static theme GenerateLightTheme(float accentHue, float defaultHue, List<palette> palettes) {
		theme t = new();

		color[] defaultColors = GenerateColors(defaultHue, .1f, 1f, defaultHue, .4f, .15f, 12);
		color[] accentColors = GenerateColors(accentHue, .3f, .2f, accentHue, .5f, 1f, 3);
		color[] commonColors = {"#296662", "#293166", "#663029"};
		
		ApplyColors(t, defaultColors, accentColors, commonColors);
		t.palettes = palettes;

		return t;
	}

	private static color[] GenerateColors(float h1, float s1, float l1, float h2, float s2, float l2, int count, float a1 = 1, float a2 = 1) {
		color[] colors = new color[count];

		float hAdd = (h2 - h1) / count;
		float sAdd = (s2 - s1) / count;
		float lAdd = (l2 - l1) / count;
		float aAdd = (a2 - a1) / count;
		for (int i = 0; i < count; i++) {
			colors[i] = color.FromHsl((h1, s1, l1), a1);
			h1 += hAdd;
			s1 += sAdd;
			l1 += lAdd;
			a1 += aAdd;
		}
		
		return colors;
	}

	private static void ApplyColors(theme t, color[] defaultColors, color[] accentColors, color[] commonColors) {
		for (ushort i = theme.default0_ind; i <= theme.default11_ind; i++)
			t[i] = defaultColors[i - theme.default0_ind];
		
		for (ushort i = theme.accent0_ind; i <= theme.accent2_ind; i++)
			t[i] = accentColors[i - theme.accent0_ind];

		t.good = commonColors[0];
		t.normal = commonColors[1];
		t.bad = commonColors[2];
	}

	private static List<palette> GeneratePalettes(float h1, float h2, float s1, float s2, float l1, float l2, int seed, int len = 8, int count = 16) {
		List<palette> palettes = new();

		Random rnd = new(seed);

		for (int i = 0; i < count; i++) {
			(float curH1, float curH2) = (math.lerp(h1, h2, (float)rnd.NextDouble()), math.lerp(h1, h2, (float)rnd.NextDouble()));
			(float curS1, float curS2) = (math.lerp(s1, s2, (float)rnd.NextDouble()), math.lerp(s1, s2, (float)rnd.NextDouble()));
			(float curL1, float curL2) = (math.lerp(l1, l2, (float)rnd.NextDouble()), math.lerp(l1, l2, (float)rnd.NextDouble()));
			color[] colors = GenerateColors(curH1, curS1, curL1, curH2, curS2, curL2, len);

			palette p = new(colors, i);
			palettes.Add(p);
		}
		
		return palettes;
	}
	
	
}