using System;
using System.Collections.Generic;
using MathStuff;
using SkiaSharp;
using SkiaSharp.HarfBuzz;
using SomeChartsUi.ui.text;

namespace SomeChartsUiAvalonia.utils;

public static class SkiaFontFamilies {
	public static SKShaper defaultShaper = new(SKTypeface.Default);
	public static Dictionary<string, FontFamilyData?> fonts = new();

	public static FontFamilyData? GetFontFamily(string name) {

		if (fonts.TryGetValue(name, out FontFamilyData? v)) return v;
		TryLoad(name);
		return fonts[name];
	}

	private static void TryLoad(string name) {
		if (SKTypeface.FromFamilyName(name) == null) {
			fonts.Add(name, null);
			return;
		}
		fonts.Add(name, FontFamilyData.Load(name));
	}

	public static SKShaper Get(string fontFamily, SKFontStyleWeight weight, SKFontStyleWidth width, SKFontStyleSlant slant) =>
		GetFontFamily(fontFamily)?.Get(weight, width, slant) ?? defaultShaper;

	public static SKShaper Get(FontData data) => Get(data.family,
	                                                 data.isBold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal,
	                                                 data.isExpanded ? SKFontStyleWidth.Expanded : SKFontStyleWidth.Normal,
	                                                 data.isItalic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright);
}

public class FontFamilyData : IDisposable {
	public List<SKShaper> fonts = new();

	public void Dispose() {
		foreach (SKShaper v in fonts) v.Dispose();
	}

	public static FontFamilyData Load(string name) {
		SKFontStyleSet styles = SKFontManager.Default.GetFontStyles(name);
		FontFamilyData data = new();
		foreach (SKFontStyle style in styles) data.fonts.Add(new(styles.CreateTypeface(style)));

		return data;
	}

	public SKShaper Get(SKFontStyleWeight weight, SKFontStyleWidth width, SKFontStyleSlant slant) {
		int len = fonts.Count;

		int bestValue = int.MaxValue;
		int bestIndex = 0;

		for (int i = 0; i < len; i++) {
			int curValue = math.abs(fonts[i].Typeface.FontWeight - (int)weight) +
			               math.abs(fonts[i].Typeface.FontWidth - (int)width) * 100 +
			               math.abs((int)fonts[i].Typeface.FontSlant - (int)slant) * 100;
			if (curValue > bestValue) continue;
			bestValue = curValue;
			bestIndex = i;
		}

		return fonts[bestIndex];
	}
}