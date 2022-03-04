using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Avalonia.OpenGL;
using SomeChartsUi.utils.shaders;
using SomeChartsUi.utils.vectors;
using SomeChartsUiAvalonia.controls.gl;

namespace SomeChartsUiAvalonia.utils;

public class GlShader : Shader {
	public static GlVersion glVersion;
	public static GlInterface? gl;
	public static GlExtrasInterface? glExtras;
	public int vertexShader;
	public int fragmentShader;
	public int shaderProgram;

	public void TryCompile() {
		if (gl == null) return;
		GetUniforms();
		
		vertexShader = gl.CreateShader(GlConsts.GL_VERTEX_SHADER);
		Console.WriteLine(gl.CompileShaderAndGetError(vertexShader, vertexShaderSrc));
		
		fragmentShader = gl.CreateShader(GlConsts.GL_FRAGMENT_SHADER);
		Console.WriteLine(gl.CompileShaderAndGetError(fragmentShader, fragmentShaderSrc));

		shaderProgram = gl.CreateProgram();
		gl.AttachShader(shaderProgram, vertexShader);
		gl.AttachShader(shaderProgram, fragmentShader);
		
		const int posLoc = 0;
		const int normalLoc = 1;
		const int uvLoc = 2;
		const int colLoc = 3;
		gl.BindAttribLocationString(shaderProgram, posLoc, "pos");
		gl.BindAttribLocationString(shaderProgram, normalLoc, "normal");
		gl.BindAttribLocationString(shaderProgram, uvLoc, "uv");
		gl.BindAttribLocationString(shaderProgram, colLoc, "col");
		
		Console.WriteLine(gl.LinkProgramAndGetError(shaderProgram));
		
		GetUniforms();
	}
	
	public static string ProcessShader(bool isFragment, string shader) {
		bool isOsX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
		bool isOpenGles = glVersion.Type == GlProfileType.OpenGLES;

		shader = shader.Replace("// ADD ATTRIBUTES", @"
attribute vec3 pos;
attribute vec3 normal;
attribute vec2 uv;
attribute vec4 col;")
		               .Replace("// ADD MATRICES", @"
uniform mat4 model;
uniform mat4 projection;
uniform mat4 view;");
		
		int version = !isOpenGles ? isOsX ? 150 : 120 : 100;
		
		string data = "#version " + version + "\n";
		if (isOpenGles) data += "precision mediump float;\n";
		if (version >= 150)
		{
			shader = shader.Replace("attribute", "in");
			if (isFragment)
				shader = shader
				        .Replace("varying", "in")
				        .Replace("// DECLAREGLFRAG", "out vec4 outFragColor;")
				        .Replace("gl_FragColor", "outFragColor");
			else
				shader = shader.Replace("varying", "out");
		}

		data += shader.Replace("// PROCESS VERTEX", "").Replace("// PROCESS FRAGMENT", "");

		return data;
	}

	public GlShader(string name, string vertexShaderSrc, string fragmentShaderSrc) : base(name, vertexShaderSrc, fragmentShaderSrc) {
		if (vertexShaderSrc.Trim().StartsWith("// PROCESS VERTEX")) this.vertexShaderSrc = ProcessShader(false, vertexShaderSrc);
		if (fragmentShaderSrc.Trim().StartsWith("// PROCESS FRAGMENT")) this.fragmentShaderSrc = ProcessShader(true, fragmentShaderSrc);
	}

	private unsafe void GetUniforms() {
		int uniformCount = 0;
		gl!.GetProgramiv(shaderProgram, GlConsts.GL_ACTIVE_UNIFORMS, &uniformCount);

		const int stringBufferSize = 32;
		sbyte* namePtr = stackalloc sbyte[stringBufferSize];
		int size = 0; // size of var
		int length = 0; // name length
		int type = 0; // type of var

		uniforms = new ShaderUniform[uniformCount];
		for (int i = 0; i < uniformCount; i++) {
			glExtras!.GetActiveUniform(shaderProgram, i, stringBufferSize, &length, &size, &type, namePtr);
			string varName = new(namePtr, 0, length);
			uniforms[i] = new(varName, i, type, size);
		}
	}

	public unsafe void TrySetUniform<T>(string uniformName, T v) {
		int loc = uniforms.FirstOrDefault(u => u.name == uniformName).location;
		if (loc == -1) return;
		
		gl!.UseProgram(shaderProgram);

		switch (v) {
			case float v0:     gl.Uniform1f(loc, v0); return;
			case float2 v0:    glExtras!.Uniform2f(loc, v0.x, v0.y); return;
			case float3 v0:    glExtras!.Uniform3f(loc, v0.x, v0.y, v0.z); return;
			case float4 v0:    glExtras!.Uniform4f(loc, v0.x, v0.y, v0.z, v0.w); return;
			case Matrix4x4 v0: gl.UniformMatrix4fv(loc, 1, false, &v0); return;
			case object obj:
				switch (obj) {
					case float v0:     gl.Uniform1f(loc, v0); return;
					case float2 v0:    glExtras!.Uniform2f(loc, v0.x, v0.y); return;
					case float3 v0:    glExtras!.Uniform3f(loc, v0.x, v0.y, v0.z); return;
					case float4 v0:    glExtras!.Uniform4f(loc, v0.x, v0.y, v0.z, v0.w); return;
					case Matrix4x4 v0: gl.UniformMatrix4fv(loc, 1, false, &v0); return;
				}
				return;
			default:           throw new NotImplementedException();
		}
	}

	public void TryApplyMaterial(Material mat) {
		foreach (MaterialProperty property in mat.properties) 
			TrySetUniform(property.name, property.value);
	}
}