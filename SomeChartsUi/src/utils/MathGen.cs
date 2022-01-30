/*
 * This file is generated using T4 (Tachyon5/src/utils/Mathematics/MathGen.tt)
 * Any changes will be reset on template update
 */

namespace SomeChartsUi.utils; 

public static partial class math {
	// * region floatOps : math and other useful operations with float
#region floatOps

	public static float min(float a, float b) => a >= b ? b : a;
	public static float min(float a, float b, float c) => min(a, min(b, c));
	public static float min(float a, float b, float c, float d) => min(min(a, b), min(c, d));

	public static float max(float a, float b) => a >= b ? a : b;
	public static float max(float a, float b, float c) => max(a, max(b, c));
	public static float max(float a, float b, float c, float d) => max(max(a, b), max(c, d));

	public static float pow2(float v) => v * v;
	public static float pow3(float v) => v * v * v;
	public static float pow4(float v) => v * v * v * v;
	public static float pow(float v, int e) {
		float result = 1;
		for (;;) {
			if ((e & 1) == 1) result *= v;
			e >>= 1;
			if (e == 0) break;
			v *= v;
		}

		return result;
	}

	///<summary>logarithm of base 10</summary>
	public static float log10(float v) => MathF.Log10(v);
	///<summary>logarithm of base 2</summary>
	public static float log2(float v) => MathF.Log2(v);
	///<summary>logarithm of base e (2.71828)</summary>
	public static float log(float v) => MathF.Log(v);
	///<summary>logarithm of base 'b'</summary>
	public static float log(float v, float b) => MathF.Log(v, b);

	public static float sqrt(float v) => MathF.Sqrt(v);

	///<summary>clamps value 'v' between 'a' and 'b'</summary>
	public static float clamp(float v, float a, float b) => max(a, min(v, b));

	///<summary>interpolate between 'a' and 'b' using value 't' (0 to 1)</summary>
	public static float lerp(float a, float b, float t) => a * (1 - t) + b * t;

	public static float abs(float a) => a < 0 ? -a : a;


	public static unsafe uint bitCastInt(float v) => *(uint*)&v;
	public static unsafe uint bitCastInt(float* v) => *(uint*)v;

	public static unsafe float bitCastFloatFromInt(uint v) => *(float*)&v;
	public static unsafe float bitCastFloatFromInt(uint* v) => *(float*)v;

	public static unsafe T bitCast<T>(float v) where T : unmanaged => *(T*)&v;
	public static unsafe T bitCast<T>(float* v) where T : unmanaged => *(T*)v;

	public static unsafe float nextValue(float v) => *(float*)(*(uint*)&v + 1);
	public static unsafe float nextValue(float* v) => *(float*)(*(uint*)v + 1);

	public static unsafe float previousValue(float v) => *(float*)(*(uint*)&v - 1);
	public static unsafe float previousValue(float* v) => *(float*)(*(uint*)v - 1);

#endregion floatOps

	// * region doubleOps : math and other useful operations with double
#region doubleOps

	public static double min(double a, double b) => a >= b ? b : a;
	public static double min(double a, double b, double c) => min(a, min(b, c));
	public static double min(double a, double b, double c, double d) => min(min(a, b), min(c, d));

	public static double max(double a, double b) => a >= b ? a : b;
	public static double max(double a, double b, double c) => max(a, max(b, c));
	public static double max(double a, double b, double c, double d) => max(max(a, b), max(c, d));

	public static double pow2(double v) => v * v;
	public static double pow3(double v) => v * v * v;
	public static double pow4(double v) => v * v * v * v;
	public static double pow(double v, int e) {
		double result = 1;
		for (;;) {
			if ((e & 1) == 1) result *= v;
			e >>= 1;
			if (e == 0) break;
			v *= v;
		}

		return result;
	}

	///<summary>logarithm of base 10</summary>
	public static double log10(double v) => Math.Log10(v);
	///<summary>logarithm of base 2</summary>
	public static double log2(double v) => Math.Log2(v);
	///<summary>logarithm of base e (2.71828)</summary>
	public static double log(double v) => Math.Log(v);
	///<summary>logarithm of base 'b'</summary>
	public static double log(double v, double b) => Math.Log(v, b);

	public static double sqrt(double v) => Math.Sqrt(v);

	///<summary>clamps value 'v' between 'a' and 'b'</summary>
	public static double clamp(double v, double a, double b) => max(a, min(v, b));

	///<summary>interpolate between 'a' and 'b' using value 't' (0 to 1)</summary>
	public static double lerp(double a, double b, double t) => a * (1 - t) + b * t;

	public static double abs(double a) => a < 0 ? -a : a;


	public static unsafe ulong bitCastInt(double v) => *(ulong*)&v;
	public static unsafe ulong bitCastInt(double* v) => *(ulong*)v;

	public static unsafe double bitCastDoubleFromInt(ulong v) => *(double*)&v;
	public static unsafe double bitCastDoubleFromInt(ulong* v) => *(double*)v;

	public static unsafe T bitCast<T>(double v) where T : unmanaged => *(T*)&v;
	public static unsafe T bitCast<T>(double* v) where T : unmanaged => *(T*)v;

	public static unsafe double nextValue(double v) => *(double*)(*(ulong*)&v + 1);
	public static unsafe double nextValue(double* v) => *(double*)(*(ulong*)v + 1);

	public static unsafe double previousValue(double v) => *(double*)(*(ulong*)&v - 1);
	public static unsafe double previousValue(double* v) => *(double*)(*(ulong*)v - 1);

#endregion doubleOps

	// * region decimalOps : math and other useful operations with decimal
#region decimalOps

	public static decimal min(decimal a, decimal b) => a >= b ? b : a;
	public static decimal min(decimal a, decimal b, decimal c) => min(a, min(b, c));
	public static decimal min(decimal a, decimal b, decimal c, decimal d) => min(min(a, b), min(c, d));

	public static decimal max(decimal a, decimal b) => a >= b ? a : b;
	public static decimal max(decimal a, decimal b, decimal c) => max(a, max(b, c));
	public static decimal max(decimal a, decimal b, decimal c, decimal d) => max(max(a, b), max(c, d));

	public static decimal pow2(decimal v) => v * v;
	public static decimal pow3(decimal v) => v * v * v;
	public static decimal pow4(decimal v) => v * v * v * v;
	public static decimal pow(decimal v, int e) {
		decimal result = 1;
		for (;;) {
			if ((e & 1) == 1) result *= v;
			e >>= 1;
			if (e == 0) break;
			v *= v;
		}

		return result;
	}

	///<summary>logarithm of base 10</summary>
	public static double log10(decimal v) => Math.Log10((double)v);
	///<summary>logarithm of base 2</summary>
	public static double log2(decimal v) => Math.Log2((double)v);
	///<summary>logarithm of base e (2.71828)</summary>
	public static double log(decimal v) => Math.Log((double)v);
	///<summary>logarithm of base 'b'</summary>
	public static double log(decimal v, double b) => Math.Log((double)v, b);

	public static double sqrt(decimal v) => Math.Sqrt((double)v);

	///<summary>clamps value 'v' between 'a' and 'b'</summary>
	public static decimal clamp(decimal v, decimal a, decimal b) => max(a, min(v, b));

	///<summary>interpolate between 'a' and 'b' using value 't' (0 to 1)</summary>
	public static decimal lerp(decimal a, decimal b, double t) => a * (decimal)(1 - t) + b * (decimal)t;

	public static decimal abs(decimal a) => a < 0 ? -a : a;

#endregion decimalOps

	// * region byteOps : math and other useful operations with byte
#region byteOps

	public static byte min(byte a, byte b) => a >= b ? b : a;
	public static byte min(byte a, byte b, byte c) => min(a, min(b, c));
	public static byte min(byte a, byte b, byte c, byte d) => min(min(a, b), min(c, d));

	public static byte max(byte a, byte b) => a >= b ? a : b;
	public static byte max(byte a, byte b, byte c) => max(a, max(b, c));
	public static byte max(byte a, byte b, byte c, byte d) => max(max(a, b), max(c, d));

	public static int pow2(byte v) => v * v;
	public static int pow3(byte v) => v * v * v;
	public static int pow4(byte v) => v * v * v * v;
	public static int pow(byte v, int e) {
		int result = 1;
		for (;;) {
			if ((e & 1) == 1) result *= v;
			e >>= 1;
			if (e == 0) break;
			v *= v;
		}

		return result;
	}

	///<summary>logarithm of base 10</summary>
	public static float log10(byte v) => MathF.Log10(v);
	///<summary>logarithm of base 2</summary>
	public static float log2(byte v) => MathF.Log2(v);
	///<summary>logarithm of base e (2.71828)</summary>
	public static float log(byte v) => MathF.Log(v);
	///<summary>logarithm of base 'b'</summary>
	public static float log(byte v, float b) => MathF.Log(v, b);

	public static float sqrt(byte v) => MathF.Sqrt(v);

	///<summary>clamps value 'v' between 'a' and 'b'</summary>
	public static byte clamp(byte v, byte a, byte b) => max(a, min(v, b));

	///<summary>interpolate between 'a' and 'b' using value 't' (0 to 1)</summary>
	public static byte lerp(byte a, byte b, float t) => (byte)(a * (1 - t) + b * t);


	public static unsafe byte bitCastInt(byte v) => *&v;
	public static unsafe byte bitCastInt(byte* v) => *v;

	public static unsafe byte bitCastByteFromInt(byte v) => *&v;
	public static unsafe byte bitCastByteFromInt(byte* v) => *v;

	public static unsafe T bitCast<T>(byte v) where T : unmanaged => *(T*)&v;
	public static unsafe T bitCast<T>(byte* v) where T : unmanaged => *(T*)v;

	public static unsafe byte nextValue(byte v) => *(byte*)(v + 1);
	public static unsafe byte nextValue(byte* v) => *(byte*)(*v + 1);

	public static unsafe byte previousValue(byte v) => *(byte*)(v - 1);
	public static unsafe byte previousValue(byte* v) => *(byte*)(*v - 1);

#endregion byteOps

	// * region sbyteOps : math and other useful operations with sbyte
#region sbyteOps

	public static sbyte min(sbyte a, sbyte b) => a >= b ? b : a;
	public static sbyte min(sbyte a, sbyte b, sbyte c) => min(a, min(b, c));
	public static sbyte min(sbyte a, sbyte b, sbyte c, sbyte d) => min(min(a, b), min(c, d));

	public static sbyte max(sbyte a, sbyte b) => a >= b ? a : b;
	public static sbyte max(sbyte a, sbyte b, sbyte c) => max(a, max(b, c));
	public static sbyte max(sbyte a, sbyte b, sbyte c, sbyte d) => max(max(a, b), max(c, d));

	public static int pow2(sbyte v) => v * v;
	public static int pow3(sbyte v) => v * v * v;
	public static int pow4(sbyte v) => v * v * v * v;
	public static int pow(sbyte v, int e) {
		int result = 1;
		for (;;) {
			if ((e & 1) == 1) result *= v;
			e >>= 1;
			if (e == 0) break;
			v *= v;
		}

		return result;
	}

	///<summary>logarithm of base 10</summary>
	public static float log10(sbyte v) => MathF.Log10(v);
	///<summary>logarithm of base 2</summary>
	public static float log2(sbyte v) => MathF.Log2(v);
	///<summary>logarithm of base e (2.71828)</summary>
	public static float log(sbyte v) => MathF.Log(v);
	///<summary>logarithm of base 'b'</summary>
	public static float log(sbyte v, float b) => MathF.Log(v, b);

	public static float sqrt(sbyte v) => MathF.Sqrt(v);

	///<summary>clamps value 'v' between 'a' and 'b'</summary>
	public static sbyte clamp(sbyte v, sbyte a, sbyte b) => max(a, min(v, b));

	///<summary>interpolate between 'a' and 'b' using value 't' (0 to 1)</summary>
	public static sbyte lerp(sbyte a, sbyte b, float t) => (sbyte)(a * (1 - t) + b * t);

	public static sbyte abs(sbyte a) => (sbyte)(a < 0 ? -a : a);


	public static unsafe byte bitCastInt(sbyte v) => *(byte*)&v;
	public static unsafe byte bitCastInt(sbyte* v) => *(byte*)v;

	public static unsafe sbyte bitCastSbyteFromInt(byte v) => *(sbyte*)&v;
	public static unsafe sbyte bitCastSbyteFromInt(byte* v) => *(sbyte*)v;

	public static unsafe T bitCast<T>(sbyte v) where T : unmanaged => *(T*)&v;
	public static unsafe T bitCast<T>(sbyte* v) where T : unmanaged => *(T*)v;

	public static unsafe sbyte nextValue(sbyte v) => *(sbyte*)(v + 1);
	public static unsafe sbyte nextValue(sbyte* v) => *(sbyte*)(*(byte*)v + 1);

	public static unsafe sbyte previousValue(sbyte v) => *(sbyte*)(v - 1);
	public static unsafe sbyte previousValue(sbyte* v) => *(sbyte*)(*(byte*)v - 1);

#endregion sbyteOps

	// * region shortOps : math and other useful operations with short
#region shortOps

	public static short min(short a, short b) => a >= b ? b : a;
	public static short min(short a, short b, short c) => min(a, min(b, c));
	public static short min(short a, short b, short c, short d) => min(min(a, b), min(c, d));

	public static short max(short a, short b) => a >= b ? a : b;
	public static short max(short a, short b, short c) => max(a, max(b, c));
	public static short max(short a, short b, short c, short d) => max(max(a, b), max(c, d));

	public static int pow2(short v) => v * v;
	public static int pow3(short v) => v * v * v;
	public static int pow4(short v) => v * v * v * v;
	public static int pow(short v, int e) {
		int result = 1;
		for (;;) {
			if ((e & 1) == 1) result *= v;
			e >>= 1;
			if (e == 0) break;
			v *= v;
		}

		return result;
	}

	///<summary>logarithm of base 10</summary>
	public static float log10(short v) => MathF.Log10(v);
	///<summary>logarithm of base 2</summary>
	public static float log2(short v) => MathF.Log2(v);
	///<summary>logarithm of base e (2.71828)</summary>
	public static float log(short v) => MathF.Log(v);
	///<summary>logarithm of base 'b'</summary>
	public static float log(short v, float b) => MathF.Log(v, b);

	public static float sqrt(short v) => MathF.Sqrt(v);

	///<summary>clamps value 'v' between 'a' and 'b'</summary>
	public static short clamp(short v, short a, short b) => max(a, min(v, b));

	///<summary>interpolate between 'a' and 'b' using value 't' (0 to 1)</summary>
	public static short lerp(short a, short b, float t) => (short)(a * (1 - t) + b * t);

	public static short abs(short a) => (short)(a < 0 ? -a : a);


	public static unsafe ushort bitCastInt(short v) => *(ushort*)&v;
	public static unsafe ushort bitCastInt(short* v) => *(ushort*)v;

	public static unsafe short bitCastShortFromInt(ushort v) => *(short*)&v;
	public static unsafe short bitCastShortFromInt(ushort* v) => *(short*)v;

	public static unsafe T bitCast<T>(short v) where T : unmanaged => *(T*)&v;
	public static unsafe T bitCast<T>(short* v) where T : unmanaged => *(T*)v;

	public static unsafe short nextValue(short v) => *(short*)(v + 1);
	public static unsafe short nextValue(short* v) => *(short*)(*(ushort*)v + 1);

	public static unsafe short previousValue(short v) => *(short*)(v - 1);
	public static unsafe short previousValue(short* v) => *(short*)(*(ushort*)v - 1);

#endregion shortOps

	// * region ushortOps : math and other useful operations with ushort
#region ushortOps

	public static ushort min(ushort a, ushort b) => a >= b ? b : a;
	public static ushort min(ushort a, ushort b, ushort c) => min(a, min(b, c));
	public static ushort min(ushort a, ushort b, ushort c, ushort d) => min(min(a, b), min(c, d));

	public static ushort max(ushort a, ushort b) => a >= b ? a : b;
	public static ushort max(ushort a, ushort b, ushort c) => max(a, max(b, c));
	public static ushort max(ushort a, ushort b, ushort c, ushort d) => max(max(a, b), max(c, d));

	public static int pow2(ushort v) => v * v;
	public static int pow3(ushort v) => v * v * v;
	public static int pow4(ushort v) => v * v * v * v;
	public static int pow(ushort v, int e) {
		int result = 1;
		for (;;) {
			if ((e & 1) == 1) result *= v;
			e >>= 1;
			if (e == 0) break;
			v *= v;
		}

		return result;
	}

	///<summary>logarithm of base 10</summary>
	public static float log10(ushort v) => MathF.Log10(v);
	///<summary>logarithm of base 2</summary>
	public static float log2(ushort v) => MathF.Log2(v);
	///<summary>logarithm of base e (2.71828)</summary>
	public static float log(ushort v) => MathF.Log(v);
	///<summary>logarithm of base 'b'</summary>
	public static float log(ushort v, float b) => MathF.Log(v, b);

	public static float sqrt(ushort v) => MathF.Sqrt(v);

	///<summary>clamps value 'v' between 'a' and 'b'</summary>
	public static ushort clamp(ushort v, ushort a, ushort b) => max(a, min(v, b));

	///<summary>interpolate between 'a' and 'b' using value 't' (0 to 1)</summary>
	public static ushort lerp(ushort a, ushort b, float t) => (ushort)(a * (1 - t) + b * t);


	public static unsafe ushort bitCastInt(ushort v) => *&v;
	public static unsafe ushort bitCastInt(ushort* v) => *v;

	public static unsafe ushort bitCastUshortFromInt(ushort v) => *&v;
	public static unsafe ushort bitCastUshortFromInt(ushort* v) => *v;

	public static unsafe T bitCast<T>(ushort v) where T : unmanaged => *(T*)&v;
	public static unsafe T bitCast<T>(ushort* v) where T : unmanaged => *(T*)v;

	public static unsafe ushort nextValue(ushort v) => *(ushort*)(v + 1);
	public static unsafe ushort nextValue(ushort* v) => *(ushort*)(*v + 1);

	public static unsafe ushort previousValue(ushort v) => *(ushort*)(v - 1);
	public static unsafe ushort previousValue(ushort* v) => *(ushort*)(*v - 1);

#endregion ushortOps

	// * region intOps : math and other useful operations with int
#region intOps

	public static int min(int a, int b) => a >= b ? b : a;
	public static int min(int a, int b, int c) => min(a, min(b, c));
	public static int min(int a, int b, int c, int d) => min(min(a, b), min(c, d));

	public static int max(int a, int b) => a >= b ? a : b;
	public static int max(int a, int b, int c) => max(a, max(b, c));
	public static int max(int a, int b, int c, int d) => max(max(a, b), max(c, d));

	public static int pow2(int v) => v * v;
	public static int pow3(int v) => v * v * v;
	public static int pow4(int v) => v * v * v * v;
	public static int pow(int v, int e) {
		int result = 1;
		for (;;) {
			if ((e & 1) == 1) result *= v;
			e >>= 1;
			if (e == 0) break;
			v *= v;
		}

		return result;
	}

	///<summary>logarithm of base 10</summary>
	public static float log10(int v) => MathF.Log10(v);
	///<summary>logarithm of base 2</summary>
	public static float log2(int v) => MathF.Log2(v);
	///<summary>logarithm of base e (2.71828)</summary>
	public static float log(int v) => MathF.Log(v);
	///<summary>logarithm of base 'b'</summary>
	public static float log(int v, float b) => MathF.Log(v, b);

	public static float sqrt(int v) => MathF.Sqrt(v);

	///<summary>clamps value 'v' between 'a' and 'b'</summary>
	public static int clamp(int v, int a, int b) => max(a, min(v, b));

	///<summary>interpolate between 'a' and 'b' using value 't' (0 to 1)</summary>
	public static int lerp(int a, int b, float t) => (int)(a * (1 - t) + b * t);

	public static int abs(int a) => a < 0 ? -a : a;


	public static unsafe uint bitCastInt(int v) => *(uint*)&v;
	public static unsafe uint bitCastInt(int* v) => *(uint*)v;

	public static unsafe int bitCastIntFromInt(uint v) => *(int*)&v;
	public static unsafe int bitCastIntFromInt(uint* v) => *(int*)v;

	public static unsafe T bitCast<T>(int v) where T : unmanaged => *(T*)&v;
	public static unsafe T bitCast<T>(int* v) where T : unmanaged => *(T*)v;

	public static unsafe int nextValue(int v) => *(int*)(v + 1);
	public static unsafe int nextValue(int* v) => *(int*)(*(uint*)v + 1);

	public static unsafe int previousValue(int v) => *(int*)(v - 1);
	public static unsafe int previousValue(int* v) => *(int*)(*(uint*)v - 1);

#endregion intOps

	// * region uintOps : math and other useful operations with uint
#region uintOps

	public static uint min(uint a, uint b) => a >= b ? b : a;
	public static uint min(uint a, uint b, uint c) => min(a, min(b, c));
	public static uint min(uint a, uint b, uint c, uint d) => min(min(a, b), min(c, d));

	public static uint max(uint a, uint b) => a >= b ? a : b;
	public static uint max(uint a, uint b, uint c) => max(a, max(b, c));
	public static uint max(uint a, uint b, uint c, uint d) => max(max(a, b), max(c, d));

	public static long pow2(uint v) => v * v;
	public static long pow3(uint v) => v * v * v;
	public static long pow4(uint v) => v * v * v * v;
	public static long pow(uint v, int e) {
		long result = 1;
		for (;;) {
			if ((e & 1) == 1) result *= v;
			e >>= 1;
			if (e == 0) break;
			v *= v;
		}

		return result;
	}

	///<summary>logarithm of base 10</summary>
	public static float log10(uint v) => MathF.Log10(v);
	///<summary>logarithm of base 2</summary>
	public static float log2(uint v) => MathF.Log2(v);
	///<summary>logarithm of base e (2.71828)</summary>
	public static float log(uint v) => MathF.Log(v);
	///<summary>logarithm of base 'b'</summary>
	public static float log(uint v, float b) => MathF.Log(v, b);

	public static float sqrt(uint v) => MathF.Sqrt(v);

	///<summary>clamps value 'v' between 'a' and 'b'</summary>
	public static uint clamp(uint v, uint a, uint b) => max(a, min(v, b));

	///<summary>interpolate between 'a' and 'b' using value 't' (0 to 1)</summary>
	public static uint lerp(uint a, uint b, float t) => (uint)(a * (1 - t) + b * t);


	public static unsafe uint bitCastInt(uint v) => *&v;
	public static unsafe uint bitCastInt(uint* v) => *v;

	public static unsafe uint bitCastUintFromInt(uint v) => *&v;
	public static unsafe uint bitCastUintFromInt(uint* v) => *v;

	public static unsafe T bitCast<T>(uint v) where T : unmanaged => *(T*)&v;
	public static unsafe T bitCast<T>(uint* v) where T : unmanaged => *(T*)v;

	public static unsafe uint nextValue(uint v) => *(uint*)(v + 1);
	public static unsafe uint nextValue(uint* v) => *(uint*)(*v + 1);

	public static unsafe uint previousValue(uint v) => *(uint*)(v - 1);
	public static unsafe uint previousValue(uint* v) => *(uint*)(*v - 1);

#endregion uintOps

	// * region longOps : math and other useful operations with long
#region longOps

	public static long min(long a, long b) => a >= b ? b : a;
	public static long min(long a, long b, long c) => min(a, min(b, c));
	public static long min(long a, long b, long c, long d) => min(min(a, b), min(c, d));

	public static long max(long a, long b) => a >= b ? a : b;
	public static long max(long a, long b, long c) => max(a, max(b, c));
	public static long max(long a, long b, long c, long d) => max(max(a, b), max(c, d));

	public static long pow2(long v) => v * v;
	public static long pow3(long v) => v * v * v;
	public static long pow4(long v) => v * v * v * v;
	public static long pow(long v, int e) {
		long result = 1;
		for (;;) {
			if ((e & 1) == 1) result *= v;
			e >>= 1;
			if (e == 0) break;
			v *= v;
		}

		return result;
	}

	///<summary>logarithm of base 10</summary>
	public static double log10(long v) => Math.Log10(v);
	///<summary>logarithm of base 2</summary>
	public static double log2(long v) => Math.Log2(v);
	///<summary>logarithm of base e (2.71828)</summary>
	public static double log(long v) => Math.Log(v);
	///<summary>logarithm of base 'b'</summary>
	public static double log(long v, double b) => Math.Log(v, b);

	public static double sqrt(long v) => Math.Sqrt(v);

	///<summary>clamps value 'v' between 'a' and 'b'</summary>
	public static long clamp(long v, long a, long b) => max(a, min(v, b));

	///<summary>interpolate between 'a' and 'b' using value 't' (0 to 1)</summary>
	public static long lerp(long a, long b, double t) => (long)(a * (1 - t) + b * t);

	public static long abs(long a) => a < 0 ? -a : a;


	public static unsafe ulong bitCastInt(long v) => *(ulong*)&v;
	public static unsafe ulong bitCastInt(long* v) => *(ulong*)v;

	public static unsafe long bitCastLongFromInt(ulong v) => *(long*)&v;
	public static unsafe long bitCastLongFromInt(ulong* v) => *(long*)v;

	public static unsafe T bitCast<T>(long v) where T : unmanaged => *(T*)&v;
	public static unsafe T bitCast<T>(long* v) where T : unmanaged => *(T*)v;

	public static unsafe long nextValue(long v) => *(long*)(v + 1);
	public static unsafe long nextValue(long* v) => *(long*)(*(ulong*)v + 1);

	public static unsafe long previousValue(long v) => *(long*)(v - 1);
	public static unsafe long previousValue(long* v) => *(long*)(*(ulong*)v - 1);

#endregion longOps

	// * region ulongOps : math and other useful operations with ulong
#region ulongOps

	public static ulong min(ulong a, ulong b) => a >= b ? b : a;
	public static ulong min(ulong a, ulong b, ulong c) => min(a, min(b, c));
	public static ulong min(ulong a, ulong b, ulong c, ulong d) => min(min(a, b), min(c, d));

	public static ulong max(ulong a, ulong b) => a >= b ? a : b;
	public static ulong max(ulong a, ulong b, ulong c) => max(a, max(b, c));
	public static ulong max(ulong a, ulong b, ulong c, ulong d) => max(max(a, b), max(c, d));

	public static ulong pow2(ulong v) => v * v;
	public static ulong pow3(ulong v) => v * v * v;
	public static ulong pow4(ulong v) => v * v * v * v;
	public static ulong pow(ulong v, int e) {
		ulong result = 1;
		for (;;) {
			if ((e & 1) == 1) result *= v;
			e >>= 1;
			if (e == 0) break;
			v *= v;
		}

		return result;
	}

	///<summary>logarithm of base 10</summary>
	public static double log10(ulong v) => Math.Log10(v);
	///<summary>logarithm of base 2</summary>
	public static double log2(ulong v) => Math.Log2(v);
	///<summary>logarithm of base e (2.71828)</summary>
	public static double log(ulong v) => Math.Log(v);
	///<summary>logarithm of base 'b'</summary>
	public static double log(ulong v, double b) => Math.Log(v, b);

	public static double sqrt(ulong v) => Math.Sqrt(v);

	///<summary>clamps value 'v' between 'a' and 'b'</summary>
	public static ulong clamp(ulong v, ulong a, ulong b) => max(a, min(v, b));

	///<summary>interpolate between 'a' and 'b' using value 't' (0 to 1)</summary>
	public static ulong lerp(ulong a, ulong b, double t) => (ulong)(a * (1 - t) + b * t);


	public static unsafe ulong bitCastInt(ulong v) => *&v;
	public static unsafe ulong bitCastInt(ulong* v) => *v;

	public static unsafe ulong bitCastUlongFromInt(ulong v) => *&v;
	public static unsafe ulong bitCastUlongFromInt(ulong* v) => *v;

	public static unsafe T bitCast<T>(ulong v) where T : unmanaged => *(T*)&v;
	public static unsafe T bitCast<T>(ulong* v) where T : unmanaged => *(T*)v;

	public static unsafe ulong nextValue(ulong v) => *(ulong*)(v + 1);
	public static unsafe ulong nextValue(ulong* v) => *(ulong*)(*v + 1);

	public static unsafe ulong previousValue(ulong v) => *(ulong*)(v - 1);
	public static unsafe ulong previousValue(ulong* v) => *(ulong*)(*v - 1);

#endregion ulongOps
}