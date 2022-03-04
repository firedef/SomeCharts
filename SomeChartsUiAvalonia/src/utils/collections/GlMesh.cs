using System;
using System.Collections.Generic;
using System.Numerics;
using Avalonia.OpenGL;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;
using SomeChartsUi.utils.vectors;
using SomeChartsUiAvalonia.controls.gl;
using static Avalonia.OpenGL.GlConsts;
namespace SomeChartsUiAvalonia.utils.collections; 

public class GlMesh : Mesh {
	public static bool wireframeMode = false;
	
	public static GlInterface? gl;
	public static GlExtrasInterface? glExtras;

	//public static Dictionary<Mesh, GlMesh> objects = new();

	//public readonly Mesh mesh;
	public int vertexBufferObject = 0;
	public int indexBufferObject = 0;
	public int vertexArrayObject = 0;

	public bool isDynamic;
	public bool updateRequired = true;

	protected unsafe void GenBuffers() {
		if (gl == null || glExtras == null) return;
		
		int[] buffers = new int[2];
		gl.GenBuffers(2, buffers);
		vertexBufferObject = buffers[0];
		indexBufferObject = buffers[1];
		glExtras.GenVertexArrays(1, buffers);
		vertexArrayObject = buffers[0];

		BindBuffers();
		
		int vertexSize = sizeof(Vertex);
		const int posLoc = 0;
		const int normalLoc = 1;
		const int uvLoc = 2;
		const int colLoc = 3;
		
		gl.VertexAttribPointer(posLoc, 3, GL_FLOAT, 0, vertexSize, IntPtr.Zero);
		gl.VertexAttribPointer(normalLoc, 3, GL_FLOAT, 0, vertexSize, (IntPtr) (3 * sizeof(float)));
		gl.VertexAttribPointer(uvLoc, 2, GL_FLOAT, 0, vertexSize, (IntPtr) ((3 + 3) * sizeof(float)));
		gl.VertexAttribPointer(colLoc, 4, GL_FLOAT, 0, vertexSize, (IntPtr) ((3 + 3 + 2) * sizeof(float)));
		gl.EnableVertexAttribArray(posLoc);
		gl.EnableVertexAttribArray(normalLoc);
		gl.EnableVertexAttribArray(uvLoc);
		gl.EnableVertexAttribArray(colLoc);
	}

	protected void BindBuffers() {
		gl.BindBuffer(GL_ARRAY_BUFFER, vertexBufferObject);
		gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferObject);
		glExtras.BindVertexArray(vertexArrayObject);
	}

	protected unsafe void UpdateBuffers() {
		int vSize = sizeof(Vertex);
		const int iSize = sizeof(ushort);
		
		// vertices
		bool sizeChanged = isDynamic ? vertices.GetCapacityChange() : vertices.GetCountChange();
		gl.BindBuffer(GL_ARRAY_BUFFER, vertexBufferObject);
		if (sizeChanged) {
			int c = isDynamic ? vertices.capacity : vertices.count;
			gl.BufferData(GL_ARRAY_BUFFER, (IntPtr)(c * vSize), (IntPtr)vertices.dataPtr, GL_DYNAMIC_DRAW);
			vertices.ResetChanges();
		}
		else {
			List<Range> changes = vertices.GetChanges();
			foreach (Range r in changes)
				glExtras.BufferSubData(GL_ARRAY_BUFFER, r.Start.Value * vSize, (r.End.Value - r.Start.Value) * vSize, vertices.dataPtr);
		}
		
		// indexes
		sizeChanged = isDynamic ? indexes.GetCapacityChange() : indexes.GetCountChange();
		gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferObject);
		if (sizeChanged) {
			int c = isDynamic ? indexes.capacity : indexes.count;
			gl.BufferData(GL_ELEMENT_ARRAY_BUFFER, (IntPtr)(c * iSize), (IntPtr)indexes.dataPtr, GL_DYNAMIC_DRAW);
			indexes.ResetChanges();
		}
		else {
			List<Range> changes = indexes.GetChanges();
			foreach (Range r in changes)
				glExtras.BufferSubData(GL_ELEMENT_ARRAY_BUFFER, r.Start.Value * iSize, (r.End.Value - r.Start.Value) * iSize, indexes.dataPtr);
		}

		updateRequired = false;
	}

	public override void OnModified() => updateRequired = true;

	public unsafe void Render(Material? material, Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection, float3 cameraPos) {
		if (vertexArrayObject == 0) GenBuffers();
		if (vertexArrayObject == 0) return;
		if (material is {shader: not GlShader}) return;
		
		if (updateRequired | isDynamic) UpdateBuffers();
		GlShader shader = material == null ? GlShaders.basic : (GlShader) material.shader;
		if (shader.shaderProgram == 0) shader.TryCompile();
		if (shader.shaderProgram == 0) return;

		gl!.UseProgram(shader.shaderProgram);
		
		shader.TrySetUniform("model", model);
		shader.TrySetUniform("view", view);
		shader.TrySetUniform("projection", projection);
		shader.TrySetUniform("cameraPos", cameraPos);
		shader.TrySetUniform("time", (float)DateTime.Now.TimeOfDay.TotalMilliseconds);
		if (material != null) shader.TryApplyMaterial(material);
		
		BindBuffers();
		gl.DrawElements(wireframeMode ? GL_LINES : GL_TRIANGLES, indexes.count, GL_UNSIGNED_SHORT, IntPtr.Zero);
	}

	public override void Dispose() {
		if (vertexBufferObject != 0) {
			gl.DeleteBuffers(2, new[] {vertexBufferObject, indexBufferObject});
			glExtras.DeleteVertexArrays(1, new[] {vertexArrayObject});
		}

		vertexBufferObject = vertexArrayObject = indexBufferObject = 0;
		gl.BindBuffer(GL_ARRAY_BUFFER, 0);
		gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
		
		base.Dispose();
	}

	//public static GlMesh Get(Mesh mesh) => objects.TryGetValue(mesh, out GlMesh? existing) ? existing : new(mesh);

	// public static void Remove(Mesh mesh) {
	// 	objects.Remove(mesh);
	// }
}