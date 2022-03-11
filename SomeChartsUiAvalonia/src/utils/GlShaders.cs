namespace SomeChartsUiAvalonia.utils;

public static class GlShaders {
	public static readonly GlShader basic = new("", @"
// PROCESS VERTEX
// ADD ATTRIBUTES
// ADD MATRICES

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	fragUv = uv;
	fragCol = col;
	fragNormal = normal;
}
", @"
// PROCESS FRAGMENT
// DECLAREGLFRAG

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;

void main() {
	gl_FragColor = fragCol;
}
");

	public static readonly GlShader basicTextured = new("", @"
// PROCESS VERTEX
// ADD ATTRIBUTES
// ADD MATRICES

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

#extension GL_ARB_gpu_shader5 : enable

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	fragUv = uv;
	fragCol = col;
	fragNormal = normal;
	texCoord = uv;
}
", @"
// PROCESS FRAGMENT
// DECLAREGLFRAG

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

uniform sampler2D texture0;

void main() {
	vec4 texColor = texture2D(texture0, texCoord);

	gl_FragColor = texColor * fragCol;
}
");
	
		public static readonly GlShader postFxFXAA = new("", @"
// PROCESS VERTEX
// ADD ATTRIBUTES
// ADD MATRICES

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

#extension GL_ARB_gpu_shader5 : enable

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	fragUv = uv;
	fragCol = col;
	fragNormal = normal;
	texCoord = uv;
}
", @"
// PROCESS FRAGMENT
// DECLAREGLFRAG

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

uniform sampler2D texture0;

uniform vec2 u_texelStep;
uniform int u_showEdges = 0;
uniform int u_fxaaOn = 1;

uniform float u_lumaThreshold = 0.5;
uniform float u_mulReduce = 1.0 / 8.0;
uniform float u_minReduce = 1.0 / 128.0;
uniform float u_maxSpan = 8.0;

float sampleLightness(vec2 coord) {
	vec4 col = texture(texture0, coord);
	return max(col.r, max(col.g, col.b));
}

void main() {
	const float str = .1;
	vec4 col = texture(texture0, texCoord);
	float r = sampleLightness(texCoord - vec2(1,0) * u_texelStep / 3);
	float b = sampleLightness(texCoord + vec2(1,0) * u_texelStep / 3);

	col.r *= 1 + r * str;
	col.b *= 1 + b * str;

	gl_FragColor = col;
}
");
	
	public static readonly GlShader postFxFXAA2 = new("", @"
// PROCESS VERTEX
// ADD ATTRIBUTES
// ADD MATRICES

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

#extension GL_ARB_gpu_shader5 : enable

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	fragUv = uv;
	fragCol = col;
	fragNormal = normal;
	texCoord = uv;
}
", @"
// PROCESS FRAGMENT
// DECLAREGLFRAG

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

uniform sampler2D texture0;

uniform vec2 u_texelStep;
uniform int u_showEdges = 0;
uniform int u_fxaaOn = 1;

uniform float u_lumaThreshold = 0.5;
uniform float u_mulReduce = 1.0 / 8.0;
uniform float u_minReduce = 1.0 / 128.0;
uniform float u_maxSpan = 8.0;

void main() {
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
	if (lumaMax - lumaMin <= lumaMax * u_lumaThreshold)
	{
		// ... do no AA and return.
		gl_FragColor = vec4(rgbM, 1.0);
		
		return;
	}  

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
	if (lumaFourTab < lumaMin || lumaFourTab > lumaMax)
	{
		// ... yes, so use only two samples.
		gl_FragColor = vec4(rgbTwoTab, 1.0); 
	}
	else
	{
		// ... no, so use four samples. 
		gl_FragColor = vec4(rgbFourTab, 1.0);
	}

	// Show edges for debug purposes.	
	if (u_showEdges != 0)
	{
		gl_FragColor.r = 1.0;
	}
}
");
	
	public static readonly GlShader postFxBloom = new("", @"
// PROCESS VERTEX
// ADD ATTRIBUTES
// ADD MATRICES

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

#extension GL_ARB_gpu_shader5 : enable

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	fragUv = uv;
	fragCol = col;
	fragNormal = normal;
	texCoord = uv;
}
", @"
// PROCESS FRAGMENT
// DECLAREGLFRAG

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

uniform sampler2D texture0;
uniform float brightness = 2;
uniform float step = .0002;
uniform vec2 scale = vec2(1,1);

void main() {
	//float s = .001;
	//vec2 coord = texCoord + vec2(sin(texCoord.x * 50), cos(texCoord.y * 50)) * .01;

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
");
	
	public static readonly GlShader postFxWobble = new("", @"
// PROCESS VERTEX
// ADD ATTRIBUTES
// ADD MATRICES

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

#extension GL_ARB_gpu_shader5 : enable

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	fragUv = uv;
	fragCol = col;
	fragNormal = normal;
	texCoord = uv;
}
", @"
// PROCESS FRAGMENT
// DECLAREGLFRAG

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

uniform sampler2D texture0;

void main() {
	vec2 coord = texCoord + vec2(sin(texCoord.x * 50), cos(texCoord.y * 50)) * .01;
	vec4 texColor = texture2D(texture0, coord);

	gl_FragColor = texColor;
}
");

	public static readonly GlShader basicText = new("", @"
// PROCESS VERTEX
// ADD ATTRIBUTES
// ADD MATRICES

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

#extension GL_ARB_gpu_shader5 : enable

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	fragUv = uv;
	fragCol = col;
	fragNormal = normal;
	texCoord = uv;
}
", @"
// PROCESS FRAGMENT
// DECLAREGLFRAG

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

uniform float u_gamma = 0.52;

uniform sampler2D texture0;
uniform vec3 cameraPos;
uniform float textQuality;

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
");

	public static readonly GlShader diffuse = new("", @"
// PROCESS VERTEX
// ADD MATRICES

attribute vec3 pos;
attribute vec3 normal;
attribute vec2 uv;
attribute vec4 col;

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec3 viewPos;
varying vec2 texCoord;

#extension GL_ARB_gpu_shader5 : enable

void main() {
	float scale = 1.0;
	vec3 scaledPos = pos * scale;

	gl_Position = projection * view * model * vec4(scaledPos, 1.0);
	fragPos = vec3(model * vec4(pos,1.0));
	fragUv = uv;
	fragCol = col;
	//fragNormal = normal;
	fragNormal = normalize(mat3(transpose(inverse(model))) * normal);
	texCoord = uv;
}
", @"
// PROCESS FRAGMENT
// DECLAREGLFRAG

varying vec3 fragPos;
varying vec3 fragNormal;
varying vec2 fragUv;
varying vec4 fragCol;
varying vec2 texCoord;

uniform vec3 cameraPos;

uniform sampler2D texture0;

uniform float time = 0;
uniform vec3 lightDirr = normalize(vec3(10,-1,-1));
uniform float lightIntensity = 2;
uniform vec3 lightCol = vec3(255/255.0, 139/255.0, 73/255.0);
uniform vec3 ambientCol = vec3(6/255.0, 12/255.0, 45/255.0);
uniform vec3 diffuseCol = vec3(0.7, 0.7, 0.8);
uniform float shininess = 4;
uniform float specularIntensity = 2.5;

void main() {
	vec3 lightDir = normalize(vec3(sin(time * 0.001) * 10, sin(time * 0.0001) * 10, sin(time * 0.0005) * 20));

	vec3 viewDir = normalize(cameraPos - fragPos);
	vec3 halfwayDir = normalize(lightDir + viewDir);
	float specularStrength = pow(max(dot(fragNormal, halfwayDir), 0.0), shininess);
	vec3 specular = specularIntensity * specularStrength * lightCol; 

	float diff = max(dot(fragNormal,lightDir), 0);
	vec3 diffuse = diff * lightCol * lightIntensity;

	vec4 texColor = texture2D(texture0, texCoord);

	gl_FragColor = vec4((diffuse + ambientCol + specular), 1) * texColor * fragCol;
}
");
}