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
	
	public static readonly GlShader basicTextured = new("", @"
// PROCESS VERTEX
// ADD ATTRIBUTES
// ADD MATRICES

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

#extension GL_ARB_gpu_shader5 : enable

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	fragUv = uv;
	fragCol = col;
	fragNormal = normal;
	texCoord = uv;
}
", @"
// PROCESS FRAGMENT
// DECLAREGLFRAG

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

uniform sampler2D texture0;

void main() {
	vec4 texColor = texture2D(texture0, texCoord);

	gl_FragColor = texColor * fragCol;
}
");
	
	public static readonly GlShader basicText = new("", @"
// PROCESS VERTEX
// ADD ATTRIBUTES
// ADD MATRICES

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

#extension GL_ARB_gpu_shader5 : enable

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	fragUv = uv;
	fragCol = col;
	fragNormal = normal;
	texCoord = uv;
}
", @"
// PROCESS FRAGMENT
// DECLAREGLFRAG

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

uniform float u_gamma = 0.52;

uniform sampler2D texture0;

void main() {
	vec4 texColor = texture2D(texture0, texCoord);
	float dist = texColor.r;
	float alpha = smoothstep(0, 1, smoothstep(1 - u_gamma, 1 + u_gamma, dist) * 1000);

	gl_FragColor = vec4(1,1,1,alpha) * fragCol;
}
");
	
	public static readonly GlShader diffuse = new("", @"
// PROCESS VERTEX
// ADD MATRICES

attribute vec3 pos;
attribute vec3 normal;
attribute vec2 uv;
attribute vec4 col;

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec3 viewPos;
varying vec2 texCoord;

#extension GL_ARB_gpu_shader5 : enable

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	fragUv = uv;
	fragCol = col;
	//fragNormal = normal;
	fragNormal = normalize(mat3(transpose(inverse(model))) * normal);
	texCoord = uv;
}
", @"
// PROCESS FRAGMENT
// DECLAREGLFRAG

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

uniform vec3 cameraPos;

uniform sampler2D texture0;

uniform float time = 0;
uniform vec3 lightDirr = normalize(vec3(10,-1,-1));
uniform float lightIntensity = 2;
uniform vec3 lightCol = vec3(255/255.0, 139/255.0, 73/255.0);
uniform vec3 ambientCol = vec3(6/255.0, 12/255.0, 45/255.0);
uniform vec3 diffuseCol = vec3(0.7, 0.7, 0.8);
uniform float shininess = 4;
uniform float specularIntensity = 2.5;

void main() {
	vec3 lightDir = normalize(vec3(sin(time * 0.001) * 10, sin(time * 0.0001) * 10, sin(time * 0.0005) * 20));

	vec3 viewDir = normalize(cameraPos - fragPos);
	vec3 halfwayDir = normalize(lightDir + viewDir);
	float specularStrength = pow(max(dot(fragNormal, halfwayDir), 0.0), shininess);
	vec3 specular = specularIntensity * specularStrength * lightCol; 

	float diff = max(dot(fragNormal,lightDir), 0);
	vec3 diffuse = diff * lightCol * lightIntensity;

	vec4 texColor = texture2D(texture0, texCoord);

	gl_FragColor = vec4((diffuse + ambientCol + specular), 1) * texColor * fragCol;
}
");
}