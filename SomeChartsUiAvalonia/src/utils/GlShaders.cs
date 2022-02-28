using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Avalonia.OpenGL;
using SomeChartsUi.utils.shaders;
using static Avalonia.OpenGL.GlConsts;

namespace SomeChartsUiAvalonia.utils; 

public static class GlShaders {
	public static GlVersion glVersion;

	private static readonly Shader _basic = new("", ProcessShader(false,@"
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
}
"), ProcessShader(true,@"
varying vec3 fragPos;
varying vec2 fragUv;
varying vec4 fragCol;
//DECLAREGLFRAG

void main() {
	gl_FragColor = fragCol;
}
"));
	
	public static Dictionary<string, GlShaderData> shaders = new() {
		{"", new(_basic)}
	};

	public static GlShaderData basicShader = shaders[""];
	
	
	public static string ProcessShader(bool isFragment, string shader) {
		bool isOsX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
		bool isOpenGLES = glVersion.Type == GlProfileType.OpenGLES;
		
		int version = !isOpenGLES 
			? isOsX 
				? 150 
				: 120 
			: 100;
		string data = "#version " + version + "\n";
		if (isOpenGLES) data += "precision mediump float;\n";
		if (version >= 150)
		{
			shader = shader.Replace("attribute", "in");
			if (isFragment)
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
	
	public static GlShaderData? Get(string name) {
		shaders.TryGetValue(name, out GlShaderData? s);
		return s;
	}
	
	public static GlShaderData Get(Shader shader) {
		if (shaders.TryGetValue(shader.name, out GlShaderData? s)) return s;
		//shaders.Add(shader.name, GlShaderData.Compile(shader));
		shaders.Add(shader.name, new(shader));
		return shaders[shader.name];
	}
}

public class GlShaderData {
	public static GlInterface? gl;
	public Shader shader;
	public int vertexShader;
	public int fragmentShader;
	public int shaderProgram;

	public GlShaderData(Shader shader) {
		this.shader = shader;
	}

	public GlShaderData(Shader shader, int vertexShader, int fragmentShader, int shaderProgram) {
		this.shader = shader;
		this.vertexShader = vertexShader;
		this.fragmentShader = fragmentShader;
		this.shaderProgram = shaderProgram;
	}

	public void TryCompile() {
		if (gl == null) return;
		
		vertexShader = gl.CreateShader(GL_VERTEX_SHADER);
		Console.WriteLine(gl.CompileShaderAndGetError(vertexShader, shader.vertexShaderSrc));
		
		fragmentShader = gl.CreateShader(GL_FRAGMENT_SHADER);
		Console.WriteLine(gl.CompileShaderAndGetError(fragmentShader, shader.fragmentShaderSrc));

		shaderProgram = gl.CreateProgram();
		gl.AttachShader(shaderProgram, vertexShader);
		gl.AttachShader(shaderProgram, fragmentShader);
		
		const int posLoc = 0;
		const int uvLoc = 1;
		const int colLoc = 2;
		gl.BindAttribLocationString(shaderProgram, posLoc, "pos");
		gl.BindAttribLocationString(shaderProgram, uvLoc, "uv");
		gl.BindAttribLocationString(shaderProgram, colLoc, "col");
		
		Console.WriteLine(gl.LinkProgramAndGetError(shaderProgram));
	}

	public static GlShaderData Compile(Shader shader) {
		int vertexShader = gl.CreateShader(GL_VERTEX_SHADER);
		Console.WriteLine(gl.CompileShaderAndGetError(vertexShader, shader.vertexShaderSrc));
		
		int fragmentShader = gl.CreateShader(GL_FRAGMENT_SHADER);
		Console.WriteLine(gl.CompileShaderAndGetError(fragmentShader, shader.fragmentShaderSrc));

		int shaderProgram = gl.CreateProgram();
		gl.AttachShader(shaderProgram, vertexShader);
		gl.AttachShader(shaderProgram, fragmentShader);
		
		const int posLoc = 0;
		const int uvLoc = 1;
		const int colLoc = 2;
		gl.BindAttribLocationString(shaderProgram, posLoc, "pos");
		gl.BindAttribLocationString(shaderProgram, uvLoc, "uv");
		gl.BindAttribLocationString(shaderProgram, colLoc, "col");
		
		Console.WriteLine(gl.LinkProgramAndGetError(shaderProgram));

		return new(shader, vertexShader, fragmentShader, shaderProgram);
	}
}