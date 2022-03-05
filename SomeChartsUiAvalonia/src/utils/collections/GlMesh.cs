using System;
using System.Collections.Generic;
using System.Numerics;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;
using SomeChartsUi.utils.vectors;
using SomeChartsUiAvalonia.controls.gl;
using static Avalonia.OpenGL.GlConsts;
namespace SomeChartsUiAvalonia.utils.collections; 

public class GlMesh : Mesh {
	public int vertexBufferObject;
	public int indexBufferObject;
	public int vertexArrayObject;

	public bool isDynamic;
	public bool updateRequired = true;

	protected unsafe void GenBuffers() {
		if (GlInfo.gl == null || GlInfo.glExt == null) return;
		
		int[] buffers = new int[2];
		GlInfo.gl.GenBuffers(2, buffers);
		vertexBufferObject = buffers[0];
		indexBufferObject = buffers[1];
		GlInfo.glExt.GenVertexArrays(1, buffers);
		vertexArrayObject = buffers[0];

		BindBuffers();
		
		int vertexSize = sizeof(Vertex);
		const int posLoc = 0;
		const int normalLoc = 1;
		const int uvLoc = 2;
		const int colLoc = 3;
		
		GlInfo.gl.VertexAttribPointer(posLoc, 3, GL_FLOAT, 0, vertexSize, IntPtr.Zero);
		GlInfo.gl.VertexAttribPointer(normalLoc, 3, GL_FLOAT, 0, vertexSize, (IntPtr) (3 * sizeof(float)));
		GlInfo.gl.VertexAttribPointer(uvLoc, 2, GL_FLOAT, 0, vertexSize, (IntPtr) ((3 + 3) * sizeof(float)));
		GlInfo.gl.VertexAttribPointer(colLoc, 4, GL_FLOAT, 0, vertexSize, (IntPtr) ((3 + 3 + 2) * sizeof(float)));
		GlInfo.gl.EnableVertexAttribArray(posLoc);
		GlInfo.gl.EnableVertexAttribArray(normalLoc);
		GlInfo.gl.EnableVertexAttribArray(uvLoc);
		GlInfo.gl.EnableVertexAttribArray(colLoc);
	}

	protected void BindBuffers() {
		GlInfo.gl!.BindBuffer(GL_ARRAY_BUFFER, vertexBufferObject);
		GlInfo.gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferObject);
		GlInfo.glExt!.BindVertexArray(vertexArrayObject);
	}

	protected unsafe void UpdateBuffers() {
		int vSize = sizeof(Vertex);
		const int iSize = sizeof(ushort);
		
		// vertices
		bool sizeChanged = isDynamic ? vertices.GetCapacityChange() : vertices.GetCountChange();
		GlInfo.gl!.BindBuffer(GL_ARRAY_BUFFER, vertexBufferObject);
		if (sizeChanged) {
			int c = isDynamic ? vertices.capacity : vertices.count;
			GlInfo.gl.BufferData(GL_ARRAY_BUFFER, (IntPtr)(c * vSize), (IntPtr)vertices.dataPtr, GL_DYNAMIC_DRAW);
			vertices.ResetChanges();
		}
		else {
			List<Range> changes = vertices.GetChanges();
			foreach (Range r in changes)
				GlInfo.glExt!.BufferSubData(GL_ARRAY_BUFFER, r.Start.Value * vSize, (r.End.Value - r.Start.Value) * vSize, vertices.dataPtr);
		}
		
		// indexes
		sizeChanged = isDynamic ? indexes.GetCapacityChange() : indexes.GetCountChange();
		GlInfo.gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferObject);
		if (sizeChanged) {
			int c = isDynamic ? indexes.capacity : indexes.count;
			GlInfo.gl.BufferData(GL_ELEMENT_ARRAY_BUFFER, (IntPtr)(c * iSize), (IntPtr)indexes.dataPtr, GL_DYNAMIC_DRAW);
			indexes.ResetChanges();
		}
		else {
			List<Range> changes = indexes.GetChanges();
			foreach (Range r in changes)
				GlInfo.glExt!.BufferSubData(GL_ELEMENT_ARRAY_BUFFER, r.Start.Value * iSize, (r.End.Value - r.Start.Value) * iSize, indexes.dataPtr);
		}

		updateRequired = false;
	}

	public override void OnModified() => updateRequired = true;

	public void Render(Material? material, Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection, float3 cameraPos) {
		if (vertexArrayObject == 0) GenBuffers();
		if (vertexArrayObject == 0) return;
		if (material is {shader: not GlShader}) return;
		
		if (updateRequired | isDynamic) UpdateBuffers();
		GlShader shader = material == null ? GlShaders.basic : (GlShader) material.shader;
		if (shader.shaderProgram == 0) shader.TryCompile();
		if (shader.shaderProgram == 0) return;

		GlInfo.gl!.UseProgram(shader.shaderProgram);
		
		shader.TrySetUniform("model", model);
		shader.TrySetUniform("view", view);
		shader.TrySetUniform("projection", projection);
		shader.TrySetUniform("cameraPos", cameraPos);
		shader.TrySetUniform("time", (float)DateTime.Now.TimeOfDay.TotalMilliseconds);
		if (material != null) shader.TryApplyMaterial(material);
		
		BindBuffers();
		GlInfo.gl.DrawElements(GL_TRIANGLES, indexes.count, GL_UNSIGNED_SHORT, IntPtr.Zero);
	}

	public override void Dispose() {
		if (vertexBufferObject != 0) {
			GlInfo.gl!.DeleteBuffers(2, new[] {vertexBufferObject, indexBufferObject});
			GlInfo.glExt!.DeleteVertexArrays(1, new[] {vertexArrayObject});
		}

		vertexBufferObject = vertexArrayObject = indexBufferObject = 0;
		GlInfo.gl!.BindBuffer(GL_ARRAY_BUFFER, 0);
		GlInfo.gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
		
		base.Dispose();
	}
}