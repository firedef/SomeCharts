using System;
using System.Numerics;
using Avalonia.OpenGL;
using SomeChartsUi.backends;
using SomeChartsUi.themes.colors;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.rects;
using SomeChartsUi.utils.shaders;
using SomeChartsUi.utils.vectors;
using SomeChartsUiAvalonia.utils;
using SomeChartsUiAvalonia.utils.collections;

namespace SomeChartsUiAvalonia.backends; 

public class GlChartsBackend : ChartsBackendBase {
	public static GlInterface gl;
	// public override unsafe void DrawMesh(float2* points, float2* uvs, color* colors, ushort* indexes, int vertexCount, int indexCount, RenderableTransform transform) {
	// 	throw new System.NotImplementedException();
	// }
	// public override void DrawMesh(float2[] points, float2[]? uvs, color[]? colors, ushort[] indexes, RenderableTransform transform) {
	// 	throw new System.NotImplementedException();
	// }
	// public override void DrawText(string text, color col, FontData font, RenderableTransform transform) {
	// 	throw new System.NotImplementedException();
	// }
	// public override void DrawRect(rect rectangle, color color) {
	// 	throw new System.NotImplementedException();
	// }
	
	public override void DrawText(string text, color col, FontData font, RenderableTransform transform) {
		throw new System.NotImplementedException();
	}
	public override void ClearScreen(color col) {
		gl.ClearColor(col.rF, col.gF, col.bF, col.aF);
		gl.Clear(GlConsts.GL_COLOR_BUFFER_BIT | GlConsts.GL_DEPTH_BUFFER_BIT);
	}

	public override void DrawMesh(Mesh mesh, Shader? shader, RenderableTransform transform) {
		if (mesh is not GlMesh obj) throw new NotImplementedException("opengl backend support only GlMesh mesh type; use CreateMesh() in ChartsBackendBase");
		
		GlMesh.gl = gl;

		float z = 1 / owner.transform.zoom.animatedValue.x;
		Matrix4x4 projection = Matrix4x4.CreateOrthographic(owner.transform.screenBounds.width*z, owner.transform.screenBounds.height*z, .001f, 10000);
		// Matrix4x4 projection = Matrix4x4.CreatePerspectiveFieldOfView(100 / 180f * MathF.PI, (owner.transform.screenBounds.width / owner.transform.screenBounds.height), .001f, 10000f);


		float3 camPos = owner.transform.position.animatedValue;
		//Console.WriteLine(camPos);
		Matrix4x4 view = Matrix4x4.CreateLookAt(new(camPos.x, -camPos.y, z), new(camPos.x,-camPos.y,0), new(0, 1, 0));

		float3 p = transform.position;
		Matrix4x4 model = Matrix4x4.CreateTranslation(new(p.x, -p.y, p.z));

		obj.Render(shader, model, view, projection);
	}

	public override Mesh CreateMesh() => new GlMesh();
}