// ReSharper disable CompareOfFloatsByEqualityOperator
namespace SomeChartsUi.utils.vectors;

public struct float3 {
	public static readonly float3 zero = new();
	public static readonly float3 one = new(1);
	
	public float x;
	public float y;
	public float z;

	public float3 xxx => new(x);
	public float3 yyy => new(y);
	public float3 zzz => new(z);
	
	public float avg => (x + y + z) * .33333f;

	public float3 norm { get { float len = 1/length; return new(x * len, y * len, z * len); } }

	public float3(float v) : this(v, v, v) { }

	public float3(float x, float y) {
		this.x = x;
		this.y = y;
		this.z = 0;
	}
	
	public float3(float x, float y, float z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public float lengthSq => x * x + y * y;
	public float length => MathF.Sqrt(x * x + y * y);

	public void FlipY() => y = -y;
	public void FlipX() => x = -x;
	public void FlipZ() => z = -z;
	public void Flip() => (x, y, z) = (-x, -y, -z);
	
	

	public static implicit operator float3(float v) => new(v);
	public static implicit operator float3(int v) => new(v);

	public static float3 operator +(float3 a, float3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
	public static float3 operator +(float3 a, float b) => new(a.x + b, a.y + b, a.z + b);
	public static float3 operator +(float3 a, int b) => new(a.x + b, a.y + b, a.z + b);
	public static float3 operator -(float3 a, float3 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
	public static float3 operator -(float3 a, float b) => new(a.x - b, a.y - b, a.z - b);
	public static float3 operator -(float3 a, int b) => new(a.x - b, a.y - b, a.z - b);
	
	public static float3 operator *(float3 a, float3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
	public static float3 operator *(float3 a, float b) => new(a.x * b, a.y * b, a.z * b);
	public static float3 operator *(float3 a, int b) => new(a.x * b, a.y * b, a.z * b);
	public static float3 operator /(float3 a, float3 b) => new(a.x / b.x, a.y / b.y, a.z / b.z);
	public static float3 operator /(float3 a, float b) => new(a.x / b, a.y / b, a.z / b);
	public static float3 operator /(float3 a, int b) => new(a.x / b, a.y / b, a.z / b);
	
	public static bool operator ==(float3 a, float3 b) => a.x == b.x && a.y == b.y && a.z == b.z;
	public static bool operator !=(float3 a, float3 b) => a.x != b.x || a.y != b.y || a.z != b.z;
	public static bool operator ==(float3 a, float b) => a.x == b && a.y == b && a.z == b;
	public static bool operator !=(float3 a, float b) => a.x != b || a.y != b || a.z != b;
	
	public static bool operator >(float3 a, float3 b) => a.x > b.x && a.y > b.y && a.z > b.z;
	public static bool operator >(float3 a, float b) => a.x > b && a.y > b && a.z > b;
	public static bool operator >=(float3 a, float3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;
	public static bool operator >=(float3 a, float b) => a.x >= b && a.y >= b && a.z >= b;
	public static bool operator <(float3 a, float3 b) => a.x < b.x && a.y < b.y && a.z < b.z;
	public static bool operator <(float3 a, float b) => a.x < b && a.y < b && a.z < b;
	public static bool operator <=(float3 a, float3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;
	public static bool operator <=(float3 a, float b) => a.x <= b && a.y <= b && a.z <= b;
	
	public static float3 Min(float3 a, float3 b) => new(math.min(a.x, b.x), math.min(a.y, b.y));
	public static float3 Max(float3 a, float3 b) => new(math.max(a.x, b.x), math.max(a.y, b.y));
	public static float3 Clamp(float3 v, float3 min, float3 max) => Max(Min(v, max), min);
	
	public bool Equals(float3 other) => x == other.x && y == other.y;
	public override bool Equals(object? obj) => obj is float3 other && Equals(other);
	public override int GetHashCode() => HashCode.Combine(x, y, z);

	public override string ToString() => $"({x},{y},{z})";
}