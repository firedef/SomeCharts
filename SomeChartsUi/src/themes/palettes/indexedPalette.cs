using SomeChartsUi.themes.themes;

namespace SomeChartsUi.themes.palettes; 

public struct indexedPalette {
	public ushort paletteIndex;

	public indexedPalette(ushort paletteIndex) => this.paletteIndex = paletteIndex;

	public palette palette => theme.globalTheme.GetPalette(paletteIndex);

	public static indexedPalette Random() => new((ushort)new Random().Next(1024));
}