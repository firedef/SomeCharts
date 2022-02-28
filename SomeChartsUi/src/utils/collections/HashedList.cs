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

	protected int ComputeHash(int blockIndex) {
		int hash = 0;
		int iterCount = math.min(blockSize, count - blockIndex * blockSize);

		int startIndex = blockIndex * blockSize;
		for (int i = 0; i < iterCount; i++) {
			int elementHash = this[i + startIndex].GetHashCode();
			hash = HashCode.Combine(hash, elementHash, i + startIndex);
		}

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