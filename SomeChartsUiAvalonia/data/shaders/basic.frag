// PROCESS FRAGMENT

// inputs
in vec3 fragPos;
in vec3 fragNormal;
in vec2 fragUv;
in vec4 fragCol;

// outputs
out vec4 outFragColor;

void main() {
	gl_FragColor = fragCol;
}