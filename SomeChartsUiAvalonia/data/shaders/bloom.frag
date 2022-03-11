// PROCESS FRAGMENT

// inputs
in vec3 fragPos;
in vec3 fragNormal;
in vec2 fragUv;
in vec4 fragCol;
in vec2 texCoord;

// uniforms
uniform sampler2D texture0;
uniform float brightness = 2;
uniform float step = .0002;
uniform vec2 scale = vec2(1,1);

// outputs
out vec4 outFragColor;

void main() {
	vec4 orig = texture2D(texture0, texCoord);
	vec4 texColor = orig;

	float iter = 32;
	float sampleCount = 5;

	float angleStep = 3.14 / sampleCount;

	float c = 1;
	for (float i = 1; i <= iter; i++) {
		float s = step * pow(i, 1.2);

		float a = i * 1.2;
		for (float j = 0; j < sampleCount; j++) {
			float angle = a + j * angleStep;
			texColor += texture2D(texture0, texCoord + vec2(sin(angle)*s,cos(angle)*s) * scale);
		}
		c += sampleCount;
	}
	
	texColor /= c;
	texColor.x = pow(texColor.x, 3);
	texColor.y = pow(texColor.y, 3);
	texColor.z = pow(texColor.z, 3);
	texColor *= brightness;

	gl_FragColor = texColor + orig;
}