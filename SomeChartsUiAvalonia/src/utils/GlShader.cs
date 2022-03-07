using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Avalonia.OpenGL;
using MathStuff.vectors;
using SomeChartsUi.utils.shaders;
using SomeChartsUiAvalonia.controls.gl;

namespace SomeChartsUiAvalonia.utils;

public class GlShader : Shader {
	public int vertexShader;
	public int fragmentShader;
	public int shaderProgram;

	public void TryCompile() {
		if (GlInfo.gl == null) return;
		GetUniforms();
		
		vertexShader = GlInfo.gl.CreateShader(GlConsts.GL_VERTEX_SHADER);
		Console.WriteLine(GlInfo.gl.CompileShaderAndGetError(vertexShader, vertexShaderSrc));
		
		fragmentShader = GlInfo.gl.CreateShader(GlConsts.GL_FRAGMENT_SHADER);
		Console.WriteLine(GlInfo.gl.CompileShaderAndGetError(fragmentShader, fragmentShaderSrc));

		shaderProgram = GlInfo.gl.CreateProgram();
		GlInfo.gl.AttachShader(shaderProgram, vertexShader);
		GlInfo.gl.AttachShader(shaderProgram, fragmentShader);
		
		const int posLoc = 0;
		const int normalLoc = 1;
		const int uvLoc = 2;
		const int colLoc = 3;
		GlInfo.gl.BindAttribLocationString(shaderProgram, posLoc, "pos");
		GlInfo.gl.BindAttribLocationString(shaderProgram, normalLoc, "normal");
		GlInfo.gl.BindAttribLocationString(shaderProgram, uvLoc, "uv");
		GlInfo.gl.BindAttribLocationString(shaderProgram, colLoc, "col");
		
		Console.WriteLine(GlInfo.gl.LinkProgramAndGetError(shaderProgram));
		
		GetUniforms();
	}
	
	public static string ProcessShader(bool isFragment, string shader) {
		bool isOsX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
		bool isOpenGles = GlInfo.version!.Value.Type == GlProfileType.OpenGLES;

		shader = shader.Replace("// ADD ATTRIBUTES", @"
attribute vec3 pos;
attribute vec3 normal;
attribute vec2 uv;
attribute vec4 col;")
		               .Replace("// ADD MATRICES", @"
uniform mat4 model;
uniform mat4 projection;
uniform mat4 view;");
		
		int version = isOpenGles ? 110 : 330;
		// int version = !isOpenGles ? isOsX ? 150 : 120 : 100;
		
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
		GlInfo.gl!.GetProgramiv(shaderProgram, GlConsts.GL_ACTIVE_UNIFORMS, &uniformCount);

		const int stringBufferSize = 32;
		sbyte* namePtr = stackalloc sbyte[stringBufferSize];
		int size = 0; // size of var
		int length = 0; // name length
		int type = 0; // type of var

		uniforms = new ShaderUniform[uniformCount];
		for (int i = 0; i < uniformCount; i++) {
			GlInfo.glExt!.GetActiveUniform(shaderProgram, i, stringBufferSize, &length, &size, &type, namePtr);
			string varName = new(namePtr, 0, length);
			uniforms[i] = new(varName, i, type, size);
		}
	}

	public unsafe void TrySetUniform(string uniformName, object v) {
		ShaderUniform uniform = uniforms.FirstOrDefault(u => u.name == uniformName);
		if (uniform == default) return;
		int loc = uniform.location;
		
		GlInfo.gl!.UseProgram(shaderProgram);

		switch (v) {
			case int v0:     GlInfo.gl.Uniform1f(loc, v0); return;
			case float v0:     GlInfo.gl.Uniform1f(loc, v0); return;
			case float2 v0:    GlInfo.glExt!.Uniform2f(loc, v0.x, v0.y); return;
			case float3 v0:    GlInfo.glExt!.Uniform3f(loc, v0.x, v0.y, v0.z); return;
			case float4 v0:    GlInfo.glExt!.Uniform4f(loc, v0.x, v0.y, v0.z, v0.w); return;
			case Matrix4x4 v0: GlInfo.gl.UniformMatrix4fv(loc, 1, false, &v0); return;
			default:
				if (v.GetType().IsAssignableTo(typeof(Texture))) {
					Texture v0 = (Texture)v;
					if (v0 is not GlTexture tex) throw new NotSupportedException("opengl charts renderer supports only GlTexture");
					GlInfo.gl.ActiveTexture(GlConsts.GL_TEXTURE0);
					tex.Bind();
					return;
				}
				throw new NotImplementedException();
		}
	}

	public void TryApplyMaterial(Material mat) {
		foreach (MaterialProperty property in mat.properties) 
			TrySetUniform(property.name, property.value);
	}
}