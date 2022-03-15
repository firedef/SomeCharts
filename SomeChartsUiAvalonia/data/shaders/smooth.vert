// PROCESS VERTEX

// attributes
in vec3 pos;
in vec3 normal;
in vec2 uv;
in vec4 col;

// uniforms
uniform mat4 mvp;

// output
out vec3 fragPos;
out vec3 fragNormal;
out vec2 fragUv;
out vec4 fragCol;

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = mvp * vec4(scaledPos, 1.0);
	fragPos = gl_Position.xyz;
	fragUv = uv;
	fragCol = col;
	fragNormal = normal;
}