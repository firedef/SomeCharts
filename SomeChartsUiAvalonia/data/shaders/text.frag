// PROCESS FRAGMENT

// inputs
in vec3 fragPos;
in vec3 fragNormal;
in vec2 fragUv;
in vec4 fragCol;
in vec2 texCoord;
in vec2 subPixelSize;

// uniforms
uniform float u_gamma = 0.3;
uniform sampler2D texture0;
uniform float textQuality;

uniform float shift;

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

vec4 sampleSubpixel(vec2 coord, float gammaAdd, float offset) {
	float r = sample(coord - vec2(offset, 0), gammaAdd);
	float g = sample(coord, gammaAdd);
	float b = sample(coord + vec2(offset, 0), gammaAdd);
	float a = (r + g + b) * .333333;
	return vec4(r,g,b,a);
}

void main() {
	const float rA = .4;
	const float bA = .4;
	float shift_ = fract(subPixelSize.x);

	float fragGamma = (fragCol.x + fragCol.y + fragCol.z) * .02;
	float s = -dFdx(texCoord.x) / 3.0;

	if (textQuality == 1) { 
		vec4 col0 = sampleSubpixel(texCoord, fragGamma, s);
		vec4 col1 = sampleSubpixel(texCoord + vec2(s,0), fragGamma, s);
		vec4 curCol = col0;

		if (shift_ <= 1/3.0) {
			float z = 3 + shift_;
			curCol.r = mix(col0.r, col1.b, z);
			curCol.g = mix(col0.g, col0.r, z);
			curCol.b = mix(col0.b, col0.g, z);
		}
		else if (shift_ <= 2/3.0) {
			float z = 2 + shift_;
			curCol.r = mix(col1.b, col1.g, z);
			curCol.g = mix(col0.r, col1.b, z);
			curCol.b = mix(col0.g, col0.r, z);
		}
		else if (shift_ < 1) {
			float z = 1 + shift_;
			curCol.r = mix(col1.g, col1.r, z);
			curCol.g = mix(col1.b, col1.g, z);
			curCol.b = mix(col0.r, col1.b, z);
		}

		gl_FragColor = curCol * fragCol;
		// float g = sample(texCoord, fragGamma);
		// float r = sample(texCoord - vec2(s,0), fragGamma);
		// float b = sample(texCoord + vec2(s,0), fragGamma);
		
		// float rContrast = max(r-g, 0) * rA;
		// float bContrast = max(b-g, 0) * bA;

		// float a = g + rContrast + bContrast;
		
		// gl_FragColor = vec4(1,1,1,a) * fragCol;
		// gl_FragColor.r = mix(gl_FragColor.r, r, r * rContrast);
		// gl_FragColor.b = mix(gl_FragColor.b, b, b * bContrast);
	}
	else {
		float a = sample(texCoord, fragGamma);
		gl_FragColor = vec4(1,1,1,a) * fragCol;
	}
}