// PROCESS FRAGMENT

// inputs
in vec3 fragPos;
in vec3 fragNormal;
in vec2 fragUv;
in vec4 fragCol;
in vec2 texCoord;

// uniforms
uniform float u_gamma = 0.3;
uniform sampler2D texture0;
uniform float textQuality;

// outputs
out vec4 outFragColor;

float sample(vec2 coord, float gammaAdd) {
	float dist = texture2D(texture0, coord).r - u_gamma - gammaAdd;
	float alpha = clamp(dist / fwidth(dist) + u_gamma + gammaAdd, 0.0, 1.0);
	return alpha;
}

void main() {
	const float rA = 1;
	const float bA = 1;

	float fragGamma = (fragCol.x + fragCol.y + fragCol.z) * .1;
	float s = -dFdx(texCoord.x) / 3.0;

	if (textQuality == 1) { 
		float g = sample(texCoord, fragGamma);
		float r = sample(texCoord - vec2(s,0), fragGamma);
		float b = sample(texCoord + vec2(s,0), fragGamma);
		float a = max(r,max(g,b));
		float rContrast = max(r-g, 0);
		float bContrast = max(b-g, 0);
		
		gl_FragColor = vec4(1,1,1,a) * fragCol;
		gl_FragColor.r = mix(gl_FragColor.r, r, r * rContrast);
	}
	else {
		float a = sample(texCoord, fragGamma);
		gl_FragColor = vec4(1,1,1,a) * fragCol;
	}
}