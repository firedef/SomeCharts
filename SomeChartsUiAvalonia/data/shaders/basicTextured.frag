// PROCESS FRAGMENT

// inputs
in vec3 fragPos;
in vec3 fragNormal;
in vec2 fragUv;
in vec4 fragCol;
in vec2 texCoord;

// uniforms
uniform sampler2D texture0;

// outputs
out vec4 outFragColor;

void main() {
	vec4 texColor = texture2D(texture0, texCoord);

	gl_FragColor = texColor * fragCol;
}