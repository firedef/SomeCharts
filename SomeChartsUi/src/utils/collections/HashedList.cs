namespace SomeChartsUi.utils.collections; 

public class HashedList<T> : NativeList<T> where T : unmanaged {
	public const int blockSize = 16;
	
	protected int[] oldHashes = null!;
	protected int oldCount;
	protected int oldCapacity;

	public HashedList() : base() {
		ResetChanges();
	}
	public HashedList(int c) : base(c) {
		ResetChanges();
	}

	/// <summary>data from ptr will been copied to new allocation</summary>
	public unsafe HashedList(T* ptr, int c) : base(ptr, c) {
		ResetChanges();
	}

	protected unsafe int ComputeHash(int blockIndex) {
		int iterCount = math.min(blockSize, count - blockIndex * blockSize);
		byte* start = (byte*) (dataPtr + blockIndex * blockSize);

		return (int)HashBytes(start, iterCount * sizeof(T), (uint) blockIndex);
	}

	private static uint Murmur32Scramble(uint k) {
		k *= 0xcc9e2d51;
		k = (k << 15) | (k >> 17);
		k *= 0x1b873593;
		return k;
	}

	/// <summary>murmur 32 hash</summary>
	protected static unsafe uint HashBytes(byte* start, int c, uint hash = 0) {
		uint* hashEnd = (uint*) (start + (c & -4));
		for (uint* s = (uint*) start; s < hashEnd; s++) {
			hash ^= Murmur32Scramble(*s);
			hash = (hash << 13) | (hash >> 19);
			hash = hash * 5 + 0xe6546b64;
		}
		
		uint key = 0;
		start = (byte*)hashEnd;
		byte* end = start + (c & 4);
		for (; start < end; start++) {
			key <<= 8;
			key |= *start;
		}

		hash ^= Murmur32Scramble(key);
		hash ^= (uint) c;
		hash ^= hash >> 16;
		hash *= 0x85ebca6b;
		hash ^= hash >> 13;
		hash *= 0xc2b2ae35;
		hash ^= hash >> 16;
		return hash;
	}

	public bool GetCountChange() => oldCount != count;
	public bool GetCapacityChange() => oldCapacity != capacity;

	public void ResetChanges() {
		int blockCount = GetBlockCount();
		oldCount = count;
		oldCapacity = capacity;
		
		oldHashes = new int[blockCount];
		for (int i = 0; i < blockCount; i++)
			oldHashes[i] = ComputeHash(i);
	}
	
	protected int GetBlockCount() => (int) MathF.Ceiling(capacity / (float) blockSize);
	protected int GetCurBlockCount() => (int) MathF.Ceiling(count / (float) blockSize);

	public List<Range> GetChanges(bool reset = true) {
		int blockCount = GetBlockCount();
		if (oldHashes.Length != blockCount) {
			if (reset) ResetChanges();
			return new() {..count};
		}

		int curBlockCount = GetCurBlockCount();
		List<Range> changes = new();

		for (int i = 0; i < curBlockCount; i++) {
			int newHash = ComputeHash(i);
			if (newHash == oldHashes[i]) continue;

			if (reset) oldHashes[i] = newHash;
			
			Range r = (i * blockSize)..((i + 1) * blockSize);
			if (changes.Count > 0 && changes[^1].End.Value == r.Start.Value) {
				changes[^1] = changes[^1].Start..r.End;
				continue;
			}
			
			changes.Add(r);
		}

		if (reset) {
			oldCount = count;
			oldCapacity = capacity;
		}

		return changes;
	}
}