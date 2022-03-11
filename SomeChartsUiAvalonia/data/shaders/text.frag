// PROCESS FRAGMENT

// inputs
in vec3 fragPos;
in vec3 fragNormal;
in vec2 fragUv;
in vec4 fragCol;
in vec2 texCoord;

// uniforms
uniform float u_gamma = 0.52;
uniform sampler2D texture0;
uniform vec3 cameraPos;
uniform float textQuality;

// outputs
out vec4 outFragColor;

float sample(vec2 coord) {
	float dist = texture2D(texture0, coord).r - .5;
	float alpha = clamp(dist / fwidth(dist) + .5, 0.0, 1.0);
	return alpha;
}

void main() {
	float s = dFdx(texCoord.x) / 3.0;

	if (textQuality == 1) { 
		float r = sample(texCoord - vec2(s,0));
		float g = sample(texCoord);
		float b = sample(texCoord + vec2(s,0));
		float a = max(r,max(g,b));
		gl_FragColor = vec4(r,g,b,a) * fragCol;
	}
	else {
		float a = sample(texCoord);
		gl_FragColor = vec4(1,1,1,a) * fragCol;
	}
}