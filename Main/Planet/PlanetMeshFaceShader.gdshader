shader_type spatial;

uniform float minHeight;
uniform float maxHeight;
uniform sampler2D heightColor;

varying float height;

void vertex() { // runs for every vertex within the mesh and can pass variables to fragment()
	height = length(VERTEX);
}

void fragment() { // runs for every pixel on the mesh
	float t = height / (maxHeight - minHeight) - (minHeight / (maxHeight - minHeight));
	vec3 color = texture(heightColor, vec2(t, 0.0)).rgb;
	ALBEDO = color;
}


