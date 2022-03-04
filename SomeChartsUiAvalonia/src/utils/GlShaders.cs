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
	//fragNormal = normal;
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

	gl_FragColor = vec4(diffuse + ambientCol + specular, 1);
}
");
}