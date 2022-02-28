using System;
using System.Collections.Generic;
using System.Numerics;
using Avalonia.OpenGL;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;
using SomeChartsUiAvalonia.controls.gl;
using static Avalonia.OpenGL.GlConsts;
namespace SomeChartsUiAvalonia.utils.collections; 

public class GlObject : IDisposable {
	public static GlInterface gl;
	public static GlExtrasInterface glExtras;

	public static Dictionary<Mesh, GlObject> objects = new();

	public readonly Mesh mesh;
	public int vertexBufferObject = 0;
	public int indexBufferObject = 0;
	public int vertexArrayObject = 0;

	public bool isDynamic;

	public GlObject(Mesh mesh) {
		this.mesh = mesh;
		objects.Add(mesh, this);
		
		GenBuffers();
	}

	protected unsafe void GenBuffers() {
		int[] buffers = new int[2];
		gl.GenBuffers(2, buffers);
		vertexBufferObject = buffers[0];
		indexBufferObject = buffers[1];
		vertexArrayObject = glExtras.GenVertexArray();

		BindBuffers();
		
		int vertexSize = sizeof(Vertex);
		const int posLoc = 0;
		const int uvLoc = 1;
		const int colLoc = 2;
		
		gl.VertexAttribPointer(posLoc, 3, GL_FLOAT, 0, vertexSize, IntPtr.Zero);
		gl.VertexAttribPointer(uvLoc, 2, GL_FLOAT, 0, vertexSize, (IntPtr) (3 * sizeof(float)));
		gl.VertexAttribPointer(colLoc, 4, GL_FLOAT, 0, vertexSize, (IntPtr) ((3 + 2) * sizeof(float)));
		gl.EnableVertexAttribArray(posLoc);
		gl.EnableVertexAttribArray(uvLoc);
		gl.EnableVertexAttribArray(colLoc);
	}

	protected void BindBuffers() {
		gl.BindBuffer(GL_ARRAY_BUFFER, vertexBufferObject);
		gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferObject);
		//gl.BindBuffer(GL_VERTEX_ARRAY, vertexArrayObject);
		glExtras.BindVertexArray(vertexArrayObject);
	}

	protected unsafe void UpdateBuffers() {
		int vSize = sizeof(Vertex);
		const int iSize = sizeof(ushort);
		
		// vertices
		bool sizeChanged = isDynamic ? mesh.vertices.GetCapacityChange() : mesh.vertices.GetCountChange();
		gl.BindBuffer(GL_ARRAY_BUFFER, vertexBufferObject);
		if (sizeChanged) {
			int c = isDynamic ? mesh.vertices.capacity : mesh.vertices.count;
			gl.BufferData(GL_ARRAY_BUFFER, (IntPtr)(c * vSize), (IntPtr)mesh.vertices.dataPtr, GL_DYNAMIC_DRAW);
			mesh.vertices.ResetChanges();
		}
		else {
			List<Range> changes = mesh.vertices.GetChanges();
			foreach (Range r in changes)
				glExtras.BufferSubData(GL_ARRAY_BUFFER, r.Start.Value * vSize, (r.End.Value - r.Start.Value) * vSize, mesh.vertices.dataPtr);
		}
		
		// indexes
		sizeChanged = isDynamic ? mesh.indexes.GetCapacityChange() : mesh.indexes.GetCountChange();
		gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferObject);
		if (sizeChanged) {
			int c = isDynamic ? mesh.indexes.capacity : mesh.indexes.count;
			gl.BufferData(GL_ELEMENT_ARRAY_BUFFER, (IntPtr)(c * iSize), (IntPtr)mesh.indexes.dataPtr, GL_DYNAMIC_DRAW);
			mesh.indexes.ResetChanges();
		}
		else {
			List<Range> changes = mesh.indexes.GetChanges();
			foreach (Range r in changes)
				glExtras.BufferSubData(GL_ELEMENT_ARRAY_BUFFER, r.Start.Value * iSize, (r.End.Value - r.Start.Value) * iSize, mesh.indexes.dataPtr);
		}
	}

	public unsafe void Render(Shader? shader, Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection) {
		UpdateBuffers();
		GlShaderData shaderData = shader == null ? GlShaders.basicShader : GlShaders.Get(shader);
		if (shaderData.shaderProgram == 0) shaderData.TryCompile();
		if (shaderData.shaderProgram == 0) return;

		gl.UseProgram(shaderData.shaderProgram);
		
		int modelLoc = gl.GetUniformLocationString(shaderData.shaderProgram, "model");
		int viewLoc = gl.GetUniformLocationString(shaderData.shaderProgram, "view");
		int projectionLoc = gl.GetUniformLocationString(shaderData.shaderProgram, "projection");
		
		gl.UniformMatrix4fv(modelLoc, 1, false, &model);
		gl.UniformMatrix4fv(viewLoc, 1, false, &view);
		gl.UniformMatrix4fv(projectionLoc, 1, false, &projection);
		BindBuffers();
		gl.DrawElements(GL_TRIANGLES, mesh.indexes.count, GL_UNSIGNED_SHORT, IntPtr.Zero);
	}

	public void Dispose() {
		if (vertexBufferObject != 0) gl.DeleteBuffers(1, new[] {vertexBufferObject});
		if (indexBufferObject != 0) gl.DeleteBuffers(1, new[] {indexBufferObject});
		glExtras.DeleteVertexArrays(1, new[] { vertexArrayObject });

		vertexBufferObject = 0;
		indexBufferObject = 0;
		gl.BindBuffer(GL_ARRAY_BUFFER, 0);
		gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
		
		mesh.Dispose();
	}

	public static GlObject Get(Mesh mesh) => objects.TryGetValue(mesh, out GlObject? existing) ? existing : new(mesh);

	public static void Remove(Mesh mesh) {
		objects.Remove(mesh);
	}
}