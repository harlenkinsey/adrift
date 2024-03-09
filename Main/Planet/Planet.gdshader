shader_type spatial;

uniform sampler2D sandTexture;
uniform sampler2D sandNormal : hint_normal;
uniform sampler2D mountainTexture;
uniform sampler2D mountainNormal : hint_normal;
uniform sampler2D grassTexture;
uniform float minHeight;
uniform float maxHeight;
varying float height;

void vertex()
{
	height = length(VERTEX);
	UV = UV * vec2(20.0, 20.0);
}

void fragment() {
	
	float between = height / (maxHeight - minHeight) - (minHeight / (maxHeight - minHeight));
	
	if(between <= 0.1 && abs(dot(normalize(VERTEX), NORMAL)) > 0.9)
	{
		ALBEDO = texture(sandTexture, UV).rgb;
		NORMAL_MAP = texture(sandNormal, UV).rgb;
	}
	else if(between <= 0.2 && abs(dot(normalize(VERTEX), NORMAL)) > 0.9)
	{
		ALBEDO = texture(grassTexture, UV).rgb;
	}
	else
	{
		ALBEDO = texture(mountainTexture, UV).rgb;
		NORMAL_MAP = texture(mountainNormal, UV).rgb;
	}
}
