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

namespace SomeChartsUiAvalonia.impl.opengl.backend;

public class GlChartsBackend : ChartsBackendBase {
	public static bool perspectiveMode = false;

	private Font? testFont;

	public override void ClearScreen(color col) {
		GlInfo.gl!.ClearColor(col.rF, col.gF, col.bF, col.aF);
		GlInfo.gl.Clear(GlConsts.GL_COLOR_BUFFER_BIT | GlConsts.GL_DEPTH_BUFFER_BIT);
	}

	public override void DrawMesh(Mesh mesh, Material? material, Transform transform) {
		if (mesh is not GlMesh obj) throw new NotImplementedException("opengl backend support only GlMesh mesh type; use CreateMesh() in ChartsBackendBase");

		if (!mesh.IsVisible(owner.transform.worldBounds, transform)) {
			return;
		}

		// //TODO: fix perspective
		// if (perspectiveMode) {
		// 	projection = Matrix4x4.CreatePerspectiveFieldOfView(90 / 180f * MathF.PI, owner.transform.screenBounds.width / owner.transform.screenBounds.height, .001f, 10000f);
		// 	z *= 100;
		// }

		float z = 1 / owner.transform.scale.animatedValue.x;
		float3 camPos = new(owner.transform.position.animatedValue, z);
		
		if (transform.modelMatrix.IsIdentity) transform.RecalculateMatrix();

		Matrix4x4 mvp = transform.modelMatrix * owner.transform.viewMatrix * owner.transform.projectionMatrix;
		
		obj.Render(material, mvp, camPos, owner.transform.screenBounds.widthHeight);
	}
}