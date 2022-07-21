// PROCESS VERTEX

// attributes
in vec3 pos;
in vec3 normal;
in vec2 uv;
in vec4 col;

// uniforms
uniform mat4 mvp;
uniform vec2 screenSize;

// outputs
out vec3 fragPos;
out vec3 fragNormal;
out vec2 fragUv;
out vec4 fragCol;
out vec2 texCoord;

out vec2 subPixelSize;

#extension GL_ARB_gpu_shader5 : enable

void main() {
	gl_Position = mvp * vec4(pos, 1.0);
	subPixelSize = 0.33333 / screenSize;
	vec2 posXy = floor(gl_Position.xy / subPixelSize) * subPixelSize;
	gl_Position = vec4(posXy, gl_Position.zw);

	fragPos = gl_Position.xyz;
	fragUv = uv;
	fragCol = col;
	fragNormal = normal;
	texCoord = uv;
}