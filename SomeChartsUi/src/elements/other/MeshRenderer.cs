using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils.mesh;

namespace SomeChartsUi.elements.other; 

public class MeshRenderer : RenderableBase {
	public string path;

	public MeshRenderer(ChartsCanvas owner, string path) : base(owner) => this.path = path;
	public MeshRenderer(ChartsCanvas owner, Mesh m) : base(owner) {
		this.path = "";
		this.mesh = m;
	}

	public override void GenerateMesh() {
		if (!File.Exists(path)) throw new FileNotFoundException($"file '{path}' not found");

		string extension = Path.GetExtension(path).ToLower();
		switch (extension) {
			case ".obj":
				ObjImport.LoadMesh(mesh!, path);
				break;
			// case ".obj": GenerateMesh_Obj(); break;
			default:     throw new NotImplementedException();
		}
		
		
	}

	private void GenerateMesh_Obj() {
		string[] lines = File.ReadAllLines(path);

		(List<float3> points, List<float2> uvs, List<(int p, int uv)[]> indexes) = ParseObj(lines);
		
		int vCount = points.Count;
		Vertex[] meshVertices = new Vertex[vCount];
		List<ushort> meshIndexes = new(indexes.Count * 3);

		for (int i = 0; i < vCount; i++) {
			float3 p = points[i];
			float2 uv = uvs.Count > i ? uvs[i] : float2.zero;
			float4 col = color.white;
			float3 normal = float3.front;
			meshVertices[i] = new(p, normal, uv, col);
		}

		foreach ((int p, int uv)[] index in indexes) {
			int vertexCount = index.Length;

			switch (vertexCount) {
				case 3:
					meshIndexes.Add(ConvertIndex_(index[0].p));
					meshIndexes.Add(ConvertIndex_(index[1].p));
					meshIndexes.Add(ConvertIndex_(index[2].p));
					break;
				case 4:
					meshIndexes.Add(ConvertIndex_(index[0].p));
					meshIndexes.Add(ConvertIndex_(index[1].p));
					meshIndexes.Add(ConvertIndex_(index[2].p));
					
					meshIndexes.Add(ConvertIndex_(index[0].p));
					meshIndexes.Add(ConvertIndex_(index[2].p));
					meshIndexes.Add(ConvertIndex_(index[3].p));
					break;
			}
		}
		
		mesh!.SetVertices(meshVertices);
		mesh.SetIndexes(meshIndexes.ToArray());
		mesh.RecalculateNormals();

		ushort ConvertIndex_(int i) {
			if (i < 0) return (ushort)(meshVertices.Length - i);
			return (ushort)(i - 1);
		}
	}

	private (List<float3> points, List<float2> uvs, List<(int p, int uv)[]> indexes) ParseObj(string[] lines) {
		List<float3> points = new();
		List<float2> uvs = new();
		List<(int p, int uv)[]> indexes = new();
		
		foreach (string s in lines) {
			string[] parts = s.ToLower().Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 0 || parts[0][0] == '#') continue;// skip empty lines and comments

			switch (parts[0]) {
				case "v":// point
					points.Add(Parse_(parts.Skip(1)));
					break;
				case "vt":// uv
					uvs.Add(Parse_(parts.Skip(1)));
					break;
				case "f":// face
					indexes.Add(ParseIndexes_(parts.Skip(1)));
					break;
			}
		}

		return (points, uvs, indexes);

		static float3 Parse_(IEnumerable<string> parts) {
			float[] values = parts.Select(float.Parse).ToArray();
			float3 v = new();
			if (values.Length > 0) v.x = values[0];
			if (values.Length > 1) v.y = values[1];
			if (values.Length > 2) v.z = values[2];

			return v;
		}
		
		static (int p, int uv)[] ParseIndexes_(IEnumerable<string> parts) {
			List<(int p, int uv)> indexes = new();

			foreach (string s in parts) {
				string[] index = s.Split('/');
				int p = index.Length > 0 ? int.Parse(index[0]) : -1;
				int uv = index.Length > 1 ? int.Parse(index[1]) : -1;
				indexes.Add((p,uv));
			}

			return indexes.ToArray();
		}
	}
}