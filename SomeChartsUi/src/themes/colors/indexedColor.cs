using SomeChartsUi.themes.palettes;
using SomeChartsUi.themes.themes;

namespace SomeChartsUi.themes.colors; 

public readonly struct indexedColor {
	public readonly color customColor;
	public readonly ushort colorIndex;
	public readonly byte colorMask;

	public indexedColor(color customColor, ushort colorIndex, byte colorMask) {
		this.customColor = customColor;
		this.colorIndex = colorIndex;
		this.colorMask = colorMask;
	}

	public indexedColor(color col) : this(col, 0, 0b1111) { }
	public indexedColor(ushort index) : this(color.black, index, 0) { }
	public indexedColor(ushort index, byte alpha) : this(color.black.WithAlpha(alpha), index, 0b1000) { }
	public indexedColor(palette _, ushort index) : this(color.black, index, 0b1_0000) { }
	public indexedColor(palette _, ushort index, byte alpha) : this(color.black.WithAlpha(alpha), index, 0b1_1000) { }

	public color GetColor() {
		uint mask = 0;
		if ((colorMask & 0b1) != 0) mask = 0xFF;
		if ((colorMask & 0b10) != 0) mask |= 0xFF00;
		if ((colorMask & 0b100) != 0) mask |= 0xFF0000;
		if ((colorMask & 0b1000) != 0) mask |= 0xFF000000;
			
		return new((customColor.raw & mask) |
		           (theme.globalTheme[colorIndex].raw & ~mask));
	}
	
	public color GetColor(palette p) {
		uint mask = 0;
		if ((colorMask & 0b1) != 0) mask = 0xFF;
		if ((colorMask & 0b10) != 0) mask |= 0xFF00;
		if ((colorMask & 0b100) != 0) mask |= 0xFF0000;
		if ((colorMask & 0b1000) != 0) mask |= 0xFF000000;

		uint mask2 = (mask & 0b1_000) == 0 ? 0 : uint.MaxValue;

		return new(
			(customColor.raw & mask) | 
			(theme.globalTheme[colorIndex].raw & ~mask & ~mask2) |
			(p[colorIndex].raw & mask2)
			);
	}
}