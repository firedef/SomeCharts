using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.layers.render;
using SomeChartsUi.utils.collections;

namespace SomeChartsUi.elements.other;

public class RectPackVisualization : RenderableBase {
	private readonly List<(rect r, color c)> rectangles = new();
	private bool isDirty;
	public RectPack packer = new(2048, 0);

	public RectPackVisualization(ChartsCanvas owner) : base(owner) { }

	protected override void GenerateMesh() {
		mesh!.vertices.Clear();
		mesh.indexes.Clear();

		int objCount = rectangles.Count;
		int shelfCount = packer.shelfs.Count;
		mesh.vertices.EnsureCapacity((objCount * 2 + shelfCount) * 4);
		mesh.indexes.EnsureCapacity((objCount * 2 + shelfCount) * 6);
		isDirty = false;

		const float th = 1;
		const float texSize = 0.001f;

		for (int i = 0; i < objCount; i++) {
			const byte a = 50;
			float4 col = rectangles[i].c.WithAlpha(a);
			float2 x0y0 = rectangles[i].r.leftBottom;
			float2 x0y1 = rectangles[i].r.leftTop;
			float2 x1y1 = rectangles[i].r.rightTop;
			float2 x1y0 = rectangles[i].r.rightBottom;
			float3 norm = float3.front;

			// inner
			mesh.vertices.Add(new(x0y0, norm, x0y0 * texSize, col));
			mesh.vertices.Add(new(x0y1, norm, x0y1 * texSize, col));
			mesh.vertices.Add(new(x1y1, norm, x1y1 * texSize, col));
			mesh.vertices.Add(new(x1y0, norm, x1y0 * texSize, col));

			mesh.vertices.Add(new(x1y0, norm, x0y0 * texSize, color.softBlue));
			mesh.vertices.Add(new(x1y1, norm, x0y1 * texSize, color.softBlue));
			mesh.vertices.Add(new(x1y1 + new float2(th * 2, 0), norm, x1y1 * texSize, color.softBlue));
			mesh.vertices.Add(new(x1y0 + new float2(th * 2, 0), norm, x1y0 * texSize, color.softBlue));

			// inner
			int vP = i * 8;
			mesh.indexes.Add((ushort)(vP + 0));
			mesh.indexes.Add((ushort)(vP + 1));
			mesh.indexes.Add((ushort)(vP + 2));
			mesh.indexes.Add((ushort)(vP + 0));
			mesh.indexes.Add((ushort)(vP + 2));
			mesh.indexes.Add((ushort)(vP + 3));

			mesh.indexes.Add((ushort)(vP + 4));
			mesh.indexes.Add((ushort)(vP + 5));
			mesh.indexes.Add((ushort)(vP + 6));
			mesh.indexes.Add((ushort)(vP + 4));
			mesh.indexes.Add((ushort)(vP + 6));
			mesh.indexes.Add((ushort)(vP + 7));
		}

		for (int i = 0; i < shelfCount; i++) {
			const float x0 = 0;
			float x1 = packer.size.x;
			float y = packer.shelfs[i].top;
			float3 norm = float3.front;
			float4 col = color.softBlue;

			// inner
			mesh.vertices.Add(new(new(x0, y - th, .001f), norm, new float2(x0, y - th) * texSize, col));
			mesh.vertices.Add(new(new(x0, y + th, .001f), norm, new float2(x0, y + th) * texSize, col));
			mesh.vertices.Add(new(new(x1, y + th, .001f), norm, new float2(x1, y + th) * texSize, col));
			mesh.vertices.Add(new(new(x1, y - th, .001f), norm, new float2(x1, y - th) * texSize, col));

			// inner
			int vP = (i + objCount * 2) * 4;
			mesh.indexes.Add((ushort)(vP + 0));
			mesh.indexes.Add((ushort)(vP + 1));
			mesh.indexes.Add((ushort)(vP + 2));
			mesh.indexes.Add((ushort)(vP + 0));
			mesh.indexes.Add((ushort)(vP + 2));
			mesh.indexes.Add((ushort)(vP + 3));
		}

		mesh.OnModified();
	}

	protected override bool CheckMeshForUpdate() => isDirty;

	public bool AddRect(rect r, color c) {
		float2 pos = packer.Pack(r.widthHeight);
		if (pos == new float2(-1, -1)) return false;
		rectangles.Add((new(pos.x, pos.y, r.width, r.height), c));
		//rectangles.Add((r, c));
		isDirty = true;
		return true;
	}
	
	public override void Render(RenderLayerId pass) {
		if (!isTransparent && pass == RenderLayerId.opaque) DrawMesh(material);
		if (isTransparent && pass == RenderLayerId.transparent) DrawMesh(material);
	}
}