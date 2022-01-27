using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.utils.rects;

public struct rect {
	public float left;
	public float bottom;
	public float width;
	public float height;

	public float right => left + width;
	public float top => bottom + height;

	public float midX => left + width * .5f;
	public float midY => bottom + height * .5f;

	public rect(float left, float bottom, float width, float height) {
		this.left = left;
		this.bottom = bottom;
		this.width = width;
		this.height = height;
	}

	public bool Contains(float2 p) => (p.x >= left) & (p.y >= bottom) & (p.x <= right) & (p.y <= top);
	public bool Contains(float x, float y) => (x >= left) & (y >= bottom) & (x <= right) & (y <= top);

	public bool Contains(float x, float y, float w, float h) => (x + w >= left) & (y + h >= bottom) & (x - w <= right) & (y - h <= top);
	public bool ContainsY(float y, float h) => (y + h >= bottom) & (y - h <= top);

	public bool Contains(rect r) => r.left <= right && r.right >= left && r.bottom <= top && r.top >= bottom;
	public bool Contains(rect r, float a) => r.left <= right + a && r.right + a >= left && r.bottom <= top + a && r.top + a >= bottom;

	public bool Intersects(float2 p0, float2 p1) =>
		Contains(p0) & Contains(p1) ||
		_BottomIntersection(p0, p1) ||
		_TopIntersection(p0, p1) ||
		_LeftIntersection(p0, p1) ||
		_RightIntersection(p0, p1);

	private bool _BottomIntersection(float2 p0, float2 p1) => LineLine(p0, p1, new(left, bottom), new(right, bottom));
	private bool _TopIntersection(float2 p0, float2 p1) => LineLine(p0, p1, new(left, top), new(right, top));
	private bool _LeftIntersection(float2 p0, float2 p1) => LineLine(p0, p1, new(left, bottom), new(left, top));
	private bool _RightIntersection(float2 p0, float2 p1) => LineLine(p0, p1, new(right, bottom), new(right, top));

	private static bool LineLine(float2 a0, float2 a1, float2 b0, float2 b1) {
		float2 b = a1 - a0;
		float2 d = b1 - b0;
		float bDotDPerp = b.x * d.y - b.y * d.x;

		if (bDotDPerp == 0) return false;
		bDotDPerp = 1 / bDotDPerp;

		float2 c = b0 - a0;
		float t = (c.x * d.y - c.y * d.x) * bDotDPerp;
		if ((t < 0) | (t > 1)) return false;

		float u = (c.x * b.y - c.y * b.x) * bDotDPerp;
		return (u > 0) & (u < 1);
	}

	public void Deconstruct(out float x, out float y, out float w, out float h) {
		x = left;
		y = bottom;
		w = width;
		h = height;
	}

	public rect ToWorld(float2 pos, float2 scale) => new(left - pos.x, bottom - pos.y, width / scale.x, height / scale.y);
	public rect ToScreen(float2 pos, float2 scale) => new((left + pos.x) * scale.x, (bottom + pos.y) * scale.y, width * scale.x, height * scale.y);

	// public static implicit operator ChRect(Rect v) {
	// 	return new((float)v.Left, (float)v.Top, (float)v.Width, (float)v.Height);
	// }
	// public static implicit operator SKRect(ChRect v) {
	// 	return new(v.left, v.bottom, v.right, v.top);
	// }

	public override string ToString() => $"({left},{bottom},{width},{height})";
}