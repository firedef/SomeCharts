// PROCESS FRAGMENT

// inputs
in vec3 fragPos;
in vec3 fragNormal;
in vec2 fragUv;
in vec4 fragCol;
in vec2 texCoord;

// uniforms
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

// outputs
out vec4 outFragColor;

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