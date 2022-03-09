using System;
using System.Numerics;
using Avalonia.OpenGL;
using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.backends;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;
using SomeChartsUiAvalonia.controls.gl;
using SomeChartsUiAvalonia.utils.collections;

namespace SomeChartsUiAvalonia.backends;

public class GlChartsBackend : ChartsBackendBase {
	public static bool perspectiveMode = false;

	private Font? testFont;

	public override void ClearScreen(color col) {
		GlInfo.gl!.ClearColor(col.rF, col.gF, col.bF, col.aF);
		GlInfo.gl.Clear(GlConsts.GL_COLOR_BUFFER_BIT | GlConsts.GL_DEPTH_BUFFER_BIT);
	}

	public override void DrawMesh(Mesh mesh, Material? material, RenderableTransform transform) {
		if (mesh is not GlMesh obj) throw new NotImplementedException("opengl backend support only GlMesh mesh type; use CreateMesh() in ChartsBackendBase");

		float z = 1 / owner.transform.zoom.animatedValue.x;
		Matrix4x4 projection = Matrix4x4.CreateOrthographic(owner.transform.screenBounds.width * z, owner.transform.screenBounds.height * z, .001f, 10000);
		//TODO: fix perspective
		if (perspectiveMode) {
			projection = Matrix4x4.CreatePerspectiveFieldOfView(90 / 180f * MathF.PI, owner.transform.screenBounds.width / owner.transform.screenBounds.height, .001f, 10000f);
			z *= 100;
		}


		float3 camPos = new(owner.transform.position.animatedValue, z);
		Matrix4x4 view = Matrix4x4.CreateLookAt(new(-camPos.x, camPos.y, z), new(-camPos.x, camPos.y, 0), new(0, -1, 0));

		float3 p = transform.position;
		Matrix4x4 model = Matrix4x4.CreateFromYawPitchRoll(transform.rotation.x, transform.rotation.y, transform.rotation.z) * Matrix4x4.CreateScale(new Vector3(-transform.scale.x, transform.scale.y, transform.scale.z)) * Matrix4x4.CreateTranslation(new(-p.x, -p.y, p.z));

		obj.Render(material, model, view, projection, camPos);
	}
}