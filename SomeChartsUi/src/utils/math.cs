using System.Numerics;

namespace SomeChartsUi.utils {
	public static partial class math {
		public static int round(float v) => (int)v;

		// public static int min(int a, int b) => a <= b ? a : b;
		// public static int max(int a, int b) => a >= b ? a : b;
		// public static int clamp(int v, int min_, int max_) => min(max_, max(v, min_));
		//
		// public static uint min(uint a, uint b) => a <= b ? a : b;
		// public static uint max(uint a, uint b) => a >= b ? a : b;
		// public static uint clamp(uint v, uint min_, uint max_) => min(max_, max(v, min_));
		//
		// public static long min(long a, long b) => a <= b ? a : b;
		// public static long max(long a, long b) => a >= b ? a : b;
		// public static long clamp(long v, long min_, long max_) => min(max_, max(v, min_));
		//
		// public static ulong min(ulong a, ulong b) => a <= b ? a : b;
		// public static ulong max(ulong a, ulong b) => a >= b ? a : b;
		// public static ulong clamp(ulong v, ulong min_, ulong max_) => min(max_, max(v, min_));
		//
		// public static float min(float a, float b) => a <= b ? a : b;
		// public static float max(float a, float b) => a >= b ? a : b;
		// public static float clamp(float v, float min_, float max_) => min(max_, max(v, min_));
		//
		public static int minFast_i16(int a, int b) => b + ((a - b) & (a - b) >> 16);
		public static int maxFast_i16(int a, int b) => a - ((a - b) & (a - b) >> 16);
		public static int clampFast_i16(int v, int min_, int max_) => minFast_i16(max_, maxFast_i16(v, min_));

		public static int absFast(int a) => (a + (a >> 31)) ^ (a >> 31);
		//public static float abs(float a) => MathF.Abs(a);

		public static int Log10Floor(long v) => (int)Math.Floor(Math.Log10(v));
		public static int Log10Floor(int v) => (int)Math.Floor(Math.Log10(v));
		public static int DivLog(int v) => (int)(v / Math.Log(v));
		public static long DivLog(long v) => (long)(v / Math.Log(v));

		public static BigInteger DivLog(BigInteger v) => v / new BigInteger(BigInteger.Log(v));

		public static unsafe long ceilDiv(long a, long b) {
			bool check = a % b != 0;
			return a / b + *(byte*)&check;
		}

		public static unsafe void Sort_Tiny<T>(float* indexes, T* bindValues, int count) where T : unmanaged {
			for (int first = 1; first < count; first++) {
				float temp = indexes[first];
				T tempB = bindValues[first];
				int left = first - 1;
				while (left >= 0 && indexes[left] > temp) {
					indexes[left + 1] = indexes[left];
					bindValues[left + 1] = bindValues[left];
					left--;
				}

				indexes[left + 1] = temp;
				bindValues[left + 1] = tempB;
			}
		}

		public static unsafe void Sort_TinyU16<T>(T* values, int count, int indexOffset) where T : unmanaged {
			if ((sizeof(T) & 0b1) != 0) throw new InvalidDataException($"Sort_TinyU16 accept values with size of power of 2 ({typeof(T).Name})");

			ushort* indexes = (ushort*)((byte*)values + indexOffset);
			int indexAdd = sizeof(T) >> 1;
			for (int first = 1; first < count; first++) {
				int temp = indexes[first * indexAdd];
				T tempB = values[first];
				int left = first - 1;
				while (left >= 0 && indexes[left * indexAdd] > temp) {
					values[left + 1] = values[left];
					left--;
				}

				values[left + 1] = tempB;
			}
		}


		public static unsafe (float v, int ind) GetMinIndex(float* values, int count) {
			float minV = values[0];
			int minInd = 0;
			for (int i = 1; i < count; i++) {
				if (values[i] >= minV) continue;
				minV = values[i];
				minInd = i;
			}

			return (minV, minInd);
		}
	}
}