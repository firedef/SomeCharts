// PROCESS FRAGMENT

// inputs
in vec3 fragPos;
in vec3 fragNormal;
in vec2 fragUv;
in vec4 fragCol;
in vec2 texCoord;

// uniforms

// general
uniform sampler2D texture0;

// FXAA
uniform float enableFxaa = 1;
uniform vec2 u_texelStep;
uniform int u_showEdges = 0;
uniform float u_lumaThreshold = 0.5;
uniform float u_mulReduce = 1.0 / 8.0;
uniform float u_minReduce = 1.0 / 128.0;
uniform float u_maxSpan = 8.0;

// BLOOM
uniform float enableBloom = 0;
uniform float brightness = 2;
uniform float step = .0002;
uniform vec2 scale = vec2(1,1);

// outputs
out vec4 outFragColor;

vec4 processFXAA(vec4 col) {
    vec3 rgbM = texture(texture0, texCoord).rgb;

	// Sampling neighbour texels. Offsets are adapted to OpenGL texture coordinates. 
	vec3 rgbNW = textureOffset(texture0, texCoord, ivec2(-1, 1)).rgb;
    vec3 rgbNE = textureOffset(texture0, texCoord, ivec2(1, 1)).rgb;
    vec3 rgbSW = textureOffset(texture0, texCoord, ivec2(-1, -1)).rgb;
    vec3 rgbSE = textureOffset(texture0, texCoord, ivec2(1, -1)).rgb;

	// see http://en.wikipedia.org/wiki/Grayscale
	const vec3 toLuma = vec3(0.299, 0.587, 0.114);

	// Convert from RGB to luma.
	float lumaNW = dot(rgbNW, toLuma);
	float lumaNE = dot(rgbNE, toLuma);
	float lumaSW = dot(rgbSW, toLuma);
	float lumaSE = dot(rgbSE, toLuma);
	float lumaM = dot(rgbM, toLuma);

	// Gather minimum and maximum luma.
	float lumaMin = min(lumaM, min(min(lumaNW, lumaNE), min(lumaSW, lumaSE)));
	float lumaMax = max(lumaM, max(max(lumaNW, lumaNE), max(lumaSW, lumaSE)));

	// If contrast is lower than a maximum threshold ...
	if (lumaMax - lumaMin <= lumaMax * u_lumaThreshold) // ... do no AA and return.
		return vec4(rgbM, 1.0);

	// Sampling is done along the gradient.
	vec2 samplingDirection;	
	samplingDirection.x = -((lumaNW + lumaNE) - (lumaSW + lumaSE));
    samplingDirection.y =  ((lumaNW + lumaSW) - (lumaNE + lumaSE));

	// Sampling step distance depends on the luma: The brighter the sampled texels, the smaller the final sampling step direction.
    // This results, that brighter areas are less blurred/more sharper than dark areas.  
    float samplingDirectionReduce = max((lumaNW + lumaNE + lumaSW + lumaSE) * 0.25 * u_mulReduce, u_minReduce);

	// Factor for norming the sampling direction plus adding the brightness influence. 
	float minSamplingDirectionFactor = 1.0 / (min(abs(samplingDirection.x), abs(samplingDirection.y)) + samplingDirectionReduce);

	// Calculate final sampling direction vector by reducing, clamping to a range and finally adapting to the texture size. 
    samplingDirection = clamp(samplingDirection * minSamplingDirectionFactor, vec2(-u_maxSpan), vec2(u_maxSpan)) * u_texelStep;

	// Inner samples on the tab.
	vec3 rgbSampleNeg = texture(texture0, texCoord + samplingDirection * (1.0/3.0 - 0.5)).rgb;
	vec3 rgbSamplePos = texture(texture0, texCoord + samplingDirection * (2.0/3.0 - 0.5)).rgb;

	vec3 rgbTwoTab = (rgbSamplePos + rgbSampleNeg) * 0.5;  

	// Outer samples on the tab.
	vec3 rgbSampleNegOuter = texture(texture0, texCoord + samplingDirection * (0.0/3.0 - 0.5)).rgb;
	vec3 rgbSamplePosOuter = texture(texture0, texCoord + samplingDirection * (3.0/3.0 - 0.5)).rgb;
	
	vec3 rgbFourTab = (rgbSamplePosOuter + rgbSampleNegOuter) * 0.25 + rgbTwoTab * 0.5;   

	// Calculate luma for checking against the minimum and maximum value.
	float lumaFourTab = dot(rgbFourTab, toLuma);

	// Are outer samples of the tab beyond the edge ... 
	if (lumaFourTab < lumaMin || lumaFourTab > lumaMax) // ... yes, so use only two samples.
		col = vec4(rgbTwoTab, 1.0);
    // ... no, so use four samples. 
	else 
        col = vec4(rgbFourTab, 1.0);

	// Show edges for debug purposes.	
	if (u_showEdges != 0) col.r = 1.0;

    return col;
}

vec4 processBloom(vec4 col) {
    vec4 orig = col;
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

	return texColor + orig;
}

void main() {
	vec4 col = texture2D(texture0, texCoord);
    if (enableFxaa > 0) col = processFXAA(col);
    if (enableBloom > 0) col = processBloom(col);

	gl_FragColor = col;
}