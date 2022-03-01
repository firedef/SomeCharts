using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Avalonia;
using Avalonia.Input;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.OpenGL.Imaging;
using Avalonia.Threading;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.layers;
using SomeChartsUi.utils;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.vectors;
using SomeChartsUiAvalonia.backends;
using SomeChartsUiAvalonia.utils;
using SomeChartsUiAvalonia.utils.collections;
using static Avalonia.OpenGL.GlConsts;

namespace SomeChartsUiAvalonia.controls.gl;

public class AvaloniaGlChartsCanvas : OpenGlControlBase {
	private GlExtrasInterface _glExtras;
	public readonly ChartsCanvas canvas = CreateCanvas();
	
	/// <summary>pause redraw loop</summary>
	public bool stopRender;
	
	/// <summary>name of current canvas</summary>
	public string canvasName = "???";
	
	/// <summary>pointer (mouse) instance</summary>
	public IPointer? pointer;
	
	public AvaloniaGlChartsCanvas() {
		//_updateTimer = new(_ => Update(), null, 0, 10);
		canvas.controller = new AvaloniaGlCanvasUiController(canvas, this);
		Focusable = true;
	}
	
	private static ChartsCanvas CreateCanvas() {
		ChartsCanvas canvas = new(new GlChartsBackend());
		canvas.AddLayer("bg");
		canvas.AddLayer("normal");
		canvas.AddLayer("top");

		return canvas;
	}
	
	/// <summary>add element to layer</summary>
	public void AddElement(RenderableBase el, string layer = "normal") => (canvas.GetLayer(layer) ?? canvas.GetLayer(1)).AddElement(el);
	/// <summary>remove element from layer</summary>
	public void RemoveElement(RenderableBase el, string layer = "normal") => (canvas.GetLayer(layer) ?? canvas.GetLayer(1)).RemoveElement(el);


	protected override void OnOpenGlInit(GlInterface gl, int fb) {
		_glExtras = new(gl);
		GlMesh.gl = gl;
		GlMesh.glExtras = _glExtras;
		GlShaders.glVersion = GlVersion;
		GlShaderData.gl = gl;
		GlChartsBackend.gl = gl;
	}

	protected override void OnOpenGlDeinit(GlInterface gl, int fb) {
		// Unbind everything
		gl.BindBuffer(GL_ARRAY_BUFFER, 0);
		gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
		_glExtras.BindVertexArray(0);
		gl.UseProgram(0);
	}

	protected override void OnOpenGlRender(GlInterface gl, int fb) {
		gl.Enable(GL_DEPTH_TEST);
		
		gl.Enable(GL_MULTISAMPLE);
		
		gl.Viewport(0, 0, (int)Bounds.Width, (int)Bounds.Height);
		
		canvas.transform.screenBounds = Bounds.ch();
		canvas.transform.Update();
		canvas.GetLayer("bg")!.background = theme.default0_ind;

		Stopwatch sw = Stopwatch.StartNew();
		foreach (CanvasLayer layer in canvas.renderer!.layers)
			layer.Render();
		//context.Custom(new CustomAvaloniaRender(canvas, Bounds));
		
		Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Render);
		CheckError(gl, "end");
		canvas.renderTime = sw.Elapsed;
	}
	
	private static void CheckError(GlInterface gl, string part)
	{
		int err;
		while ((err = gl.GetError()) != GL_NO_ERROR)
			Console.WriteLine(part + ": " + err);
	}
	
	protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e) {
		stopRender = true;
		base.OnDetachedFromVisualTree(e);
	}
	protected override void OnPointerPressed(PointerPressedEventArgs e) {
		PointerPoint currentPoint = e.GetCurrentPoint(this);
		PointerButtons buttons = currentPoint.Properties.GetEnum();
		keymods mods = e.KeyModifiers.ch();

		MouseState s = new(currentPoint.Position.ch(), float2.zero, buttons, mods);

		canvas.controller?.OnMouseDown(s);
	}
	protected override void OnPointerMoved(PointerEventArgs e) {
		PointerPoint currentPoint = e.GetCurrentPoint(this);
		pointer = e.Pointer;
		PointerButtons buttons = currentPoint.Properties.GetEnum();
		keymods mods = e.KeyModifiers.ch();

		MouseState s = new(currentPoint.Position.ch(), float2.zero, buttons, mods);

		canvas.controller?.OnMouseMove(s);
	}
	protected override void OnPointerReleased(PointerReleasedEventArgs e) {
		PointerPoint currentPoint = e.GetCurrentPoint(this);
		PointerButtons buttons = currentPoint.Properties.GetEnum();
		keymods mods = e.KeyModifiers.ch();

		MouseState s = new(currentPoint.Position.ch(), float2.zero, buttons, mods);

		canvas.controller?.OnMouseUp(s);
	}
	protected override void OnPointerWheelChanged(PointerWheelEventArgs e) {
		PointerPoint currentPoint = e.GetCurrentPoint(this);
		PointerButtons buttons = currentPoint.Properties.GetEnum();
		keymods mods = e.KeyModifiers.ch();

		MouseState s = new(currentPoint.Position.ch(), e.Delta.ch(), buttons, mods);

		canvas.controller?.OnMouseScroll(s);
	}
	protected override void OnKeyUp(KeyEventArgs e) {
		keymods mods = e.KeyModifiers.ch();

		canvas.controller?.OnKey((keycode)e.Key, mods);
	}

	/*//public static FieldInfo bitmapField = typeof(OpenGlControlBase).GetField("_bitmap", BindingFlags.NonPublic | BindingFlags.Instance)!;
	//protected OpenGlBitmap? bitmap;

	//public Mesh mesh;
	public GlObject obj;
	
	// public static Vertex[] points = {
	// 	new(new(0, 0, 0)),
	// 	new(new(50, 100, 0)),
	// 	new(new(100, 100, 0)),
	// 	new(new(100, 50, 50)),
	// };
	//
	// public static ushort[] indexes = {0, 1, 2, 0, 2, 3};

	private int _vertexBufferObject;
	private int _indexBufferObject;
	private int _vertexArrayObject;
	private int _vertexShader;
	private int _fragmentShader;
	private int _shaderProgram;
	private GlExtrasInterface _glExtras;

	protected void GenerateMesh(GlInterface gl) {
		Vertex[] verts = {
			new(new(000, 000, 0), new(0, 0), color.softRed),
			new(new(000, 100, 0), new(0, 1), color.softPurple),
			new(new(100, 100, 0), new(1, 1), color.softBlue),
			new(new(100, 000, 0), new(1, 0), color.softRed)
		};

		ushort[] indexes = {0, 1, 2, 0, 2, 3};
		
		Mesh mesh = new(verts, indexes);
		obj = new(gl, _glExtras, mesh);
	}

	protected override unsafe void OnOpenGlInit(GlInterface gl, int fb) {
		Console.WriteLine("a");
		GenerateMesh(gl);
		Console.WriteLine("b");
		_glExtras = new(gl);

		_vertexShader = gl.CreateShader(GL_VERTEX_SHADER);
		Console.WriteLine(gl.CompileShaderAndGetError(_vertexShader, vertexShaderSrc));
		
		_fragmentShader = gl.CreateShader(GL_FRAGMENT_SHADER);
		Console.WriteLine(gl.CompileShaderAndGetError(_fragmentShader, fragmentShaderSrc));

		_shaderProgram = gl.CreateProgram();
		gl.AttachShader(_shaderProgram, _vertexShader);
		gl.AttachShader(_shaderProgram, _fragmentShader);

		const int posLoc = 0;
		const int uvLoc = 1;
		const int colLoc = 2;
		gl.BindAttribLocationString(_shaderProgram, posLoc, "pos");
		gl.BindAttribLocationString(_shaderProgram, uvLoc, "uv");
		gl.BindAttribLocationString(_shaderProgram, colLoc, "col");
		Console.WriteLine(gl.LinkProgramAndGetError(_shaderProgram));
		CheckError(gl, "after shader");
		
		_vertexBufferObject = gl.GenBuffer();
		gl.BindBuffer(GL_ARRAY_BUFFER, _vertexBufferObject);
		int vertexSize = sizeof(Vertex);
		
		Vertex* pointsPtr = mesh.vertices.dataPtr;
		gl.BufferData(GL_ARRAY_BUFFER, (IntPtr)(mesh.vertices.count * vertexSize), (IntPtr)pointsPtr, GL_DYNAMIC_DRAW);
		Console.WriteLine(mesh.vertices.count);
		
		_indexBufferObject = gl.GenBuffer();
		gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, _indexBufferObject);
		const int indexSize = sizeof(ushort);
		ushort* indexesPtr = mesh.indexes.dataPtr;
		gl.BufferData(GL_ELEMENT_ARRAY_BUFFER, (IntPtr)(mesh.indexes.count * indexSize), (IntPtr)indexesPtr, GL_DYNAMIC_DRAW);
		Console.WriteLine(mesh.indexes.count);
		
		_vertexArrayObject = _glExtras.GenVertexArray();
		_glExtras.BindVertexArray(_vertexArrayObject);
		
		gl.VertexAttribPointer(posLoc, 3, GL_FLOAT, 0, vertexSize, IntPtr.Zero);
		gl.VertexAttribPointer(uvLoc, 2, GL_FLOAT, 0, vertexSize, (IntPtr) (3 * sizeof(float)));
		gl.VertexAttribPointer(colLoc, 4, GL_FLOAT, 0, vertexSize, (IntPtr) ((3 + 2) * sizeof(float)));
		gl.EnableVertexAttribArray(posLoc);
		gl.EnableVertexAttribArray(uvLoc);
		gl.EnableVertexAttribArray(colLoc);
		CheckError(gl, "after init");
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
		//points[0].pos.x = math.abs(MathF.Sin((float)DateTime.Now.TimeOfDay.TotalMilliseconds * .01f)) * 25;
		//int vertexSize = sizeof(Vertex);
		//fixed(Vertex* pointsPtr = points)
		//	_glExtras.BufferSubData(GL_ARRAY_BUFFER, vertexSize * 0, vertexSize * 1, pointsPtr + 0);
		
		//	gl.BufferData(GL_ARRAY_BUFFER, (IntPtr)(points.Length * vertexSize), (IntPtr)pointsPtr, GL_DYNAMIC_DRAW);
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
		
		gl.DrawElements(GL_TRIANGLES, mesh.indexes.count, GL_UNSIGNED_SHORT, IntPtr.Zero);
		
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
attribute vec2 uv;
attribute vec4 col;
uniform mat4 model;
uniform mat4 projection;
uniform mat4 view;

varying vec3 fragPos;
varying vec2 fragUv;
varying vec4 fragCol;

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	fragUv = uv;
	fragCol = col;
}");
	
	private string fragmentShaderSrc => GetShader(true, @"
varying vec3 fragPos;
varying vec2 fragUv;
varying vec4 fragCol;
//DECLAREGLFRAG

void main() {
	gl_FragColor = fragCol;
}
");*/
}

public class GlExtrasInterface : GlInterfaceBase<GlInterface.GlContextInfo>
{
	public delegate void GlDeleteVertexArrays(int count, int[] buffers);
	public delegate void GlBindVertexArray(int array);
	public delegate void GlGenVertexArrays(int n, int[] rv);
	public unsafe delegate void GlBufferSubData(int trgt, nint offset, nint size, void* data);
	
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