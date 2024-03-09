shader_type spatial;
render_mode blend_mix, depth_draw_opaque, cull_back, diffuse_lambert, specular_schlick_ggx;

uniform vec4 ColorParameter : source_color = vec4(0.236899, 0.455320, 0.985274, 1.000000);
uniform sampler2D noise1;
uniform sampler2D noise2;

void vertex() {
	vec2 pos = vec2(TIME) * vec2(0.001) + UV; 
	vec4 bruh = texture(noise1, pos);
	vec3 shee = NORMAL * 0.009;
	VERTEX = fma(vec3(bruh.xyz), shee, VERTEX);
}

void fragment() {

	vec2 pos = vec2(TIME) * vec2(0.001) + UV;

	vec4 texRead1 = texture(noise1, pos);

	vec2 posNeg = vec2(TIME) * vec2(-0.001) + UV;

	vec4 texRead2 = texture(noise2, posNeg);

	vec4 texOperationResult = texRead1 * texRead2;

	vec4 n_out11p0 = pow(texOperationResult, vec4(1.25));

	vec4 n_out4p0 = ColorParameter + n_out11p0;

// Output:0
	ALBEDO = vec3(n_out4p0.xyz);
	ALPHA = 0.8;
	ROUGHNESS = 0.0;
	//EMISSION = vec3(n_out11p0.xyz);
}