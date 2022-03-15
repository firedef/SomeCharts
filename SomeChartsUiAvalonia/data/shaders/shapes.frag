// PROCESS FRAGMENT

// inputs
in vec3 fragPos;
in vec3 fragNormal;
in vec2 fragUv;
in vec4 fragCol;

// outputs
out vec4 outFragColor;

// some code grabbed from https://thebookofshaders.com/07/
#define PI 3.14159265359
#define TWO_PI 6.28318530718

float circle(vec2 coords, float d) {
    const float radius = .9;
    float sm = radius * 4 * d;
    vec2 dist = coords - vec2(.5);
    return 1.0 - smoothstep(radius - sm, radius + sm, dot(dist, dist) * 4);
}

float triangle(vec2 coords, float d) {
    float sm = 2.5 * d;
    coords = coords * 2.0 - 1.0;
    
    const float n = 3;
    float angle = atan(coords.x, coords.y) + PI;
    float radius = TWO_PI / n;
    float dist = cos(floor(.5 + angle / radius) * radius - angle) * length(coords);
    return 1.0 - smoothstep(.5 - sm, .5 + sm, dist);
}

float quad(vec2 coords, float d) {
    float sm = 2.5 * d;
    coords = coords * 2.0 - 1.0;
    float dist = max(abs(coords.x), abs(coords.y));
    return 1.0 - smoothstep(.8 - sm, .8 + sm, dist);
}

float sample(vec2 coords) {
    float d = dFdx(coords.x);
    if (coords.x <= 1.0) return circle(coords, d);
    if (coords.x <= 2.0) return triangle(coords - vec2(1,0), d);
    return quad(coords - vec2(2,0), d);
}

void main() {
	gl_FragColor = sample(fragUv) * fragCol;
}