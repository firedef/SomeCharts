using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.themes.colors;

namespace SomeChartsUi.data;

public interface IChartManagedData<T> {
	public void GetValues(int start, int count, int downsample, T[] dest);
	public T GetValue(int i);
	public int GetLength();
	public int GetHashCode();
}

public interface IChartData<T> : IChartManagedData<T> where T : unmanaged {
	public unsafe void GetValues(int start, int count, int downsample, T* dest);
}

public class ArrayChartManagedData<T> : IChartManagedData<T> {
	public readonly T[] data;

	public ArrayChartManagedData(T[] data) => this.data = data;

	public void GetValues(int start, int count, int downsample, T[] dest) {
		for (int i = 0; i < count; i++)
			dest[i] = data[start + (i << downsample)];
	}
	public T GetValue(int i) => data[i];
	public int GetLength() => data.Length;
}

public class ArrayChartData<T> : ArrayChartManagedData<T>, IChartData<T> where T : unmanaged {
	public ArrayChartData(T[] data) : base(data) { }

	public unsafe void GetValues(int start, int count, int downsample, T* dest) {
		for (int i = 0; i < count; i++) dest[i] = data[start + (i << downsample)];
	}
}

public class CollectionChartManagedData<T> : IChartManagedData<T> {
	public readonly IEnumerable<T> data;

	public CollectionChartManagedData(IEnumerable<T> data) => this.data = data;

	public void GetValues(int start, int count, int downsample, T[] dest) {
		for (int i = 0; i < count; i++) dest[i] = data.ElementAt(start + (i << downsample));
	}
	public T GetValue(int i) => data.ElementAt(i);
	public int GetLength() => data.Count();
}

public class CollectionChartData<T> : CollectionChartManagedData<T>, IChartData<T> where T : unmanaged {
	public CollectionChartData(IEnumerable<T> data) : base(data) { }

	public unsafe void GetValues(int start, int count, int downsample, T* dest) {
		for (int i = 0; i < count; i++) dest[i] = data.ElementAt(start + (i << downsample));
	}
}

public class FuncChartManagedData<T> : IChartManagedData<T> {
	public readonly int length;
	public readonly Func<int, T> sample;

	public FuncChartManagedData(Func<int, T> sample, int length) {
		this.sample = sample;
		this.length = length;
	}

	public void GetValues(int start, int count, int downsample, T[] dest) {
		for (int i = 0; i < count; i++) dest[i] = sample(start + (i << downsample));
	}

	public T GetValue(int i) => sample(i);
	public int GetLength() => length;
}

public class FuncChartData<T> : FuncChartManagedData<T>, IChartData<T> where T : unmanaged {
	public FuncChartData(Func<int, T> sample, int length) : base(sample, length) { }

	public unsafe void GetValues(int start, int count, int downsample, T* dest) {
		for (int i = 0; i < count; i++) dest[i] = sample(start + (i << downsample));
	}
}

public class ConstChartManagedData<T> : IChartManagedData<T> {
	public int length;
	public T value;

	public ConstChartManagedData(T value, int length = 1) {
		this.value = value;
		this.length = length;
	}

	public void GetValues(int start, int count, int downsample, T[] dest) {
		for (int i = 0; i < count; i++) dest[i] = value;
	}
	public T GetValue(int i) => value;
	public int GetLength() => length;
}

public class ConstChartData<T> : ConstChartManagedData<T>, IChartData<T> where T : unmanaged {
	public ConstChartData(T value, int length = 1) : base(value, length) { }

	public unsafe void GetValues(int start, int count, int downsample, T* dest) {
		for (int i = 0; i < count; i++) dest[i] = value;
	}
}

public static class ChartDataExtensions {
	public static T[] GetValues<T>(this IChartManagedData<T> v, int start, int count, int downsample) {
		T[] arr = new T[count];
		v.GetValues(start, count, downsample, arr);
		return arr;
	}
	
	public static T[] GetValues<T>(this IChart2DManagedData<T> v, int2 start, int2 count, int downsample) {
		T[] arr = new T[count.x * count.y];
		v.GetValues(start, count, downsample, arr);
		return arr;
	}

	/// <summary>get values using stride</summary>
	/// <param name="v">data source</param>
	/// <param name="start">start index</param>
	/// <param name="count">count</param>
	/// <param name="downsample">downsample</param>
	/// <param name="dest">destination array</param>
	/// <param name="stride">stride of index <br/>value of 1 is default stride, 2 will skip every odd value etc.</param>
	public static unsafe void GetValuesWithStride<T>(this IChartData<T> v, int start, int count, int downsample, T* dest, int stride) where T : unmanaged {
		T* temp = stackalloc T[count];
		v.GetValues(start, count, downsample, temp);

		for (int i = 0, j = 0; i < count; i++, j += stride)
			dest[j] = temp[i];
	}

	public static unsafe void GetColors(this IChartData<indexedColor> v, int start, int count, int downsample, color* dest) {
		indexedColor* temp = stackalloc indexedColor[count];
		v.GetValues(start, count, downsample, temp);

		for (int i = 0; i < count; i++)
			dest[i] = temp[i].GetColor();
	}
}