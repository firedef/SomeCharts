using MathStuff;
using MathStuff.vectors;

namespace SomeChartsUi.utils; 

public static class Geometry {
	//public static bool Intersects(rect a, rect b, float2 offset) {
	//	if (math.abs(a.midX - b.midX - offset.x) > a.width * .5f + b.width * .5f) return false;
	//	if (math.abs(a.midY - b.midY - offset.y) > a.height * .5f + b.height * .5f) return false;
	//	return true;
	//}
	
	//public static bool Intersects(rect a, rect b, float2 offset) {
	//	if (a.left < b.left + b.right &&
	//	    a.left + a.right > b.left &&
	//	    a.bottom < b.bottom + b.top &&
	//	    a.top + a.bottom > b.bottom) return true;
	//	return false;
	//}
	public static bool Intersects(rect a, rect b, float2 offset) => !(math.abs(a.left - b.left - offset.x) > a.width + b.width) && !(math.abs(a.bottom - b.bottom - offset.y) > a.height + b.height);

	// public static bool Intersects(rect a, rect b, float aR, float bR) => Intersects(RotateRect(a, aR), RotateRect(b, bR));
	//
	// public static bool Intersects(float2[] a, float2[] b) {
	// 	int c = a.Length;
	// 	for (int i = 0; i < c; i++) {
	// 		float2 normal = Normal(a[i], a[(i + 1) % c]);
	// 		float2 minmaxA = SatTest(normal, a);
	// 		float2 minmaxB = SatTest(normal, b);
	// 		if (!Overlaps(minmaxA, minmaxB)) return false;
	// 	}
	// 	
	// 	c = b.Length;
	// 	for (int i = 0; i < c; i++) {
	// 		float2 normal = Normal(b[i], b[(i + 1) % c]);
	// 		float2 minmaxA = SatTest(normal, a);
	// 		float2 minmaxB = SatTest(normal, b);
	// 		if (!Overlaps(minmaxA, minmaxB)) return false;
	// 	}
	//
	// 	return true;
	// }
	//
	// public static float2 SatTest(float2 axis, float2[] corners) {
	// 	float2 minmax = new(float.MaxValue, -float.MaxValue);
	//
	// 	int c = corners.Length;
	// 	for (int i = 0; i < c; i++) {
	// 		float dotV = float2.Dot(corners[i], axis);
	// 		if (dotV < minmax.x) minmax.x = dotV;
	// 		if (dotV > minmax.y) minmax.y = dotV;
	// 	}
	//
	// 	return minmax;
	// }
	//
	// public static float2 SatTest(float2 axis, rect r) {
	// 	float2 minmax = new(float.MaxValue, -float.MaxValue);
	//
	// 	float dotV;
	//
	// 	dotV = float2.Dot(r.leftBottom, axis);
	// 	if (dotV < minmax.x) minmax.x = dotV;
	// 	if (dotV > minmax.y) minmax.y = dotV;
	// 	
	// 	dotV = float2.Dot(r.leftTop, axis);
	// 	if (dotV < minmax.x) minmax.x = dotV;
	// 	if (dotV > minmax.y) minmax.y = dotV;
	// 	
	// 	dotV = float2.Dot(r.rightTop, axis);
	// 	if (dotV < minmax.x) minmax.x = dotV;
	// 	if (dotV > minmax.y) minmax.y = dotV;
	// 	
	// 	dotV = float2.Dot(r.rightBottom, axis);
	// 	if (dotV < minmax.x) minmax.x = dotV;
	// 	if (dotV > minmax.y) minmax.y = dotV;
	//
	// 	return minmax;
	// }
	
	public static float2 Normal(float2 a, float2 b) => new(a.y - b.y, b.x - a.x);

	public static bool Overlaps(float2 a, float2 b) => InRange(b.x, a.x, a.y) || InRange(a.x, b.x, b.y);

	public static bool InRange(float v, float min, float max) => v >= min && v <= max;

	public static float2 RotateVector(float2 vec, float2 sincos) => new(vec.x * sincos.x - vec.y * sincos.y, vec.x * sincos.x + vec.y * sincos.y);

	public static float2[] RotateRect(rect a, float r) {
		float2 sincos = float2.SinCos(r, 1).yx;

		float2 center = new float2(a.midX, a.midY);
		float2[] points = new float2[4];

		points[0] = RotateVector(a.leftBottom - center, sincos) + center;
		points[1] = RotateVector(a.leftTop - center, sincos) + center;
		points[2] = RotateVector(a.rightTop - center, sincos) + center;
		points[3] = RotateVector(a.rightBottom - center, sincos) + center;

		return points;
	}
}