using System.Runtime.InteropServices;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.utils.mesh;

/// <summary>mesh vertex <br/>the size is (3+2+4)*4 = 36 bytes</summary>
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Vertex {
	public float3 position;
	public float2 uv;
	public float4 color;

	public Vertex(float3 position, float2 uv, float4 color) {
		this.position = position;
		this.uv = uv;
		this.color = color;
	}
}