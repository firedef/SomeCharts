using System.Collections;
using System.Runtime.InteropServices;
using MathStuff;

namespace SomeChartsUi.utils.collections;

public unsafe class NativeList<T> : IList<T>, IDisposable where T : unmanaged {

#region enumerator

	public class Enumerator : IEnumerator<T> {
		public readonly NativeList<T> owner;
		public int position = -1;

		public Enumerator(NativeList<T> owner) => this.owner = owner;

		public bool MoveNext() => ++position < owner.count;
		public void Reset() => position = -1;
		object IEnumerator.Current => Current;

		public T Current => owner[position];
		public void Dispose() { }
	}

#endregion enumerator
#region fields

	public T* dataPtr;
	public int capacity;
	public int count;

	public int freeSpace => capacity - count;
	public bool isAllocated => dataPtr != null;

#endregion fields

#region ctors

	public NativeList() : this(16) { }

	/// <summary>data from ptr will been copied to new allocation</summary>
	public NativeList(T* ptr, int size) : this(size) {
		count = size;
		MemCpy(ptr, dataPtr, size * sizeof(T));
	}

	public NativeList(int size) {
		Reallocate(size);
	}

	public void EnsureFreeSpace(int s) {
		if (freeSpace >= s) return;
		Expand(s);
	}

	public void EnsureCapacity(int s) {
		if (capacity >= s) return;
		Reallocate(s);
	}

#endregion ctors

#region methods

	public void Expand(int capacityMinAdd = 0) => Reallocate(capacity + math.max(capacityMinAdd, capacity));

	protected void Reallocate(int newCapacity) {
		if (isAllocated) dataPtr = (T*)Realloc(dataPtr, newCapacity * sizeof(T));
		else dataPtr = (T*)Alloc(newCapacity * sizeof(T));

		capacity = newCapacity;
	}

	public void CopyFrom(T* src, int c) {
		EnsureCapacity(c);
		count = c;
		MemCpy(src, dataPtr, c * sizeof(T));
	}

	public void CopyTo(T* dest, int c) => MemCpy(dataPtr, dest, math.min(c, count) * sizeof(T));
	public void CopyTo(T* dest) => MemCpy(dataPtr, dest, count * sizeof(T));

	protected static void* Alloc(int s) => NativeMemory.Alloc((nuint)s);
	protected static void Free(void* p) => NativeMemory.Free(p);
	protected static void* Realloc(void* p, int s) => NativeMemory.Realloc(p, (nuint)s);
	protected static void MemCpy(void* src, void* dest, int c) => Buffer.MemoryCopy(src, dest, c, c);

	private void ReleaseUnmanagedResources() {
		if (!isAllocated) return;
		Free(dataPtr);
		dataPtr = null;
	}
	public void Dispose() {
		ReleaseUnmanagedResources();
		GC.SuppressFinalize(this);
	}
	~NativeList() {
		ReleaseUnmanagedResources();
	}

#endregion methods

#region ilist

	public IEnumerator<T> GetEnumerator() => new Enumerator(this);
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	public void Add(T item) {
		EnsureFreeSpace(1);
		dataPtr[count++] = item;
	}
	public void Clear() => count = 0;
	public bool Contains(T item) => IndexOf(item) != -1;
	public void CopyTo(T[] array, int arrayIndex) {
		int copyBytes = math.min(array.Length - arrayIndex, count) * sizeof(T);
		fixed(T* ptr1 = array) {
			Buffer.MemoryCopy(dataPtr, ptr1 + arrayIndex, copyBytes, copyBytes);
		}
	}
	public bool Remove(T item) {
		int ind = IndexOf(item);
		if (ind == -1) return false;
		RemoveAt(ind);
		return true;
	}
	public int Count => count;
	public bool IsReadOnly => false;
	public int IndexOf(T item) {
		for (int i = 0; i < count; i++)
			if (Equals(dataPtr[i], item))
				return i;
		return -1;
	}
	public void Insert(int index, T item) {
		if (index > count || index < 0) throw new IndexOutOfRangeException();
		EnsureFreeSpace(1);
		if (index == count) {
			Add(item);
			return;
		}
		MemCpy(dataPtr + index, dataPtr + index + 1, (count - index) * sizeof(T));
		count++;
		dataPtr[index] = item;
	}
	public void RemoveAt(int index) {
		if (index >= count || index < 0) throw new IndexOutOfRangeException();
		if (index == count - 1) {
			count--;
			return;
		}
		MemCpy(dataPtr + index + 1, dataPtr + index, (count - index - 1) * sizeof(T));
		count--;
	}

	public T this[int index] {
		get => dataPtr[index];
		set => dataPtr[index] = value;
	}

#endregion ilist
}