using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.OpenGL.Imaging;
using Avalonia.Threading;
using SomeChartsUi.utils;
using SomeChartsUi.utils.vectors;
using static Avalonia.OpenGL.GlConsts;

namespace SomeChartsUiAvalonia.controls.gl;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Vertex {
	public float3 pos;

	public Vertex(float3 pos) => this.pos = pos;
}

public class AvaloniaGlChartsCanvas : OpenGlControlBase {
	//public static FieldInfo bitmapField = typeof(OpenGlControlBase).GetField("_bitmap", BindingFlags.NonPublic | BindingFlags.Instance)!;
	//protected OpenGlBitmap? bitmap;
	
	public static Vertex[] points = {
		new(new(0, 0, 0)),
		new(new(50, 100, 0)),
		new(new(100, 100, 0)),
		new(new(100, 50, 50)),
	};

	public static ushort[] indexes = {0, 1, 2, 0, 2, 3};

	private int _vertexBufferObject;
	private int _indexBufferObject;
	private int _vertexArrayObject;
	private int _vertexShader;
	private int _fragmentShader;
	private int _shaderProgram;
	private GlExtrasInterface _glExtras;

	protected override unsafe void OnOpenGlInit(GlInterface gl, int fb) {
		_glExtras = new(gl);

		_vertexShader = gl.CreateShader(GL_VERTEX_SHADER);
		Console.WriteLine(gl.CompileShaderAndGetError(_vertexShader, vertexShaderSrc));
		
		_fragmentShader = gl.CreateShader(GL_FRAGMENT_SHADER);
		Console.WriteLine(gl.CompileShaderAndGetError(_fragmentShader, fragmentShaderSrc));

		_shaderProgram = gl.CreateProgram();
		gl.AttachShader(_shaderProgram, _vertexShader);
		gl.AttachShader(_shaderProgram, _fragmentShader);

		const int posLoc = 0;
		gl.BindAttribLocationString(_shaderProgram, posLoc, "pos");
		Console.WriteLine(gl.LinkProgramAndGetError(_shaderProgram));
		
		_vertexBufferObject = gl.GenBuffer();
		gl.BindBuffer(GL_ARRAY_BUFFER, _vertexBufferObject);
		int vertexSize = sizeof(Vertex);
		fixed(Vertex* pointsPtr = points)
			gl.BufferData(GL_ARRAY_BUFFER, (IntPtr)(points.Length * vertexSize), (IntPtr)pointsPtr, GL_DYNAMIC_DRAW);
		
		_indexBufferObject = gl.GenBuffer();
		gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, _indexBufferObject);
		const int indexSize = sizeof(ushort);
		fixed(ushort* indexesPtr = indexes)
			gl.BufferData(GL_ELEMENT_ARRAY_BUFFER, (IntPtr)(indexes.Length * indexSize), (IntPtr)indexesPtr, GL_DYNAMIC_DRAW);

		_vertexArrayObject = _glExtras.GenVertexArray();
		_glExtras.BindVertexArray(_vertexArrayObject);
		
		gl.VertexAttribPointer(posLoc, 3, GL_FLOAT, 0, vertexSize, IntPtr.Zero);
		gl.EnableVertexAttribArray(posLoc);
	}

	protected override void OnOpenGlDeinit(GlInterface gl, int fb) {
		// Unbind everything
		gl.BindBuffer(GL_ARRAY_BUFFER, 0);
		gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
		_glExtras.BindVertexArray(0);
		gl.UseProgram(0);

		// Delete all resources.
		gl.DeleteBuffers(2, new[] { _vertexBufferObject, _indexBufferObject });
		_glExtras.DeleteVertexArrays(1, new[] { _vertexArrayObject });
		gl.DeleteProgram(_shaderProgram);
		gl.DeleteShader(_fragmentShader);
		gl.DeleteShader(_vertexShader);
	}

	protected override unsafe void OnOpenGlRender(GlInterface gl, int fb) {
		CheckError(gl, "start");
		//bitmap ??= (OpenGlBitmap?) bitmapField.GetValue(this);
		//bitmap.
		//points = null;
		points[0].pos.x = math.abs(MathF.Sin((float)DateTime.Now.TimeOfDay.TotalMilliseconds * .01f)) * 25;
		int vertexSize = sizeof(Vertex);
		fixed(Vertex* pointsPtr = points)
			_glExtras.BufferSubData(GL_ARRAY_BUFFER, vertexSize * 0, vertexSize * 1, pointsPtr + 0);
		//	gl.BufferData(GL_ARRAY_BUFFER, (IntPtr)(points.Length * vertexSize), (IntPtr)pointsPtr, GL_DYNAMIC_DRAW);
		
		//gl.ClearColor(
		//	math.abs(MathF.Sin((float) DateTime.Now.TimeOfDay.TotalMilliseconds * .001f)), 
		//	math.abs(MathF.Sin((float) DateTime.Now.TimeOfDay.TotalMilliseconds * .005f)), 
		//	math.abs(MathF.Sin((float) DateTime.Now.TimeOfDay.TotalMilliseconds * .0001f)),.1f);
		gl.ClearColor(.1f, .15f, .2f, 1f);
		gl.Clear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		gl.Enable(GL_DEPTH_TEST);
		
		
		gl.Enable(GL_MULTISAMPLE);
		
		gl.Viewport(0, 0, (int)Bounds.Width, (int)Bounds.Height);

		gl.BindBuffer(GL_ARRAY_BUFFER, _vertexBufferObject);
		gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, _indexBufferObject);
		//gl.BindBuffer(GL_VERTEX_ARRAY, _vertexArrayObject);
		//_glExtras.BindVertexArray(_vertexArrayObject);
		gl.UseProgram(_shaderProgram);

		Matrix4x4 projection = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI * .5f, (float)(Bounds.Width / Bounds.Height), .001f, 1000f);
		float z = 100 + MathF.Sin((float)DateTime.Now.TimeOfDay.TotalMilliseconds * .002f) * 25;
		Matrix4x4 view = Matrix4x4.CreateLookAt(new(0, 0, z), new(), new(0, 1, 0));

		float p = MathF.Sin((float)DateTime.Now.TimeOfDay.TotalMilliseconds * .003f) * 20;
		Matrix4x4 model = Matrix4x4.CreateTranslation(new(p,p,0));

		int modelLoc = gl.GetUniformLocationString(_shaderProgram, "model");
		int viewLoc = gl.GetUniformLocationString(_shaderProgram, "view");
		int projectionLoc = gl.GetUniformLocationString(_shaderProgram, "projection");

		gl.UniformMatrix4fv(modelLoc, 1, false, &model);
		gl.UniformMatrix4fv(viewLoc, 1, false, &view);
		gl.UniformMatrix4fv(projectionLoc, 1, false, &projection);
		
		CheckError(gl, "before render");
		
		gl.DrawElements(GL_TRIANGLES, indexes.Length, GL_UNSIGNED_SHORT, IntPtr.Zero);
		
		Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Render);
		CheckError(gl, "end");
	}
	
	private static void CheckError(GlInterface gl, string part)
	{
		int err;
		while ((err = gl.GetError()) != GL_NO_ERROR)
			Console.WriteLine(part + ": " + err);
	}
	
	private string GetShader(bool fragment, string shader)
	{
		var version = (GlVersion.Type == GlProfileType.OpenGL ?
			RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? 150 : 120 :
			100);
		var data = "#version " + version + "\n";
		if (GlVersion.Type == GlProfileType.OpenGLES)
			data += "precision mediump float;\n";
		if (version >= 150)
		{
			shader = shader.Replace("attribute", "in");
			if (fragment)
				shader = shader
				        .Replace("varying", "in")
				        .Replace("//DECLAREGLFRAG", "out vec4 outFragColor;")
				        .Replace("gl_FragColor", "outFragColor");
			else
				shader = shader.Replace("varying", "out");
		}

		data += shader;

		return data;
	}

	private string vertexShaderSrc => GetShader(false, @"
attribute vec3 pos;
uniform mat4 model;
uniform mat4 projection;
uniform mat4 view;

varying vec3 fragPos;
varying vec3 vecPos;

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	vecPos = pos;
}");
	
	private string fragmentShaderSrc => GetShader(true, @"
varying vec3 fragPos;
varying vec3 vecPos;
//DECLAREGLFRAG

void main() {
	gl_FragColor = vec4(sin(fragPos * .1) * .5 + .5,1.0);
}
");
}

public class GlExtrasInterface : GlInterfaceBase<GlInterface.GlContextInfo>
{
	public delegate void GlDeleteVertexArrays(int count, int[] buffers);
	public delegate void GlBindVertexArray(int array);
	public delegate void GlGenVertexArrays(int n, int[] rv);
	public unsafe delegate void GlBufferSubData(int trgt, int offset, int size, void* data);
	
	public GlExtrasInterface(GlInterface gl) : base(gl.GetProcAddress, gl.ContextInfo) { }
            
	[GlMinVersionEntryPoint("glDeleteVertexArrays", 3,0)]
	[GlExtensionEntryPoint("glDeleteVertexArraysOES", "GL_OES_vertex_array_object")]
	public GlDeleteVertexArrays DeleteVertexArrays { get; } = null!;

	[GlMinVersionEntryPoint("glBindVertexArray", 3,0)]
	[GlExtensionEntryPoint("glBindVertexArrayOES", "GL_OES_vertex_array_object")]
	public GlBindVertexArray BindVertexArray { get; } = null!;
	
	[GlMinVersionEntryPoint("glGenVertexArrays",3,0)]
	[GlExtensionEntryPoint("glGenVertexArraysOES", "GL_OES_vertex_array_object")]
	public GlGenVertexArrays GenVertexArrays { get; } = null!;
	
	[GlEntryPoint("glBufferSubData")]
	public GlBufferSubData BufferSubData { get; } = null!;

	public int GenVertexArray()
	{
		int[] rv = new int[1];
		GenVertexArrays(1, rv);
		return rv[0];
	}
}