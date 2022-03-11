// PROCESS VERTEX

// attributes
in vec3 pos;
in vec3 normal;
in vec2 uv;
in vec4 col;

// uniforms
uniform mat4 mvp;

// outputs
out vec3 fragPos;
out vec3 fragNormal;
out vec2 fragUv;
out vec4 fragCol;
out vec3 viewPos;
out vec2 texCoord;

#extension GL_ARB_gpu_shader5 : enable

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = mvp * vec4(scaledPos, 1.0);
	fragPos = gl_Position.xyz;
	fragUv = uv;
	fragCol = col;
	//fragNormal = normal;
	fragNormal = normalize(mat3(transpose(inverse(mvp))) * normal);
	texCoord = uv;
}