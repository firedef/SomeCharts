/* GENERATED USING T4 */

// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
using SomeChartsUi.themes.colors;
namespace SomeChartsUi.utils.vectors;

public struct float2 {
#region staticFields

    public static readonly float2 zero = new(0);
    public static readonly float2 one = new(1);
    public static readonly float2 maxValue = new(float.MaxValue);
    public static readonly float2 minValue = new(float.MinValue);
    public static readonly float2 up = new(0,1);
    public static readonly float2 down = new(0,-1);
    public static readonly float2 right = new(1,0);
    public static readonly float2 left = new(-1,0);


#endregion staticFields

#region axis

    // x axis
    public float x;
    public float2 xx => new(x,x);
    public float3 xxx => new(x,x,x);
    public float4 xxxx => new(x,x,x,x);
    public float4 xxxy => new(x,x,x,y);
    public float3 xxy => new(x,x,y);
    public float4 xxyx => new(x,x,y,x);
    public float4 xxyy => new(x,x,y,y);
    public float2 xy => new(x,y);
    public float3 xyx => new(x,y,x);
    public float4 xyxx => new(x,y,x,x);
    public float4 xyxy => new(x,y,x,y);
    public float3 xyy => new(x,y,y);
    public float4 xyyx => new(x,y,y,x);
    public float4 xyyy => new(x,y,y,y);
    // y axis
    public float y;
    public float2 yx => new(y,x);
    public float3 yxx => new(y,x,x);
    public float4 yxxx => new(y,x,x,x);
    public float4 yxxy => new(y,x,x,y);
    public float3 yxy => new(y,x,y);
    public float4 yxyx => new(y,x,y,x);
    public float4 yxyy => new(y,x,y,y);
    public float2 yy => new(y,y);
    public float3 yyx => new(y,y,x);
    public float4 yyxx => new(y,y,x,x);
    public float4 yyxy => new(y,y,x,y);
    public float3 yyy => new(y,y,y);
    public float4 yyyx => new(y,y,y,x);
    public float4 yyyy => new(y,y,y,y);

#endregion axis

#region constructors
    
    public float2(float x = 0, float y = 0) {
        this.x = x;
        this.y = y;
    }
    public float2(float v) : this(v, v) {}

#endregion constructors

#region functions

    public float lengthSq => x*x + y*y;
    public float length => math.sqrt(lengthSq);

    public float sum => x + y;
    public float mul => x * y;
    public float avg => sum * 0.5f;
    public float min => math.min(x, y);
    public float max => math.max(x, y);

    public float2 normalized { get { float len = 1 / length; return this * len; } }

    public float2 WithLength(float l) { float len = l / length; return this * len; }

    public void FlipX() => x = -x;
    public void FlipY() => y = -y;

#endregion functions

#region operators

    // binary operator '+'
    public static float2 operator +(float2 a, float2 b) => new(a.x + b.x, a.y + b.y);
    public static float2 operator +(float a, float2 b) => new(a + b.x, a + b.y);
    public static float2 operator +(float2 a, float b) => new(a.x + b, a.y + b);
    // binary operator '-'
    public static float2 operator -(float2 a, float2 b) => new(a.x - b.x, a.y - b.y);
    public static float2 operator -(float a, float2 b) => new(a - b.x, a - b.y);
    public static float2 operator -(float2 a, float b) => new(a.x - b, a.y - b);
    // binary operator '*'
    public static float2 operator *(float2 a, float2 b) => new(a.x * b.x, a.y * b.y);
    public static float2 operator *(float a, float2 b) => new(a * b.x, a * b.y);
    public static float2 operator *(float2 a, float b) => new(a.x * b, a.y * b);
    // binary operator '/'
    public static float2 operator /(float2 a, float2 b) => new(a.x / b.x, a.y / b.y);
    public static float2 operator /(float a, float2 b) => new(a / b.x, a / b.y);
    public static float2 operator /(float2 a, float b) => new(a.x / b, a.y / b);
    // equality operator '=='
    public static bool operator ==(float2 a, float2 b) => a.x == b.x && a.y == b.y;
    public static bool operator ==(float a, float2 b) => a == b.x && a == b.y;
    public static bool operator ==(float2 a, float b) => a.x == b && a.y == b;
    // equality operator '!='
    public static bool operator !=(float2 a, float2 b) => a.x != b.x || a.y != b.y;
    public static bool operator !=(float a, float2 b) => a != b.x || a != b.y;
    public static bool operator !=(float2 a, float b) => a.x != b || a.y != b;
    // equality operator '>'
    public static bool operator >(float2 a, float2 b) => a.x > b.x && a.y > b.y;
    public static bool operator >(float a, float2 b) => a > b.x && a > b.y;
    public static bool operator >(float2 a, float b) => a.x > b && a.y > b;
    // equality operator '>='
    public static bool operator >=(float2 a, float2 b) => a.x >= b.x && a.y >= b.y;
    public static bool operator >=(float a, float2 b) => a >= b.x && a >= b.y;
    public static bool operator >=(float2 a, float b) => a.x >= b && a.y >= b;
    // equality operator '<'
    public static bool operator <(float2 a, float2 b) => a.x < b.x && a.y < b.y;
    public static bool operator <(float a, float2 b) => a < b.x && a < b.y;
    public static bool operator <(float2 a, float b) => a.x < b && a.y < b;
    // equality operator '<='
    public static bool operator <=(float2 a, float2 b) => a.x <= b.x && a.y <= b.y;
    public static bool operator <=(float a, float2 b) => a <= b.x && a <= b.y;
    public static bool operator <=(float2 a, float b) => a.x <= b && a.y <= b;
    // other operators
    public static float2 operator -(float2 a) => new(-a.x, -a.y);
    public static implicit operator float2(float v) => new(v);
    public static implicit operator float2(int v) => new(v);


#endregion operators

#region staticFunc

    public static float2 Min(float2 a, float2 b) => new(math.min(a.x, b.x), math.min(a.y, b.y));
    public static float2 Min(float2 a, float2 b, float2 c) => new(math.min(a.x, b.x, c.x), math.min(a.y, b.y, c.y));
    public static float2 Min(float2 a, float2 b, float2 c, float2 d) => new(math.min(a.x, b.x, c.x, d.x), math.min(a.y, b.y, c.y, d.y));

    public static float2 Max(float2 a, float2 b) => new(math.max(a.x, b.x), math.max(a.y, b.y));
    public static float2 Max(float2 a, float2 b, float2 c) => new(math.max(a.x, b.x, c.x), math.max(a.y, b.y, c.y));
    public static float2 Max(float2 a, float2 b, float2 c, float2 d) => new(math.max(a.x, b.x, c.x, d.x), math.max(a.y, b.y, c.y, d.y));

    public static float2 Clamp(float2 v, float2 min, float2 max) => Max(Min(v, max), min);

#endregion staticFunc

#region impl

    public bool Equals(float2 other) => x == other.x && y == other.y;
	public override bool Equals(object? obj) => obj is float2 other && Equals(other);
	public override int GetHashCode() => HashCode.Combine(x, y);

	public override string ToString() => $"({x},{y})";

#endregion impl

#region other

    public static float2 SinCos(float radians, float length) => new(MathF.Sin(radians) * length, MathF.Cos(radians) * length);
    public static float Dot(float2 a, float2 b) => a.x * b.x + a.y * b.y;

#endregion other
}
public struct float3 {
#region staticFields

    public static readonly float3 zero = new(0);
    public static readonly float3 one = new(1);
    public static readonly float3 maxValue = new(float.MaxValue);
    public static readonly float3 minValue = new(float.MinValue);
    public static readonly float3 up = new(0,1);
    public static readonly float3 down = new(0,-1);
    public static readonly float3 right = new(1,0);
    public static readonly float3 left = new(-1,0);

    public static readonly float3 front = new(0,0,1);
    public static readonly float3 back = new(0,0,-1);

#endregion staticFields

#region axis

    // x axis
    public float x;
    public float2 xx => new(x,x);
    public float3 xxx => new(x,x,x);
    public float4 xxxx => new(x,x,x,x);
    public float4 xxxy => new(x,x,x,y);
    public float4 xxxz => new(x,x,x,z);
    public float3 xxy => new(x,x,y);
    public float4 xxyx => new(x,x,y,x);
    public float4 xxyy => new(x,x,y,y);
    public float4 xxyz => new(x,x,y,z);
    public float3 xxz => new(x,x,z);
    public float4 xxzx => new(x,x,z,x);
    public float4 xxzy => new(x,x,z,y);
    public float4 xxzz => new(x,x,z,z);
    public float2 xy => new(x,y);
    public float3 xyx => new(x,y,x);
    public float4 xyxx => new(x,y,x,x);
    public float4 xyxy => new(x,y,x,y);
    public float4 xyxz => new(x,y,x,z);
    public float3 xyy => new(x,y,y);
    public float4 xyyx => new(x,y,y,x);
    public float4 xyyy => new(x,y,y,y);
    public float4 xyyz => new(x,y,y,z);
    public float3 xyz => new(x,y,z);
    public float4 xyzx => new(x,y,z,x);
    public float4 xyzy => new(x,y,z,y);
    public float4 xyzz => new(x,y,z,z);
    public float2 xz => new(x,z);
    public float3 xzx => new(x,z,x);
    public float4 xzxx => new(x,z,x,x);
    public float4 xzxy => new(x,z,x,y);
    public float4 xzxz => new(x,z,x,z);
    public float3 xzy => new(x,z,y);
    public float4 xzyx => new(x,z,y,x);
    public float4 xzyy => new(x,z,y,y);
    public float4 xzyz => new(x,z,y,z);
    public float3 xzz => new(x,z,z);
    public float4 xzzx => new(x,z,z,x);
    public float4 xzzy => new(x,z,z,y);
    public float4 xzzz => new(x,z,z,z);
    // y axis
    public float y;
    public float2 yx => new(y,x);
    public float3 yxx => new(y,x,x);
    public float4 yxxx => new(y,x,x,x);
    public float4 yxxy => new(y,x,x,y);
    public float4 yxxz => new(y,x,x,z);
    public float3 yxy => new(y,x,y);
    public float4 yxyx => new(y,x,y,x);
    public float4 yxyy => new(y,x,y,y);
    public float4 yxyz => new(y,x,y,z);
    public float3 yxz => new(y,x,z);
    public float4 yxzx => new(y,x,z,x);
    public float4 yxzy => new(y,x,z,y);
    public float4 yxzz => new(y,x,z,z);
    public float2 yy => new(y,y);
    public float3 yyx => new(y,y,x);
    public float4 yyxx => new(y,y,x,x);
    public float4 yyxy => new(y,y,x,y);
    public float4 yyxz => new(y,y,x,z);
    public float3 yyy => new(y,y,y);
    public float4 yyyx => new(y,y,y,x);
    public float4 yyyy => new(y,y,y,y);
    public float4 yyyz => new(y,y,y,z);
    public float3 yyz => new(y,y,z);
    public float4 yyzx => new(y,y,z,x);
    public float4 yyzy => new(y,y,z,y);
    public float4 yyzz => new(y,y,z,z);
    public float2 yz => new(y,z);
    public float3 yzx => new(y,z,x);
    public float4 yzxx => new(y,z,x,x);
    public float4 yzxy => new(y,z,x,y);
    public float4 yzxz => new(y,z,x,z);
    public float3 yzy => new(y,z,y);
    public float4 yzyx => new(y,z,y,x);
    public float4 yzyy => new(y,z,y,y);
    public float4 yzyz => new(y,z,y,z);
    public float3 yzz => new(y,z,z);
    public float4 yzzx => new(y,z,z,x);
    public float4 yzzy => new(y,z,z,y);
    public float4 yzzz => new(y,z,z,z);
    // z axis
    public float z;
    public float2 zx => new(z,x);
    public float3 zxx => new(z,x,x);
    public float4 zxxx => new(z,x,x,x);
    public float4 zxxy => new(z,x,x,y);
    public float4 zxxz => new(z,x,x,z);
    public float3 zxy => new(z,x,y);
    public float4 zxyx => new(z,x,y,x);
    public float4 zxyy => new(z,x,y,y);
    public float4 zxyz => new(z,x,y,z);
    public float3 zxz => new(z,x,z);
    public float4 zxzx => new(z,x,z,x);
    public float4 zxzy => new(z,x,z,y);
    public float4 zxzz => new(z,x,z,z);
    public float2 zy => new(z,y);
    public float3 zyx => new(z,y,x);
    public float4 zyxx => new(z,y,x,x);
    public float4 zyxy => new(z,y,x,y);
    public float4 zyxz => new(z,y,x,z);
    public float3 zyy => new(z,y,y);
    public float4 zyyx => new(z,y,y,x);
    public float4 zyyy => new(z,y,y,y);
    public float4 zyyz => new(z,y,y,z);
    public float3 zyz => new(z,y,z);
    public float4 zyzx => new(z,y,z,x);
    public float4 zyzy => new(z,y,z,y);
    public float4 zyzz => new(z,y,z,z);
    public float2 zz => new(z,z);
    public float3 zzx => new(z,z,x);
    public float4 zzxx => new(z,z,x,x);
    public float4 zzxy => new(z,z,x,y);
    public float4 zzxz => new(z,z,x,z);
    public float3 zzy => new(z,z,y);
    public float4 zzyx => new(z,z,y,x);
    public float4 zzyy => new(z,z,y,y);
    public float4 zzyz => new(z,z,y,z);
    public float3 zzz => new(z,z,z);
    public float4 zzzx => new(z,z,z,x);
    public float4 zzzy => new(z,z,z,y);
    public float4 zzzz => new(z,z,z,z);

#endregion axis

#region constructors
    
    public float3(float x = 0, float y = 0, float z = 0) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public float3(float v) : this(v, v, v) {}

#endregion constructors

#region functions

    public float lengthSq => x*x + y*y + z*z;
    public float length => math.sqrt(lengthSq);

    public float sum => x + y + z;
    public float mul => x * y * z;
    public float avg => sum * 0.3333333f;
    public float min => math.min(x, y, z);
    public float max => math.max(x, y, z);

    public float3 normalized { get { float len = 1 / length; return this * len; } }

    public float3 WithLength(float l) { float len = l / length; return this * len; }

    public void FlipX() => x = -x;
    public void FlipY() => y = -y;
    public void FlipZ() => z = -z;

#endregion functions

#region operators

    // binary operator '+'
    public static float3 operator +(float3 a, float3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
    public static float3 operator +(float a, float3 b) => new(a + b.x, a + b.y, a + b.z);
    public static float3 operator +(float3 a, float b) => new(a.x + b, a.y + b, a.z + b);
    // binary operator '-'
    public static float3 operator -(float3 a, float3 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
    public static float3 operator -(float a, float3 b) => new(a - b.x, a - b.y, a - b.z);
    public static float3 operator -(float3 a, float b) => new(a.x - b, a.y - b, a.z - b);
    // binary operator '*'
    public static float3 operator *(float3 a, float3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
    public static float3 operator *(float a, float3 b) => new(a * b.x, a * b.y, a * b.z);
    public static float3 operator *(float3 a, float b) => new(a.x * b, a.y * b, a.z * b);
    // binary operator '/'
    public static float3 operator /(float3 a, float3 b) => new(a.x / b.x, a.y / b.y, a.z / b.z);
    public static float3 operator /(float a, float3 b) => new(a / b.x, a / b.y, a / b.z);
    public static float3 operator /(float3 a, float b) => new(a.x / b, a.y / b, a.z / b);
    // equality operator '=='
    public static bool operator ==(float3 a, float3 b) => a.x == b.x && a.y == b.y && a.z == b.z;
    public static bool operator ==(float a, float3 b) => a == b.x && a == b.y && a == b.z;
    public static bool operator ==(float3 a, float b) => a.x == b && a.y == b && a.z == b;
    // equality operator '!='
    public static bool operator !=(float3 a, float3 b) => a.x != b.x || a.y != b.y || a.z != b.z;
    public static bool operator !=(float a, float3 b) => a != b.x || a != b.y || a != b.z;
    public static bool operator !=(float3 a, float b) => a.x != b || a.y != b || a.z != b;
    // equality operator '>'
    public static bool operator >(float3 a, float3 b) => a.x > b.x && a.y > b.y && a.z > b.z;
    public static bool operator >(float a, float3 b) => a > b.x && a > b.y && a > b.z;
    public static bool operator >(float3 a, float b) => a.x > b && a.y > b && a.z > b;
    // equality operator '>='
    public static bool operator >=(float3 a, float3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;
    public static bool operator >=(float a, float3 b) => a >= b.x && a >= b.y && a >= b.z;
    public static bool operator >=(float3 a, float b) => a.x >= b && a.y >= b && a.z >= b;
    // equality operator '<'
    public static bool operator <(float3 a, float3 b) => a.x < b.x && a.y < b.y && a.z < b.z;
    public static bool operator <(float a, float3 b) => a < b.x && a < b.y && a < b.z;
    public static bool operator <(float3 a, float b) => a.x < b && a.y < b && a.z < b;
    // equality operator '<='
    public static bool operator <=(float3 a, float3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;
    public static bool operator <=(float a, float3 b) => a <= b.x && a <= b.y && a <= b.z;
    public static bool operator <=(float3 a, float b) => a.x <= b && a.y <= b && a.z <= b;
    // other operators
    public static float3 operator -(float3 a) => new(-a.x, -a.y, -a.z);
    public static implicit operator float3(float v) => new(v);
    public static implicit operator float3(int v) => new(v);

    public float3(float2 a, float z) {
        x = a.x;
        y = a.y;

        this.z = z;
    }

    public static implicit operator float2(float3 v) => v.xy;
    public static implicit operator float3(float2 v) => new(v.x, v.y, 0);

#endregion operators

#region staticFunc

    public static float3 Min(float3 a, float3 b) => new(math.min(a.x, b.x), math.min(a.y, b.y), math.min(a.z, b.z));
    public static float3 Min(float3 a, float3 b, float3 c) => new(math.min(a.x, b.x, c.x), math.min(a.y, b.y, c.y), math.min(a.z, b.z, c.z));
    public static float3 Min(float3 a, float3 b, float3 c, float3 d) => new(math.min(a.x, b.x, c.x, d.x), math.min(a.y, b.y, c.y, d.y), math.min(a.z, b.z, c.z, d.z));

    public static float3 Max(float3 a, float3 b) => new(math.max(a.x, b.x), math.max(a.y, b.y), math.max(a.z, b.z));
    public static float3 Max(float3 a, float3 b, float3 c) => new(math.max(a.x, b.x, c.x), math.max(a.y, b.y, c.y), math.max(a.z, b.z, c.z));
    public static float3 Max(float3 a, float3 b, float3 c, float3 d) => new(math.max(a.x, b.x, c.x, d.x), math.max(a.y, b.y, c.y, d.y), math.max(a.z, b.z, c.z, d.z));

    public static float3 Clamp(float3 v, float3 min, float3 max) => Max(Min(v, max), min);

#endregion staticFunc

#region impl

    public bool Equals(float3 other) => x == other.x && y == other.y && z == other.z;
	public override bool Equals(object? obj) => obj is float3 other && Equals(other);
	public override int GetHashCode() => HashCode.Combine(x, y, z);

	public override string ToString() => $"({x},{y},{z})";

#endregion impl

#region other

    public static implicit operator float3(color v) => new(v.rF, v.gF, v.bF);
    public static implicit operator color(float3 v) => new(v.x, v.y, v.z);

    public static float3 Cross(float3 a, float3 b) => new(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
    public static float Dot(float3 a, float3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

#endregion other
}
public struct float4 {
#region staticFields

    public static readonly float4 zero = new(0);
    public static readonly float4 one = new(1);
    public static readonly float4 maxValue = new(float.MaxValue);
    public static readonly float4 minValue = new(float.MinValue);
    public static readonly float4 up = new(0,1);
    public static readonly float4 down = new(0,-1);
    public static readonly float4 right = new(1,0);
    public static readonly float4 left = new(-1,0);

    public static readonly float4 front = new(0,0,1);
    public static readonly float4 back = new(0,0,-1);

#endregion staticFields

#region axis

    // x axis
    public float x;
    public float2 xx => new(x,x);
    public float3 xxx => new(x,x,x);
    public float4 xxxx => new(x,x,x,x);
    public float4 xxxy => new(x,x,x,y);
    public float4 xxxz => new(x,x,x,z);
    public float4 xxxw => new(x,x,x,w);
    public float3 xxy => new(x,x,y);
    public float4 xxyx => new(x,x,y,x);
    public float4 xxyy => new(x,x,y,y);
    public float4 xxyz => new(x,x,y,z);
    public float4 xxyw => new(x,x,y,w);
    public float3 xxz => new(x,x,z);
    public float4 xxzx => new(x,x,z,x);
    public float4 xxzy => new(x,x,z,y);
    public float4 xxzz => new(x,x,z,z);
    public float4 xxzw => new(x,x,z,w);
    public float3 xxw => new(x,x,w);
    public float4 xxwx => new(x,x,w,x);
    public float4 xxwy => new(x,x,w,y);
    public float4 xxwz => new(x,x,w,z);
    public float4 xxww => new(x,x,w,w);
    public float2 xy => new(x,y);
    public float3 xyx => new(x,y,x);
    public float4 xyxx => new(x,y,x,x);
    public float4 xyxy => new(x,y,x,y);
    public float4 xyxz => new(x,y,x,z);
    public float4 xyxw => new(x,y,x,w);
    public float3 xyy => new(x,y,y);
    public float4 xyyx => new(x,y,y,x);
    public float4 xyyy => new(x,y,y,y);
    public float4 xyyz => new(x,y,y,z);
    public float4 xyyw => new(x,y,y,w);
    public float3 xyz => new(x,y,z);
    public float4 xyzx => new(x,y,z,x);
    public float4 xyzy => new(x,y,z,y);
    public float4 xyzz => new(x,y,z,z);
    public float4 xyzw => new(x,y,z,w);
    public float3 xyw => new(x,y,w);
    public float4 xywx => new(x,y,w,x);
    public float4 xywy => new(x,y,w,y);
    public float4 xywz => new(x,y,w,z);
    public float4 xyww => new(x,y,w,w);
    public float2 xz => new(x,z);
    public float3 xzx => new(x,z,x);
    public float4 xzxx => new(x,z,x,x);
    public float4 xzxy => new(x,z,x,y);
    public float4 xzxz => new(x,z,x,z);
    public float4 xzxw => new(x,z,x,w);
    public float3 xzy => new(x,z,y);
    public float4 xzyx => new(x,z,y,x);
    public float4 xzyy => new(x,z,y,y);
    public float4 xzyz => new(x,z,y,z);
    public float4 xzyw => new(x,z,y,w);
    public float3 xzz => new(x,z,z);
    public float4 xzzx => new(x,z,z,x);
    public float4 xzzy => new(x,z,z,y);
    public float4 xzzz => new(x,z,z,z);
    public float4 xzzw => new(x,z,z,w);
    public float3 xzw => new(x,z,w);
    public float4 xzwx => new(x,z,w,x);
    public float4 xzwy => new(x,z,w,y);
    public float4 xzwz => new(x,z,w,z);
    public float4 xzww => new(x,z,w,w);
    public float2 xw => new(x,w);
    public float3 xwx => new(x,w,x);
    public float4 xwxx => new(x,w,x,x);
    public float4 xwxy => new(x,w,x,y);
    public float4 xwxz => new(x,w,x,z);
    public float4 xwxw => new(x,w,x,w);
    public float3 xwy => new(x,w,y);
    public float4 xwyx => new(x,w,y,x);
    public float4 xwyy => new(x,w,y,y);
    public float4 xwyz => new(x,w,y,z);
    public float4 xwyw => new(x,w,y,w);
    public float3 xwz => new(x,w,z);
    public float4 xwzx => new(x,w,z,x);
    public float4 xwzy => new(x,w,z,y);
    public float4 xwzz => new(x,w,z,z);
    public float4 xwzw => new(x,w,z,w);
    public float3 xww => new(x,w,w);
    public float4 xwwx => new(x,w,w,x);
    public float4 xwwy => new(x,w,w,y);
    public float4 xwwz => new(x,w,w,z);
    public float4 xwww => new(x,w,w,w);
    // y axis
    public float y;
    public float2 yx => new(y,x);
    public float3 yxx => new(y,x,x);
    public float4 yxxx => new(y,x,x,x);
    public float4 yxxy => new(y,x,x,y);
    public float4 yxxz => new(y,x,x,z);
    public float4 yxxw => new(y,x,x,w);
    public float3 yxy => new(y,x,y);
    public float4 yxyx => new(y,x,y,x);
    public float4 yxyy => new(y,x,y,y);
    public float4 yxyz => new(y,x,y,z);
    public float4 yxyw => new(y,x,y,w);
    public float3 yxz => new(y,x,z);
    public float4 yxzx => new(y,x,z,x);
    public float4 yxzy => new(y,x,z,y);
    public float4 yxzz => new(y,x,z,z);
    public float4 yxzw => new(y,x,z,w);
    public float3 yxw => new(y,x,w);
    public float4 yxwx => new(y,x,w,x);
    public float4 yxwy => new(y,x,w,y);
    public float4 yxwz => new(y,x,w,z);
    public float4 yxww => new(y,x,w,w);
    public float2 yy => new(y,y);
    public float3 yyx => new(y,y,x);
    public float4 yyxx => new(y,y,x,x);
    public float4 yyxy => new(y,y,x,y);
    public float4 yyxz => new(y,y,x,z);
    public float4 yyxw => new(y,y,x,w);
    public float3 yyy => new(y,y,y);
    public float4 yyyx => new(y,y,y,x);
    public float4 yyyy => new(y,y,y,y);
    public float4 yyyz => new(y,y,y,z);
    public float4 yyyw => new(y,y,y,w);
    public float3 yyz => new(y,y,z);
    public float4 yyzx => new(y,y,z,x);
    public float4 yyzy => new(y,y,z,y);
    public float4 yyzz => new(y,y,z,z);
    public float4 yyzw => new(y,y,z,w);
    public float3 yyw => new(y,y,w);
    public float4 yywx => new(y,y,w,x);
    public float4 yywy => new(y,y,w,y);
    public float4 yywz => new(y,y,w,z);
    public float4 yyww => new(y,y,w,w);
    public float2 yz => new(y,z);
    public float3 yzx => new(y,z,x);
    public float4 yzxx => new(y,z,x,x);
    public float4 yzxy => new(y,z,x,y);
    public float4 yzxz => new(y,z,x,z);
    public float4 yzxw => new(y,z,x,w);
    public float3 yzy => new(y,z,y);
    public float4 yzyx => new(y,z,y,x);
    public float4 yzyy => new(y,z,y,y);
    public float4 yzyz => new(y,z,y,z);
    public float4 yzyw => new(y,z,y,w);
    public float3 yzz => new(y,z,z);
    public float4 yzzx => new(y,z,z,x);
    public float4 yzzy => new(y,z,z,y);
    public float4 yzzz => new(y,z,z,z);
    public float4 yzzw => new(y,z,z,w);
    public float3 yzw => new(y,z,w);
    public float4 yzwx => new(y,z,w,x);
    public float4 yzwy => new(y,z,w,y);
    public float4 yzwz => new(y,z,w,z);
    public float4 yzww => new(y,z,w,w);
    public float2 yw => new(y,w);
    public float3 ywx => new(y,w,x);
    public float4 ywxx => new(y,w,x,x);
    public float4 ywxy => new(y,w,x,y);
    public float4 ywxz => new(y,w,x,z);
    public float4 ywxw => new(y,w,x,w);
    public float3 ywy => new(y,w,y);
    public float4 ywyx => new(y,w,y,x);
    public float4 ywyy => new(y,w,y,y);
    public float4 ywyz => new(y,w,y,z);
    public float4 ywyw => new(y,w,y,w);
    public float3 ywz => new(y,w,z);
    public float4 ywzx => new(y,w,z,x);
    public float4 ywzy => new(y,w,z,y);
    public float4 ywzz => new(y,w,z,z);
    public float4 ywzw => new(y,w,z,w);
    public float3 yww => new(y,w,w);
    public float4 ywwx => new(y,w,w,x);
    public float4 ywwy => new(y,w,w,y);
    public float4 ywwz => new(y,w,w,z);
    public float4 ywww => new(y,w,w,w);
    // z axis
    public float z;
    public float2 zx => new(z,x);
    public float3 zxx => new(z,x,x);
    public float4 zxxx => new(z,x,x,x);
    public float4 zxxy => new(z,x,x,y);
    public float4 zxxz => new(z,x,x,z);
    public float4 zxxw => new(z,x,x,w);
    public float3 zxy => new(z,x,y);
    public float4 zxyx => new(z,x,y,x);
    public float4 zxyy => new(z,x,y,y);
    public float4 zxyz => new(z,x,y,z);
    public float4 zxyw => new(z,x,y,w);
    public float3 zxz => new(z,x,z);
    public float4 zxzx => new(z,x,z,x);
    public float4 zxzy => new(z,x,z,y);
    public float4 zxzz => new(z,x,z,z);
    public float4 zxzw => new(z,x,z,w);
    public float3 zxw => new(z,x,w);
    public float4 zxwx => new(z,x,w,x);
    public float4 zxwy => new(z,x,w,y);
    public float4 zxwz => new(z,x,w,z);
    public float4 zxww => new(z,x,w,w);
    public float2 zy => new(z,y);
    public float3 zyx => new(z,y,x);
    public float4 zyxx => new(z,y,x,x);
    public float4 zyxy => new(z,y,x,y);
    public float4 zyxz => new(z,y,x,z);
    public float4 zyxw => new(z,y,x,w);
    public float3 zyy => new(z,y,y);
    public float4 zyyx => new(z,y,y,x);
    public float4 zyyy => new(z,y,y,y);
    public float4 zyyz => new(z,y,y,z);
    public float4 zyyw => new(z,y,y,w);
    public float3 zyz => new(z,y,z);
    public float4 zyzx => new(z,y,z,x);
    public float4 zyzy => new(z,y,z,y);
    public float4 zyzz => new(z,y,z,z);
    public float4 zyzw => new(z,y,z,w);
    public float3 zyw => new(z,y,w);
    public float4 zywx => new(z,y,w,x);
    public float4 zywy => new(z,y,w,y);
    public float4 zywz => new(z,y,w,z);
    public float4 zyww => new(z,y,w,w);
    public float2 zz => new(z,z);
    public float3 zzx => new(z,z,x);
    public float4 zzxx => new(z,z,x,x);
    public float4 zzxy => new(z,z,x,y);
    public float4 zzxz => new(z,z,x,z);
    public float4 zzxw => new(z,z,x,w);
    public float3 zzy => new(z,z,y);
    public float4 zzyx => new(z,z,y,x);
    public float4 zzyy => new(z,z,y,y);
    public float4 zzyz => new(z,z,y,z);
    public float4 zzyw => new(z,z,y,w);
    public float3 zzz => new(z,z,z);
    public float4 zzzx => new(z,z,z,x);
    public float4 zzzy => new(z,z,z,y);
    public float4 zzzz => new(z,z,z,z);
    public float4 zzzw => new(z,z,z,w);
    public float3 zzw => new(z,z,w);
    public float4 zzwx => new(z,z,w,x);
    public float4 zzwy => new(z,z,w,y);
    public float4 zzwz => new(z,z,w,z);
    public float4 zzww => new(z,z,w,w);
    public float2 zw => new(z,w);
    public float3 zwx => new(z,w,x);
    public float4 zwxx => new(z,w,x,x);
    public float4 zwxy => new(z,w,x,y);
    public float4 zwxz => new(z,w,x,z);
    public float4 zwxw => new(z,w,x,w);
    public float3 zwy => new(z,w,y);
    public float4 zwyx => new(z,w,y,x);
    public float4 zwyy => new(z,w,y,y);
    public float4 zwyz => new(z,w,y,z);
    public float4 zwyw => new(z,w,y,w);
    public float3 zwz => new(z,w,z);
    public float4 zwzx => new(z,w,z,x);
    public float4 zwzy => new(z,w,z,y);
    public float4 zwzz => new(z,w,z,z);
    public float4 zwzw => new(z,w,z,w);
    public float3 zww => new(z,w,w);
    public float4 zwwx => new(z,w,w,x);
    public float4 zwwy => new(z,w,w,y);
    public float4 zwwz => new(z,w,w,z);
    public float4 zwww => new(z,w,w,w);
    // w axis
    public float w;
    public float2 wx => new(w,x);
    public float3 wxx => new(w,x,x);
    public float4 wxxx => new(w,x,x,x);
    public float4 wxxy => new(w,x,x,y);
    public float4 wxxz => new(w,x,x,z);
    public float4 wxxw => new(w,x,x,w);
    public float3 wxy => new(w,x,y);
    public float4 wxyx => new(w,x,y,x);
    public float4 wxyy => new(w,x,y,y);
    public float4 wxyz => new(w,x,y,z);
    public float4 wxyw => new(w,x,y,w);
    public float3 wxz => new(w,x,z);
    public float4 wxzx => new(w,x,z,x);
    public float4 wxzy => new(w,x,z,y);
    public float4 wxzz => new(w,x,z,z);
    public float4 wxzw => new(w,x,z,w);
    public float3 wxw => new(w,x,w);
    public float4 wxwx => new(w,x,w,x);
    public float4 wxwy => new(w,x,w,y);
    public float4 wxwz => new(w,x,w,z);
    public float4 wxww => new(w,x,w,w);
    public float2 wy => new(w,y);
    public float3 wyx => new(w,y,x);
    public float4 wyxx => new(w,y,x,x);
    public float4 wyxy => new(w,y,x,y);
    public float4 wyxz => new(w,y,x,z);
    public float4 wyxw => new(w,y,x,w);
    public float3 wyy => new(w,y,y);
    public float4 wyyx => new(w,y,y,x);
    public float4 wyyy => new(w,y,y,y);
    public float4 wyyz => new(w,y,y,z);
    public float4 wyyw => new(w,y,y,w);
    public float3 wyz => new(w,y,z);
    public float4 wyzx => new(w,y,z,x);
    public float4 wyzy => new(w,y,z,y);
    public float4 wyzz => new(w,y,z,z);
    public float4 wyzw => new(w,y,z,w);
    public float3 wyw => new(w,y,w);
    public float4 wywx => new(w,y,w,x);
    public float4 wywy => new(w,y,w,y);
    public float4 wywz => new(w,y,w,z);
    public float4 wyww => new(w,y,w,w);
    public float2 wz => new(w,z);
    public float3 wzx => new(w,z,x);
    public float4 wzxx => new(w,z,x,x);
    public float4 wzxy => new(w,z,x,y);
    public float4 wzxz => new(w,z,x,z);
    public float4 wzxw => new(w,z,x,w);
    public float3 wzy => new(w,z,y);
    public float4 wzyx => new(w,z,y,x);
    public float4 wzyy => new(w,z,y,y);
    public float4 wzyz => new(w,z,y,z);
    public float4 wzyw => new(w,z,y,w);
    public float3 wzz => new(w,z,z);
    public float4 wzzx => new(w,z,z,x);
    public float4 wzzy => new(w,z,z,y);
    public float4 wzzz => new(w,z,z,z);
    public float4 wzzw => new(w,z,z,w);
    public float3 wzw => new(w,z,w);
    public float4 wzwx => new(w,z,w,x);
    public float4 wzwy => new(w,z,w,y);
    public float4 wzwz => new(w,z,w,z);
    public float4 wzww => new(w,z,w,w);
    public float2 ww => new(w,w);
    public float3 wwx => new(w,w,x);
    public float4 wwxx => new(w,w,x,x);
    public float4 wwxy => new(w,w,x,y);
    public float4 wwxz => new(w,w,x,z);
    public float4 wwxw => new(w,w,x,w);
    public float3 wwy => new(w,w,y);
    public float4 wwyx => new(w,w,y,x);
    public float4 wwyy => new(w,w,y,y);
    public float4 wwyz => new(w,w,y,z);
    public float4 wwyw => new(w,w,y,w);
    public float3 wwz => new(w,w,z);
    public float4 wwzx => new(w,w,z,x);
    public float4 wwzy => new(w,w,z,y);
    public float4 wwzz => new(w,w,z,z);
    public float4 wwzw => new(w,w,z,w);
    public float3 www => new(w,w,w);
    public float4 wwwx => new(w,w,w,x);
    public float4 wwwy => new(w,w,w,y);
    public float4 wwwz => new(w,w,w,z);
    public float4 wwww => new(w,w,w,w);

#endregion axis

#region constructors
    
    public float4(float x = 0, float y = 0, float z = 0, float w = 0) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }
    public float4(float v) : this(v, v, v, v) {}

#endregion constructors

#region functions

    public float lengthSq => x*x + y*y + z*z + w*w;
    public float length => math.sqrt(lengthSq);

    public float sum => x + y + z + w;
    public float mul => x * y * z * w;
    public float avg => sum * 0.25f;
    public float min => math.min(x, y, z, w);
    public float max => math.max(x, y, z, w);

    public float4 normalized { get { float len = 1 / length; return this * len; } }

    public float4 WithLength(float l) { float len = l / length; return this * len; }

    public void FlipX() => x = -x;
    public void FlipY() => y = -y;
    public void FlipZ() => z = -z;
    public void FlipW() => w = -w;

#endregion functions

#region operators

    // binary operator '+'
    public static float4 operator +(float4 a, float4 b) => new(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
    public static float4 operator +(float a, float4 b) => new(a + b.x, a + b.y, a + b.z, a + b.w);
    public static float4 operator +(float4 a, float b) => new(a.x + b, a.y + b, a.z + b, a.w + b);
    // binary operator '-'
    public static float4 operator -(float4 a, float4 b) => new(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
    public static float4 operator -(float a, float4 b) => new(a - b.x, a - b.y, a - b.z, a - b.w);
    public static float4 operator -(float4 a, float b) => new(a.x - b, a.y - b, a.z - b, a.w - b);
    // binary operator '*'
    public static float4 operator *(float4 a, float4 b) => new(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
    public static float4 operator *(float a, float4 b) => new(a * b.x, a * b.y, a * b.z, a * b.w);
    public static float4 operator *(float4 a, float b) => new(a.x * b, a.y * b, a.z * b, a.w * b);
    // binary operator '/'
    public static float4 operator /(float4 a, float4 b) => new(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w);
    public static float4 operator /(float a, float4 b) => new(a / b.x, a / b.y, a / b.z, a / b.w);
    public static float4 operator /(float4 a, float b) => new(a.x / b, a.y / b, a.z / b, a.w / b);
    // equality operator '=='
    public static bool operator ==(float4 a, float4 b) => a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
    public static bool operator ==(float a, float4 b) => a == b.x && a == b.y && a == b.z && a == b.w;
    public static bool operator ==(float4 a, float b) => a.x == b && a.y == b && a.z == b && a.w == b;
    // equality operator '!='
    public static bool operator !=(float4 a, float4 b) => a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;
    public static bool operator !=(float a, float4 b) => a != b.x || a != b.y || a != b.z || a != b.w;
    public static bool operator !=(float4 a, float b) => a.x != b || a.y != b || a.z != b || a.w != b;
    // equality operator '>'
    public static bool operator >(float4 a, float4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;
    public static bool operator >(float a, float4 b) => a > b.x && a > b.y && a > b.z && a > b.w;
    public static bool operator >(float4 a, float b) => a.x > b && a.y > b && a.z > b && a.w > b;
    // equality operator '>='
    public static bool operator >=(float4 a, float4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;
    public static bool operator >=(float a, float4 b) => a >= b.x && a >= b.y && a >= b.z && a >= b.w;
    public static bool operator >=(float4 a, float b) => a.x >= b && a.y >= b && a.z >= b && a.w >= b;
    // equality operator '<'
    public static bool operator <(float4 a, float4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;
    public static bool operator <(float a, float4 b) => a < b.x && a < b.y && a < b.z && a < b.w;
    public static bool operator <(float4 a, float b) => a.x < b && a.y < b && a.z < b && a.w < b;
    // equality operator '<='
    public static bool operator <=(float4 a, float4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;
    public static bool operator <=(float a, float4 b) => a <= b.x && a <= b.y && a <= b.z && a <= b.w;
    public static bool operator <=(float4 a, float b) => a.x <= b && a.y <= b && a.z <= b && a.w <= b;
    // other operators
    public static float4 operator -(float4 a) => new(-a.x, -a.y, -a.z, -a.w);
    public static implicit operator float4(float v) => new(v);
    public static implicit operator float4(int v) => new(v);

    public float4(float2 a, float z, float w) {
        x = a.x;
        y = a.y;

        this.z = z;
        this.w = w;
    }

    public static implicit operator float2(float4 v) => v.xy;
    public static implicit operator float4(float2 v) => new(v.x, v.y, 0, 0);
    public float4(float3 a, float w) {
        x = a.x;
        y = a.y;
        z = a.z;

        this.w = w;
    }

    public static implicit operator float3(float4 v) => v.xyz;
    public static implicit operator float4(float3 v) => new(v.x, v.y, v.z, 0);

#endregion operators

#region staticFunc

    public static float4 Min(float4 a, float4 b) => new(math.min(a.x, b.x), math.min(a.y, b.y), math.min(a.z, b.z), math.min(a.w, b.w));
    public static float4 Min(float4 a, float4 b, float4 c) => new(math.min(a.x, b.x, c.x), math.min(a.y, b.y, c.y), math.min(a.z, b.z, c.z), math.min(a.w, b.w, c.w));
    public static float4 Min(float4 a, float4 b, float4 c, float4 d) => new(math.min(a.x, b.x, c.x, d.x), math.min(a.y, b.y, c.y, d.y), math.min(a.z, b.z, c.z, d.z), math.min(a.w, b.w, c.w, d.w));

    public static float4 Max(float4 a, float4 b) => new(math.max(a.x, b.x), math.max(a.y, b.y), math.max(a.z, b.z), math.max(a.w, b.w));
    public static float4 Max(float4 a, float4 b, float4 c) => new(math.max(a.x, b.x, c.x), math.max(a.y, b.y, c.y), math.max(a.z, b.z, c.z), math.max(a.w, b.w, c.w));
    public static float4 Max(float4 a, float4 b, float4 c, float4 d) => new(math.max(a.x, b.x, c.x, d.x), math.max(a.y, b.y, c.y, d.y), math.max(a.z, b.z, c.z, d.z), math.max(a.w, b.w, c.w, d.w));

    public static float4 Clamp(float4 v, float4 min, float4 max) => Max(Min(v, max), min);

#endregion staticFunc

#region impl

    public bool Equals(float4 other) => x == other.x && y == other.y && z == other.z && w == other.w;
	public override bool Equals(object? obj) => obj is float4 other && Equals(other);
	public override int GetHashCode() => HashCode.Combine(x, y, z, w);

	public override string ToString() => $"({x},{y},{z},{w})";

#endregion impl

#region other

    public static implicit operator float4(color v) => new(v.rF, v.gF, v.bF, v.aF);
    public static implicit operator color(float4 v) => new(v.x, v.y, v.z, v.w);
    public static float Dot(float4 a, float4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

#endregion other
}
