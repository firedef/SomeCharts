using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils.collections;

namespace SomeChartsUi.utils.mesh;

public class Mesh : IDisposable {
#region fields

	public readonly HashedList<ushort> indexes;
	public readonly HashedList<Vertex> vertices;
	public rect bounds;
	public bool enabled = true;

#endregion fields

#region ctors

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

#endregion ctors

#region meshFunctions

	public unsafe void SetVertices(Vertex* v, int s) => vertices.CopyFrom(v, s);
	public unsafe void SetVertices(Vertex[] v) { fixed(Vertex* vPtr = v) SetVertices(vPtr, v.Length); }

	public unsafe void SetIndexes(ushort* v, int s) => indexes.CopyFrom(v, s);
	public unsafe void SetIndexes(ushort[] v) { fixed(ushort* vPtr = v) SetIndexes(vPtr, v.Length); }

	public void AddVertex(Vertex v) => vertices.Add(v);
	public void AddIndex(int v) => indexes.Add((ushort) v);

	public void AddRect(float2 p0, float2 p1, float2 p2, float2 p3, color c0) => AddRect(
		new(p0, float3.front, float2.zero, c0),
		new(p1, float3.front, float2.zero, c0),
		new(p2, float3.front, float2.zero, c0),
		new(p3, float3.front, float2.zero, c0)
	);
	
	public void AddRect(Vertex p0, Vertex p1, Vertex p2, Vertex p3) {
		vertices.EnsureCapacity(4);
		AddVertex(p0);
		AddVertex(p1);
		AddVertex(p2);
		AddVertex(p3);
		AddQuadIndices();
	}

	public void AddQuadIndices() {
		indexes.EnsureCapacity(6);
		int vCount = vertices.count - 4;
		AddIndex(vCount + 0);
		AddIndex(vCount + 1);
		AddIndex(vCount + 2);
		AddIndex(vCount + 0);
		AddIndex(vCount + 2);
		AddIndex(vCount + 3);
	}
	
#endregion meshFunctions

#region calculations
	
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

	public void RecalculateBounds() {
		int c = vertices.count;
		float2 min =  float2.maxValue;
		float2 max = -float2.maxValue;

		for (int i = 0; i < c; i++) {
			float2 p = vertices[i].position;
			if (p.x < min.x) min.x = p.x;
			if (p.y < min.y) min.y = p.y;
			if (p.x > max.x) max.x = p.x;
			if (p.y > max.y) max.y = p.y;
		}

		bounds = new(min.x, min.y, max.x - min.x, max.y - min.y);
	}

#endregion calculations

#region cleanup
	
	public virtual void Dispose() {
		Dispose(true);
		GC.SuppressFinalize(this);
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

#endregion cleanup
	
#region other
	
	public bool IsVisible(rect camera) => enabled && Geometry.Intersects(bounds, camera, float2.zero);
	public bool IsVisible(rect camera, Transform objTransform) => enabled && Geometry.Intersects(new(bounds.left, bounds.bottom, bounds.width * objTransform.scale.x, bounds.height * objTransform.scale.y), camera, objTransform.position);
	public virtual void OnModified() => RecalculateBounds();

#endregion other

}