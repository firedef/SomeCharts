using MathStuff;
using MathStuff.vectors;

namespace SomeChartsUi.utils.mesh; 

public static class ObjImport {
	public static void LoadMesh(Mesh instance, string path) {
		string[] lines = File.ReadAllLines(path);

		List<float3> parsedPositions = new();
		List<float3> parsedNormals = new();
		List<float2> parsedTexcoords = new();
		List<(int p, int n, int uv)[]> parsedFaces = new();
		ParseLines(lines, parsedPositions, parsedNormals, parsedTexcoords, parsedFaces);

		Dictionary<Vertex, ushort> vertices = new();
		List<ushort> indexes = new();
		ConvertParsedFile(parsedPositions, parsedNormals, parsedTexcoords, parsedFaces, vertices, indexes);
		
		instance.SetVertices(vertices.Keys.ToArray());
		instance.SetIndexes(indexes.ToArray());
		
		instance.RecalculateNormals();
	}

	private static void ParseLines(IEnumerable<string> lines, ICollection<float3> positions, ICollection<float3> normals, ICollection<float2> texcoords, ICollection<(int p, int n, int uv)[]> faces) {
		foreach (string s in lines) {
			string[] parts = s.ToLower().Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 0 || parts[0][0] == '#') continue; // comments
			
			switch (parts[0]) {
				case "v": // vertex points
					positions.Add(ParseVector(parts.Skip(1)));
					break;
				case "vt": // texture coordinates
					texcoords.Add(ParseVector(parts.Skip(1)).xy);
					break;
				case "vn": // vertex normals
					normals.Add(ParseVector(parts.Skip(1)));
					break;
				case "f": // face
					faces.Add(ParseFace(parts.Skip(1)));
					break;
			}
		}
	}
	
	private static float3 ParseVector(IEnumerable<string> parts) {
		float[] values = parts.Select(float.Parse).ToArray();
		float3 v = new();
		if (values.Length > 0) v.x = values[0];
		if (values.Length > 1) v.y = values[1];
		if (values.Length > 2) v.z = values[2];

		return v;
	}

	private static (int p, int n, int uv)[] ParseFace(IEnumerable<string> parts) {
		List<(int p, int n, int uv)> indexes = new();

		foreach (string s in parts) {
			string[] index = s.Split('/');
			int p = index.Length > 0 ? int.Parse(index[0]) : 0;
			int uv = index.Length > 1 ? int.Parse(index[1]) : 0;
			int n = index.Length > 2 ? int.Parse(index[2]) : 0;
			indexes.Add((p, n, uv));
		}

		return indexes.ToArray();
	}

	private static void ConvertParsedFile(
		List<float3> parsedPositions,
		List<float3> parsedNormals,
		List<float2> parsedTexcoords,
		List<(int p, int n, int uv)[]> parsedFaces,
		Dictionary<Vertex, ushort> vertices,
		List<ushort> indexes) {
		int pCount = parsedPositions.Count;
		int nCount = parsedNormals.Count;
		int uvCount = parsedTexcoords.Count;
		
		float3 pos = float3.zero;
		float3 normal = float3.zero;
		float2 texcoord = float2.zero;
		Vertex vert = new();
		foreach ((int p, int n, int uv)[] face in parsedFaces) {
			switch (face.Length) {
				case 3: // triangle
					ConvertVertex(face[0], pCount, nCount, uvCount, parsedPositions, parsedNormals, parsedTexcoords, vertices, indexes);
					ConvertVertex(face[1], pCount, nCount, uvCount, parsedPositions, parsedNormals, parsedTexcoords, vertices, indexes);
					ConvertVertex(face[2], pCount, nCount, uvCount, parsedPositions, parsedNormals, parsedTexcoords, vertices, indexes);
					break;
				case 4: // quad
					ConvertVertex(face[0], pCount, nCount, uvCount, parsedPositions, parsedNormals, parsedTexcoords, vertices, indexes);
					ConvertVertex(face[1], pCount, nCount, uvCount, parsedPositions, parsedNormals, parsedTexcoords, vertices, indexes);
					ConvertVertex(face[2], pCount, nCount, uvCount, parsedPositions, parsedNormals, parsedTexcoords, vertices, indexes);
					
					ConvertVertex(face[0], pCount, nCount, uvCount, parsedPositions, parsedNormals, parsedTexcoords, vertices, indexes);
					ConvertVertex(face[2], pCount, nCount, uvCount, parsedPositions, parsedNormals, parsedTexcoords, vertices, indexes);
					ConvertVertex(face[3], pCount, nCount, uvCount, parsedPositions, parsedNormals, parsedTexcoords, vertices, indexes);
					break;
			}
		}

	}
	private static void ConvertVertex((int p, int n, int uv) face, int pCount, int nCount, int uvCount, List<float3> parsedPositions, List<float3> parsedNormals, List<float2> parsedTexcoords, Dictionary<Vertex, ushort> vertices, List<ushort> indexes) {
		(int p, int n, int uv) = face;
		float3 position = p == 0 ? float3.zero : parsedPositions[ConvertIndex(p, pCount)];
		float3 normal = n == 0 ? float3.zero : parsedNormals[ConvertIndex(n, nCount)];
		float2 texcoord = uv == 0 ? float3.zero : parsedTexcoords[ConvertIndex(uv, uvCount)];
		Vertex vertex = new(position, normal, texcoord, color.white);
		if (vertices.TryGetValue(vertex, out ushort ind))
			indexes.Add(ind);
		else {
			ushort c = (ushort)vertices.Count;
			vertices.Add(vertex, c);
			indexes.Add(c);
		}
	}

	private static ushort ConvertIndex(int v, int count) {
		if (v < 0) return (ushort)(count - v);
		return (ushort)(v - 1);
	}
}