using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;

namespace SomeChartsUi.ui.text;

//TODO: use 2D texture array to reduce draw calls
public abstract class TextMesh : IDisposable {
	private static Material? textMaterial;
	private Transform _oldTransform;
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

		textMaterial.depthTest = false;
		textMaterial.SetProperty("u_gamma", ChartsRenderSettings.textThickness);
		textMaterial.SetProperty("textQuality", (float) ChartsRenderSettings.textQuality);

		foreach (TextMeshBatch batch in batches) {
			textMaterial.SetProperty("texture0", batch.texture);

			owner.canvas.renderer.backend.DrawMesh(batch.mesh, textMaterial, owner.transform);
		}
	}

	/// <summary>regenerate mesh, if text changed <br/><br/>single-text only</summary>
	public virtual bool UpdateTextMesh(string str, Font font, float size, color col, Transform transform) {
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
	public abstract void GenerateMesh(string str, Font font, float size, color col, Transform transform);

	protected TextMeshBatch AddOrSetBatch(Texture texture, Font font, int atlas, bool checkForOverflow = true) {
		int c = batches.Count;
		for (int i = 0; i < c; i++)
			if (batches[i].font == font && batches[i].atlasId == atlas) {
				if (checkForOverflow && batches[i].mesh.vertices.count > 60_000) continue;
				return batches[i];
			}

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