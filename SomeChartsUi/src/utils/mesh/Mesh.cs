using SomeChartsUi.utils.collections;

namespace SomeChartsUi.utils.mesh; 

public class Mesh : IDisposable {
	public readonly HashedList<Vertex> vertices;
	public readonly HashedList<ushort> indexes;

	public Mesh() : this(new HashedList<Vertex>(16), new(16)) {
		
	}

	public Mesh(HashedList<Vertex> vertices, HashedList<ushort> indexes) {
		this.vertices = vertices;
		this.indexes = indexes;
	}

	/// <summary>copies data from vertices and indexes to native list</summary>
	public unsafe Mesh(Vertex* vertices, ushort* indexes, int vCount, int iCount) : this(
		new HashedList<Vertex>(vertices, vCount), 
		new(indexes, iCount)) {}

	/// <summary>copies data from vertices and indexes to native list</summary>
	public unsafe Mesh(Vertex[] vertices, ushort[] indexes) {
		fixed(Vertex* vPtr = vertices)
			this.vertices = new(vPtr, vertices.Length);
		fixed(ushort* iPtr = indexes)
			this.indexes = new(iPtr, indexes.Length);
	}

	public unsafe void SetVertices(Vertex* v, int s) => vertices.CopyFrom(v, s);
	public unsafe void SetVertices(Vertex[] v) { fixed(Vertex* vPtr = v) SetVertices(vPtr, v.Length); }

	public unsafe void SetIndexes(ushort* v, int s) => indexes.CopyFrom(v, s);
	public unsafe void SetIndexes(ushort[] v) { fixed(ushort* vPtr = v) SetIndexes(vPtr, v.Length); }

	private void Dispose(bool disposing) {
		if (!disposing) return;
		vertices.Dispose();
		indexes.Dispose();
	}
	public virtual void Dispose() {
		Dispose(true);
		GC.SuppressFinalize(this);
	}
	~Mesh() => Dispose(false);
}