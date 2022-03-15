// PROCESS FRAGMENT

// inputs
in vec3 fragPos;
in vec3 fragNormal;
in vec2 fragUv;
in vec4 fragCol;

// outputs
out vec4 outFragColor;

float quad(vec2 coords, float d) {
    float sm = 6 * d;
    const float r = .8;
    coords = coords * 2.0 - 1.0;
    float dist = max(abs(coords.x), abs(coords.y));
    return 1.0 - smoothstep(r - sm, r + sm, dist);
}

void main() {
    float d = dFdx(fragUv.x);
	gl_FragColor = quad(fragUv, d) * fragCol;
}