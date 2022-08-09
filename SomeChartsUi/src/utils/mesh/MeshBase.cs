using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.ui.elements;

namespace SomeChartsUi.utils.mesh; 

public abstract class MeshBase : IDisposable {
    /// <summary>AABB using for culling <br/><br/>
    /// update using <see cref="RecalculateBounds"/><br/><br/>
    /// does not take into account object transform (rotation, translation and scale)</summary>
    public rect bounds;

    /// <summary>set to false, to disable object rendering</summary>
    public bool enabled = true;

    public abstract void Dispose();
    public abstract void AddVertex(Vertex v);
    public abstract void AddIndex(int v);

    /// <summary>add quadrilateral to mesh (vertices and indices) <br/><br/>using front normal and 0-1 uv coordinates</summary>
    public abstract void AddRect(float3 p0, float3 p1, float3 p2, float3 p3, color c0);

    /// <summary>add quadrilateral to mesh (vertices and indices) <br/><br/>using front normal</summary>
    public abstract void AddRect(float3 p0, float3 p1, float3 p2, float3 p3, color c0, rect uvs);

    /// <summary>add quadrilateral to mesh (vertices and indices)</summary>
    public abstract void AddRect(Vertex p0, Vertex p1, Vertex p2, Vertex p3);

    public abstract void AddQuadIndices();

    /// <summary>recalculate normals of mesh</summary>
    public abstract unsafe void RecalculateNormals();

    /// <summary>recalculate AABB of mesh <br/><br/>does not take into account object transform (rotation, translation and scale)</summary>
    public abstract void RecalculateBounds();

    public abstract void Clear();
    protected abstract void Dispose(bool disposing);
    public abstract bool IsVisible(rect camera);
    public abstract bool IsVisible(rect camera, Transform objTransform);

    /// <summary>mark mesh as modified <br/><br/>
    /// it will recalculate bounds and (if GlMesh) update it on gpu</summary>
    public abstract void OnModified();
}