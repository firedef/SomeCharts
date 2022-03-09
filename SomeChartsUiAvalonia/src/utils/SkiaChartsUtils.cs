using Avalonia;
using Avalonia.Input;
using MathStuff;
using MathStuff.vectors;
using SkiaSharp;
using SomeChartsUi.ui;

namespace SomeChartsUiAvalonia.utils;

public static class SkiaChartsUtils {
	public static SKRect sk(this rect v) => new(v.left, v.bottom, v.right, v.top);                          // skia using inverted y axis, so swap bottom and top
	public static rect ch(this SKRect v) => new(v.Left, v.Bottom, v.Width, v.Height);                       // skia using inverted y axis, so swap bottom and top
	public static rect ch(this Rect v) => new((float)v.Left, (float)v.Top, (float)v.Width, (float)v.Height);// skia using inverted y axis, so swap bottom and top

	public static SKColor sk(this color v) => new(v.raw);
	public static color ch(this SKColor v) => new(v.Red, v.Green, v.Blue, v.Alpha);

	public static SKPoint sk(this float2 v) => new(v.x, v.y);
	public static float2 ch(this SKPoint v) => new(v.X, v.Y);
	public static float2 ch(this Point v) => new((float)v.X, (float)v.Y);
	public static float2 ch(this Vector v) => new((float)v.X, (float)v.Y);

	public static keymods ch(this KeyModifiers v) {
		keymods mods = default;
		if ((v & KeyModifiers.Shift) != 0) mods |= keymods.shift;
		if ((v & KeyModifiers.Control) != 0) mods |= keymods.ctrl;
		if ((v & KeyModifiers.Alt) != 0) mods |= keymods.alt;
		if ((v & KeyModifiers.Meta) != 0) mods |= keymods.super;
		return mods;
	}

	public static PointerButtons GetEnum(this PointerPointProperties v) {
		PointerButtons buttons = default;
		if (v.IsLeftButtonPressed) buttons |= PointerButtons.left;
		if (v.IsRightButtonPressed) buttons |= PointerButtons.right;
		if (v.IsMiddleButtonPressed) buttons |= PointerButtons.middle;
		if (v.IsXButton1Pressed) buttons |= PointerButtons.forward;
		if (v.IsXButton2Pressed) buttons |= PointerButtons.backward;
		return buttons;
	}
}