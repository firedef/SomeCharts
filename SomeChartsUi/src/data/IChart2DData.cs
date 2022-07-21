using MathStuff.vectors;

namespace SomeChartsUi.data; 

public interface IChart2DManagedData<T> {
    public void GetValues(int2 start, int2 count, int downsample, T[] dest);
    public T GetValue(int2 i);
    public int2 GetLength();
    public int GetHashCode();
}

public interface IChart2DData<T> : IChart2DManagedData<T> where T : unmanaged {
    public unsafe void GetValues(int2 start, int2 count, int downsample, T* dest);
}

public class ArrayChart2DManagedData<T> : IChart2DManagedData<T> {
    public readonly T[] data;
    public readonly int2 length;

    public ArrayChart2DManagedData(T[] data, int2 length) {
        this.data = data;
        this.length = length;
    }

    public void GetValues(int2 start, int2 count, int downsample, T[] dest) {
        for (int x = 0; x < count.x; x++) {
            int xDestInd = x * count.y;
            int xSrcInd = (start.x + (x << downsample)) * count.y;
            
            for (int y = 0; y < count.y; y++) {
                int ySrc = start.y + (y << downsample);
                
                int indexDest = xDestInd + y;
                int indexSrc = xSrcInd + ySrc;
                dest[indexDest] = data[indexSrc];
            }
        }
    }
    public T GetValue(int2 i) => data[i.x * length.y + i.y];
    public int2 GetLength() => length;
}

public class ArrayChart2DData<T> : ArrayChart2DManagedData<T>, IChart2DData<T> where T : unmanaged {
    public ArrayChart2DData(T[] data, int2 length) : base(data, length) { }
    
    public unsafe void GetValues(int2 start, int2 count, int downsample, T* dest) {
        for (int x = 0; x < count.x; x++) {
            int xSrcInd = (start.x + (x << downsample)) * count.y;
            
            for (int y = 0; y < count.y; y++) {
                int ySrc = start.y + (y << downsample);
                
                int indexSrc = xSrcInd + ySrc;
                *dest++ = data[indexSrc];
            }
        }
    }
}

public class CollectionChart2DManagedData<T> : IChart2DManagedData<T> {
    public readonly IEnumerable<T> data;
    public readonly int2 length;

    public CollectionChart2DManagedData(T[] data, int2 length) {
        this.data = data;
        this.length = length;
    }

    public void GetValues(int2 start, int2 count, int downsample, T[] dest) {
        for (int x = 0; x < count.x; x++) {
            int xDestInd = x * count.y;
            int xSrcInd = (start.x + (x << downsample)) * count.y;
            
            for (int y = 0; y < count.y; y++) {
                int ySrc = start.y + (y << downsample);
                
                int indexDest = xDestInd + y;
                int indexSrc = xSrcInd + ySrc;
                dest[indexDest] = data.ElementAt(indexSrc);
            }
        }
    }

    public T GetValue(int2 i) => data.ElementAt(i.x * length.y + i.y);
    public int2 GetLength() => length;
}

public class CollectionChart2DData<T> : CollectionChart2DManagedData<T>, IChart2DData<T> where T : unmanaged {
    public CollectionChart2DData(T[] data, int2 length) : base(data, length) { }
    
    public unsafe void GetValues(int2 start, int2 count, int downsample, T* dest) {
        for (int x = 0; x < count.x; x++) {
            int xSrcInd = (start.x + (x << downsample)) * count.y;
            
            for (int y = 0; y < count.y; y++) {
                int ySrc = start.y + (y << downsample);
                
                int indexSrc = xSrcInd + ySrc;
                *dest++ = data.ElementAt(indexSrc);
            }
        }
    }
}

public class FuncChart2DManagedData<T> : IChart2DManagedData<T> {
    public readonly Func<int2, T> sample;
    public readonly int2 length;

    public FuncChart2DManagedData(Func<int2, T> sample, int2 length) {
        this.sample = sample;
        this.length = length;
    }

    public void GetValues(int2 start, int2 count, int downsample, T[] dest) {
        for (int x = 0; x < count.x; x++) {
            int xDestInd = x * count.y;
            int xSrc= start.x + (x << downsample);
            
            for (int y = 0; y < count.y; y++) {
                int ySrc = start.y + (y << downsample);
                
                int indexDest = xDestInd + y;
                dest[indexDest] = sample(new(xSrc, ySrc));
            }
        }
    }

    public T GetValue(int2 i) => sample(i);
    public int2 GetLength() => length;
}

public class FuncChart2DData<T> : FuncChart2DManagedData<T>, IChart2DData<T> where T : unmanaged {
    public FuncChart2DData(Func<int2, T> sample, int2 length) : base(sample, length) { }

    public unsafe void GetValues(int2 start, int2 count, int downsample, T* dest) {
        for (int x = 0; x < count.x; x++) {
            int xSrc = start.x + (x << downsample);
            
            for (int y = 0; y < count.y; y++) {
                int ySrc = start.y + (y << downsample);
                *dest++ = sample(new(xSrc, ySrc));
            }
        }
    }
}


public class ConstChart2DManagedData<T> : IChart2DManagedData<T> {
    public T value;
    public int2 length;

    public ConstChart2DManagedData(T value, int2 length) {
        this.value = value;
        this.length = length;
    }
    
    public ConstChart2DManagedData(T value) {
        this.value = value;
        length = int2.one;
    }

    public void GetValues(int2 start, int2 count, int downsample, T[] dest) {
        for (int i = 0; i < count.x * count.y; i++) dest[i] = value;
    }
    public T GetValue(int2 i) => value;
    public int2 GetLength() => length;
}

public class ConstChart2DData<T> : ConstChart2DManagedData<T>, IChart2DData<T> where T : unmanaged {
    public ConstChart2DData(T value, int2 length) : base(value, length) { }
    public ConstChart2DData(T value) : base(value) { }

    public unsafe void GetValues(int2 start, int2 count, int downsample, T* dest) {
        for (int i = 0; i < count.x * count.y; i++) dest[i] = value;
    }
}