using MathStuff.vectors;
using SomeChartsUi.utils.collections;

namespace SomeChartsUi.utils.mesh;

public class Mesh : IDisposable {
	public readonly HashedList<ushort> indexes;
	public readonly HashedList<Vertex> vertices;

	public Mesh() : this(new HashedList<Vertex>(16), new(16)) { }

	public Mesh(HashedList<Vertex> vertices, HashedList<ushort> indexes) {
		this.vertices = vertices;
		this.indexes = indexes;
	}

	/// <summary>copies data from vertices and indexes to native list</summary>
	public unsafe Mesh(Vertex* vertices, ushort* indexes, int vCount, int iCount) : this(
		new HashedList<Vertex>(vertices, vCount),
		new(indexes, iCount)) { }

	/// <summary>copies data from vertices and indexes to native list</summary>
	public unsafe Mesh(Vertex[] vertices, ushort[] indexes) {
		fixed(Vertex* vPtr = vertices) {
			this.vertices = new(vPtr, vertices.Length);
		}
		fixed(ushort* iPtr = indexes) {
			this.indexes = new(iPtr, indexes.Length);
		}
	}
	public virtual void Dispose() {
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	public unsafe void SetVertices(Vertex* v, int s) {
		vertices.CopyFrom(v, s);
		OnModified();
	}
	public unsafe void SetVertices(Vertex[] v) {
		fixed(Vertex* vPtr = v) {
			SetVertices(vPtr, v.Length);
		}
	}

	public unsafe void SetIndexes(ushort* v, int s) {
		indexes.CopyFrom(v, s);
		OnModified();
	}
	public unsafe void SetIndexes(ushort[] v) {
		fixed(ushort* vPtr = v) {
			SetIndexes(vPtr, v.Length);
		}
	}

	public unsafe void RecalculateNormals() {
		int c = indexes.count;

		for (int i = 0; i < c; i += 3) {
			ushort i1 = indexes[i];
			ushort i2 = indexes[(i + 1) % c];
			ushort i3 = indexes[(i + 2) % c];

			float3 p0 = vertices[i1].position - vertices[i2].position;
			float3 p1 = vertices[i1].position - vertices[i3].position;
			float3 normal = float3.Cross(p0, p1).normalized;

			vertices.dataPtr[i1].normal = normal;
			vertices.dataPtr[i2].normal = normal;
			vertices.dataPtr[i3].normal = normal;
		}
	}

	public void Clear() {
		vertices.Clear();
		indexes.Clear();
	}

	private void Dispose(bool disposing) {
		if (!disposing) return;
		vertices.Dispose();
		indexes.Dispose();
	}
	~Mesh() => Dispose(false);

	public virtual void OnModified() { }
}