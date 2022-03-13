using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils.mesh;

namespace SomeChartsUi.ui.text; 

public class FreeTypeTextMesh : TextMesh {
	public FreeTypeTextMesh(RenderableBase owner) : base(owner) { }

	public override void GenerateMesh(string str, Font font, float size, color col, Transform transform) {
		size /= font.textures.resolution;
		size *= transform.scale.x;

		float3 normal = float3.front;
		float xPos = 0;
		float yPos = 0;
		float zPos = 0;

		Mesh? prevMesh = null;

		int strLen = str.Length;
		for (int i = 0; i < strLen; i++) {
			char c0 = str[i];
			char c1 = i == strLen - 1 ? '\0' : str[i + 1];

			if (c0 is '\n') {
				if (font.textures.lineHeight == 0) yPos -= size * font.textures.resolution;
				else yPos -= font.textures.lineHeight * size;
				xPos = 0;
				continue;
			}
			
			// simple character
			uint ch = font.textures.ToCharacter(c0, '\0');
			if (font.textures.ContainsCharacter(ch)) {
				AddGlyph(ch, font); 
				continue;
			}
			
			// fallbacks
			for (int j = 0; j < font.fallbacks.Count; j++) {
				if (!font.fallbacks[j].textures.ContainsCharacter(ch)) continue;
				AddGlyph(ch, font.fallbacks[j]);
				break;
			}
		}
		
		prevMesh?.OnModified();

		void AddGlyph(uint code, Font f) {
			(FontCharData charData, int atlas) = f.textures.GetGlyph(code);
			if (atlas == -1) return;// glyph is invincible

			float charY = yPos + -charData.baseline * size;

			TextMeshBatch batch = AddOrSetBatch(f.textures.atlases[atlas].texture, f, atlas);
			float invCanvasSize = 1 / batch.texture.size.x;

			float2 posMin = new float2(xPos, charY) + transform.position.xy;
			float2 posMax = posMin + charData.size * size;
			float2 uvMin = charData.position * invCanvasSize;
			float2 uvMax = uvMin + charData.size * invCanvasSize;

			//uvMin = 0;
			//uvMax = 1;

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

			if (prevMesh != null && prevMesh != batch.mesh) {
				prevMesh.OnModified(); 
			}
			prevMesh = batch.mesh;
		}
	}
}