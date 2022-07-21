using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.themes.colors;

namespace SomeChartsUi.data;

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