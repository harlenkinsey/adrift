shader_type spatial;

uniform float minHeight;
uniform float maxHeight;
uniform sampler2D heightColor;

varying float height;

void vertex() {
	height = length(VERTEX);
}

void fragment() {
	float t = height / (maxHeight - minHeight) - (minHeight / (maxHeight - minHeight));
	vec3 color = texture(heightColor, vec2(t, UV.y)).rgb;
	ALBEDO = color;
}


