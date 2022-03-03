using System;
using System.Runtime.InteropServices;
using Avalonia.OpenGL;
using SomeChartsUi.utils.shaders;
using static Avalonia.OpenGL.GlConsts;

namespace SomeChartsUiAvalonia.utils;

public static class GlShaders {
	public static readonly GlShader basic = new("", @"
// PROCESS VERTEX
// ADD ATTRIBUTES
// ADD MATRICES

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	fragUv = uv;
	fragCol = col;
	fragNormal = normal;
}
", @"
// PROCESS FRAGMENT
// DECLAREGLFRAG

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;

void main() {
	gl_FragColor = fragCol;
}
");
	
	public static readonly GlShader diffuse = new("", @"
// PROCESS VERTEX
// ADD ATTRIBUTES
// ADD MATRICES

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec3 viewPos;

#extension GL_ARB_gpu_shader5 : enable

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	fragUv = uv;
	fragCol = col;
	fragNormal = normalize(mat3(transpose(inverse(model))) * normal);
}
", @"
// PROCESS FRAGMENT
// DECLAREGLFRAG

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;

uniform vec3 cameraPos;

uniform vec3 lightDir = normalize(vec3(-0.2,1,0.1));
uniform float lightIntensity = 2;
uniform vec3 lightCol = vec3(1, 0.6, 0.4);
uniform vec3 ambientCol = vec3(0.1, 0.1, 0.15);
uniform vec3 diffuseCol = vec3(0.7, 0.7, 0.8);
uniform float specularIntensity = 0.5;

void main() {
	vec3 viewDir = normalize(cameraPos - fragPos);
	vec3 reflectDir = reflect(-lightDir, fragNormal); 
	float specularStrength = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	vec3 specular = specularIntensity * specularStrength * lightCol; 

	float diff = max(dot(fragNormal,lightDir), 0);
	vec3 diffuse = diff * lightCol * diffuseCol * lightIntensity;

	gl_FragColor = vec4(diffuse + ambientCol + specular, 1);
}
");
}

public class GlShader : Shader {

	
	public static GlVersion glVersion;
	public static GlInterface? gl;
	public int vertexShader;
	public int fragmentShader;
	public int shaderProgram;

	public void TryCompile() {
		if (gl == null) return;
		
		vertexShader = gl.CreateShader(GL_VERTEX_SHADER);
		Console.WriteLine(gl.CompileShaderAndGetError(vertexShader, vertexShaderSrc));
		
		fragmentShader = gl.CreateShader(GL_FRAGMENT_SHADER);
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
}