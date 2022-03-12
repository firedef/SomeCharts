using MathStuff.vectors;

namespace SomeChartsUi.ui.text;

public readonly record struct FontCharData(float advance, float baseline, float2 position, float2 size, uint glyph) {
	/// <summary>x-shift of next character</summary>
	public readonly float advance = advance;

	/// <summary>y-shift of current character <br/><br/>positive, if character goes bellow baseline (like j, p, g...)</summary>
	public readonly float baseline = baseline;

	public readonly uint glyph = glyph;

	/// <summary>position in atlas (x, y)</summary>
	public readonly float2 position = position;

	/// <summary>size in atlas (width, height)</summary>
	public readonly float2 size = size;

	/// <summary>distance between yMax and baseline</summary>
	public float ascent => size.y - baseline;
}