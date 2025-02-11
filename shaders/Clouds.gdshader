shader_type canvas_item;

uniform float brightness = 0.8;
uniform float contrast = 2.5;
uniform float saturation = 0.8;
uniform vec4 color=vec4(1,1,1,0);
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec4 inputColor=texture(TEXTURE, UV);
	if (inputColor.a>0.0){
		vec4 outputColor=vec4(inputColor.r,inputColor.g,inputColor.b,max(0.5,sin(TIME)));
		COLOR=outputColor;
	}

	//
	//vec3 c = textureLod(TEXTURE, SCREEN_UV, 0.0).rgb;
	//c.r=sin(TIME);
	//c.g=cos(TIME);
	//c.b=sin(TIME);
	////c.rgb=;
    //c.rgb = mix(vec3(0.0), c.rgb, brightness);
    //c.rgb = mix(vec3(0.5), c.rgb, contrast);
    //c.rgb = mix(vec3(dot(vec3(1.0), c.rgb) * 0.33333), c.rgb, saturation);
    //COLOR.rgb = c;
	COLOR=color;
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
