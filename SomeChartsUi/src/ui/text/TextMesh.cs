using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;

namespace SomeChartsUi.ui.text;

//TODO: use 2D texture array to reduce draw calls
public class TextMesh : IDisposable {
	private static Material? textMaterial;
	private RenderableTransform _oldTransform;
	private color _textColor;
	private Font? _textFont;
	private int _textHash = -1;
	private float _textSize;

	/// <summary>each mesh and material for different texture</summary>
	public List<TextMeshBatch> batches = new();
	public RenderableBase owner;

	public TextMesh(RenderableBase owner) {
		this.owner = owner;
		textMaterial ??= owner.canvas.factory.CreateTextMaterial();
	}

	public void Dispose() {
		ReleaseUnmanagedResources();
		GC.SuppressFinalize(this);
	}

	public void Draw() {
		if (textMaterial == null) return;

		textMaterial.SetProperty("u_gamma", ChartsRenderSettings.textThickness);

		foreach (TextMeshBatch batch in batches) {
			textMaterial.SetProperty("testure0", batch.texture);

			owner.canvas.renderer.backend.DrawMesh(batch.mesh, textMaterial, owner.transform);
		}
	}

	/// <summary>regenerate mesh, if text changed <br/><br/>single-text only</summary>
	public bool UpdateTextMesh(string str, Font font, float size, color col, RenderableTransform transform) {
		int newHash = str.GetHashCode();

		float sizeDiff = math.abs(_textSize - size);
		if (newHash != _textHash || sizeDiff > .1f || _textFont != font || _oldTransform != transform) {
			ClearMeshes();
			GenerateMesh(str, font, size, col, transform);
			_textHash = newHash;
			_textSize = size;
			_textFont = font;
			_oldTransform = transform;
			return true;
		}

		int rDiff = math.abs(_textColor.r - col.r);
		int gDiff = math.abs(_textColor.g - col.g);
		int bDiff = math.abs(_textColor.b - col.b);
		int aDiff = math.abs(_textColor.a - col.a);
		if (rDiff + gDiff + bDiff + aDiff > 4) {
			float4 fCol = col;
			foreach (TextMeshBatch batch in batches) {
				int vCount = batch.mesh.vertices.count;

				for (int i = 0; i < vCount; i += 4) {
					Vertex vert = batch.mesh.vertices[i];
					vert.color = fCol;
					batch.mesh.vertices[i] = vert;

					vert = batch.mesh.vertices[i + 1];
					vert.color = fCol;
					batch.mesh.vertices[i + 1] = vert;

					vert = batch.mesh.vertices[i + 2];
					vert.color = fCol;
					batch.mesh.vertices[i + 2] = vert;

					vert = batch.mesh.vertices[i + 3];
					vert.color = fCol;
					batch.mesh.vertices[i + 3] = vert;
				}
			}

			_textColor = col;
			return true;
		}

		return false;
	}

	public void ClearMeshes() {
		foreach (TextMeshBatch batch in batches) batch.mesh.Clear();
	}

	//TODO: add ligatures
	//TODO: add directions, line breaks
	//TODO: fix mesh overlap (hot-fixed by changing zPos)
	public void GenerateMesh(string str, Font font, float size, color col, RenderableTransform transform) {
		size /= font.textures.resolution;
		size *= transform.scale.x;

		float3 normal = float3.front;
		float xPos = 0;
		float zPos = 0;
		foreach (char c in str) {
			(FontCharData charData, int atlas) = font.textures.GetGlyph(c.ToString());
			if (atlas == -1) continue;// glyph is invincible

			float yPos = -charData.baseline * size;

			TextMeshBatch batch = AddOrSetBatch(font.textures.atlases[atlas].texture, font, atlas);
			float invCanvasSize = 1 / batch.texture.size.x;

			float2 posMin = new float2(xPos, yPos) + transform.position.xy;
			float2 posMax = posMin + charData.size * size;
			float2 uvMin = charData.position * invCanvasSize;
			float2 uvMax = uvMin + charData.size * invCanvasSize;

			int vCount = batch.mesh.vertices.count;
			batch.mesh.vertices.Add(new(new(posMin.x, posMin.y, zPos), normal, new(uvMin.x, uvMax.y), col));// pos, normal, uv, col
			batch.mesh.vertices.Add(new(new(posMin.x, posMax.y, zPos), normal, new(uvMin.x, uvMin.y), col));// pos, normal, uv, col
			batch.mesh.vertices.Add(new(new(posMax.x, posMax.y, zPos), normal, new(uvMax.x, uvMin.y), col));// pos, normal, uv, col
			batch.mesh.vertices.Add(new(new(posMax.x, posMin.y, zPos), normal, new(uvMax.x, uvMax.y), col));// pos, normal, uv, col

			batch.mesh.indexes.Add((ushort)(vCount + 0));
			batch.mesh.indexes.Add((ushort)(vCount + 1));
			batch.mesh.indexes.Add((ushort)(vCount + 2));
			batch.mesh.indexes.Add((ushort)(vCount + 0));
			batch.mesh.indexes.Add((ushort)(vCount + 2));
			batch.mesh.indexes.Add((ushort)(vCount + 3));

			xPos += charData.advance * size;
			zPos += .01f;
		}
	}

	private TextMeshBatch AddOrSetBatch(Texture texture, Font font, int atlas) {
		int c = batches.Count;
		for (int i = 0; i < c; i++)
			if (batches[i].font == font && batches[i].atlasId == atlas)
				return batches[i];

		TextMeshBatch batch = new(owner.canvas.factory.CreateMesh(), texture, font, atlas);
		batches.Add(batch);
		return batch;
	}

	private void ReleaseUnmanagedResources() {
		foreach (TextMeshBatch batch in batches) batch.Dispose();
	}

	~TextMesh() => ReleaseUnmanagedResources();
}

public readonly struct TextMeshBatch : IDisposable {
	public readonly Mesh mesh;
	public readonly Texture texture;
	public readonly Font font;
	public readonly int atlasId;

	public TextMeshBatch(Mesh mesh, Texture texture, Font font, int atlasId) {
		this.mesh = mesh;
		this.texture = texture;
		this.font = font;
		this.atlasId = atlasId;
	}

	public void Dispose() => mesh.Dispose();
}