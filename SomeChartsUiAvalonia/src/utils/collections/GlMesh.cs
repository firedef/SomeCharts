using System;
using System.Collections.Generic;
using System.Numerics;
using MathStuff.vectors;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;
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
		
		GlInfo.glExt!.BindVertexArray(vertexArrayObject);
		GlInfo.gl!.BindBuffer(GL_ARRAY_BUFFER, vertexBufferObject);
		GlInfo.gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferObject);

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
		GlInfo.glExt!.BindVertexArray(vertexArrayObject);
	}

	protected unsafe void UpdateBuffers() {
		int vSize = sizeof(Vertex);
		const int iSize = sizeof(ushort);
		bool d = false;
		
		// vertices
		GlInfo.gl!.BindBuffer(GL_ARRAY_BUFFER, vertexBufferObject);
		bool sizeChanged = d ? vertices.GetCapacityChange() : vertices.GetCountChange();
		if (sizeChanged) {
			int c = d ? vertices.capacity : vertices.count;
			GlInfo.gl!.BufferData(GL_ARRAY_BUFFER, (IntPtr)(c * vSize), (IntPtr)vertices.dataPtr, GL_DYNAMIC_DRAW);
			vertices.ResetChanges();
		}
		else {
			List<Range> changes = vertices.GetChanges();
			foreach (Range r in changes)
				GlInfo.glExt!.BufferSubData(GL_ARRAY_BUFFER, r.Start.Value * vSize, (r.End.Value - r.Start.Value) * vSize, vertices.dataPtr);
		}
		
		// indexes
		GlInfo.gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferObject);
		sizeChanged = d ? indexes.GetCapacityChange() : indexes.GetCountChange();
		if (sizeChanged) {
			int c = d ? indexes.capacity : indexes.count;
			GlInfo.gl!.BufferData(GL_ELEMENT_ARRAY_BUFFER, (IntPtr)(c * iSize), (IntPtr)indexes.dataPtr, GL_DYNAMIC_DRAW);
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
		if (vertices.count == 0 || indexes.count == 0) return;
		if (vertexArrayObject == 0) GenBuffers();
		if (vertexArrayObject == 0) return;
		if (material is {shader: not GlShader}) return;
		
		BindBuffers();
		if (updateRequired | isDynamic) UpdateBuffers();
		GlShader shader = material == null ? GlShaders.basic : (GlShader) material.shader;
		if (shader.shaderProgram == 0) shader.TryCompile();
		if (shader.shaderProgram == 0) return;

		GlInfo.gl!.UseProgram(shader.shaderProgram);
		
		GlInfo.CheckError($"before uniforms");
		shader.TrySetUniform("model", model);
		shader.TrySetUniform("view", view);
		shader.TrySetUniform("projection", projection);
		shader.TrySetUniform("cameraPos", cameraPos);
		shader.TrySetUniform("time", (float)DateTime.Now.TimeOfDay.TotalMilliseconds);
		if (material != null) shader.TryApplyMaterial(material);
		
		GlInfo.CheckError($"after uniforms");
		GlInfo.gl.DrawElements(GL_TRIANGLES, indexes.count, GL_UNSIGNED_SHORT, IntPtr.Zero);
		GlInfo.CheckError($"after rendering object");
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