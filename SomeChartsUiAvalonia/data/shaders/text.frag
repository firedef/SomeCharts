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

float gain(float x, float k)
{
  x = clamp(x, 0.0, 1.0);
  float s = sign(x-0.5);
  float o = (1.0+s)/2.0;
  return o - 0.5*s*pow(2.0*(o-s*x),k);
}

float sample(vec2 coord, float gammaAdd) {
	float dist = texture2D(texture0, coord).r - u_gamma - gammaAdd;

	float alpha = gain(dist / fwidth(dist) + u_gamma + gammaAdd, 2);
	return alpha;
}

void main() {
	const float rA = .4;
	const float bA = .4;

	float fragGamma = (fragCol.x + fragCol.y + fragCol.z) * .02;
	float s = -dFdx(texCoord.x) / 3.0;

	if (textQuality == 1) { 
		float g = sample(texCoord, fragGamma);
		float r = sample(texCoord - vec2(s,0), fragGamma);
		float b = sample(texCoord + vec2(s,0), fragGamma);
		
		float rContrast = max(r-g, 0) * rA;
		float bContrast = max(b-g, 0) * bA;

		float a = g + rContrast + bContrast;
		
		gl_FragColor = vec4(1,1,1,a) * fragCol;
		gl_FragColor.r = mix(gl_FragColor.r, r, r * rContrast);
		gl_FragColor.b = mix(gl_FragColor.b, b, b * bContrast);
	}
	else {
		float a = sample(texCoord, fragGamma);
		gl_FragColor = vec4(1,1,1,a) * fragCol;
	}
}