// ReSharper disable CompareOfFloatsByEqualityOperator
namespace SomeChartsUi.utils.vectors;

public struct float2 {
	public static readonly float2 zero = new();
	public static readonly float2 one = new(1);
	
	public float x;
	public float y;

	public float2 xx => new(x);
	public float2 yy => new(y);
	public float2 yx => new(y,x);
	public float sum => x + y;
	
	public float avg => (x + y) * .5f;

	public float2 norm { get { float len = length; return new(x / len, y / len); } }

	public float2(float v) : this(v, v) { }

	public float2(float x, float y) {
		this.x = x;
		this.y = y;
	}

	public float lengthSq => x * x + y * y;
	public float length => MathF.Sqrt(x * x + y * y);

	public void FlipY() => y = -y;
	public void FlipX() => x = -x;
	public void Flip() => (x, y) = (-x, -y);
	
	

	public static implicit operator float2(float v) => new(v);
	public static implicit operator float2(int v) => new(v);

	public static float2 operator +(float2 a, float2 b) => new(a.x + b.x, a.y + b.y);
	public static float2 operator +(float2 a, float b) => new(a.x + b, a.y + b);
	public static float2 operator +(float2 a, int b) => new(a.x + b, a.y + b);
	public static float2 operator -(float2 a, float2 b) => new(a.x - b.x, a.y - b.y);
	public static float2 operator -(float2 a, float b) => new(a.x - b, a.y - b);
	public static float2 operator -(float2 a, int b) => new(a.x - b, a.y - b);
	
	public static float2 operator *(float2 a, float2 b) => new(a.x * b.x, a.y * b.y);
	public static float2 operator *(float2 a, float b) => new(a.x * b, a.y * b);
	public static float2 operator *(float2 a, int b) => new(a.x * b, a.y * b);
	public static float2 operator /(float2 a, float2 b) => new(a.x / b.x, a.y / b.y);
	public static float2 operator /(float2 a, float b) => new(a.x / b, a.y / b);
	public static float2 operator /(float2 a, int b) => new(a.x / b, a.y / b);
	
	public static bool operator ==(float2 a, float2 b) => a.x == b.x && a.y == b.y;
	public static bool operator !=(float2 a, float2 b) => a.x != b.x || a.y != b.y;
	public static bool operator ==(float2 a, float b) => a.x == b && a.y == b;
	public static bool operator !=(float2 a, float b) => a.x != b || a.y != b;
	
	public static bool operator >(float2 a, float2 b) => a.x > b.x && a.y > b.y;
	public static bool operator >(float2 a, float b) => a.x > b && a.y > b;
	public static bool operator >=(float2 a, float2 b) => a.x >= b.x && a.y >= b.y;
	public static bool operator >=(float2 a, float b) => a.x >= b && a.y >= b;
	public static bool operator <(float2 a, float2 b) => a.x < b.x && a.y < b.y;
	public static bool operator <(float2 a, float b) => a.x < b && a.y < b;
	public static bool operator <=(float2 a, float2 b) => a.x <= b.x && a.y <= b.y;
	public static bool operator <=(float2 a, float b) => a.x <= b && a.y <= b;

	public static float2 SinCos(float rad) => new(MathF.Sin(rad), MathF.Cos(rad));
	public static float2 SinCos(float rad, float len) => new(MathF.Sin(rad) * len, MathF.Cos(rad) * len);

	public static float2 Min(float2 a, float2 b) => new(math.min(a.x, b.x), math.min(a.y, b.y));
	public static float2 Max(float2 a, float2 b) => new(math.max(a.x, b.x), math.max(a.y, b.y));
	public static float2 Clamp(float2 v, float2 min, float2 max) => Max(Min(v, max), min);
	
	public bool Equals(float2 other) => x == other.x && y == other.y;
	public override bool Equals(object? obj) => obj is float2 other && Equals(other);
	public override int GetHashCode() => HashCode.Combine(x, y);

	public override string ToString() => $"({x},{y})";
}