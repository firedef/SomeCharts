using System;
using System.Numerics;
using Avalonia.OpenGL;
using MathStuff.vectors;
using SomeChartsUi.ui;
using SomeChartsUi.ui.canvas;
using SomeChartsUiAvalonia.controls.gl;
using SomeChartsUiAvalonia.utils.collections;

namespace SomeChartsUiAvalonia.utils; 

public class GlPostProcessor : PostProcessor {
	public GlPostProcessor(ChartsCanvas canvas) : base(canvas) { }
	
	public override void Draw() {
		if (material == null) return;
		// canvas texture already bound

		Matrix4x4 projection = Matrix4x4.CreateOrthographic(owner.transform.screenBounds.width, owner.transform.screenBounds.height, .001f, 10000);
		Matrix4x4 view = Matrix4x4.CreateLookAt(new(owner.transform.screenBounds.width * .5f, owner.transform.screenBounds.height * .5f, 10), new(owner.transform.screenBounds.width * .5f, owner.transform.screenBounds.height * .5f, 0), new(0, 1, 0));
		Matrix4x4 model = Matrix4x4.CreateScale(1, 1, 1);
		Matrix4x4 mvp = model * view * projection;
		//Matrix4x4 mvp = projection * view * model;
		
		material.SetProperty("u_texelStep", 1f / owner.transform.screenBounds.widthHeight);
		
		GlMesh glMesh = (GlMesh) mesh;
		UpdateVertices();
		glMesh.Render(material, mvp, float3.zero);
		//owner.renderer.backend.DrawMesh(glMesh, material, new(0));
	}
}