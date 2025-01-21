shader_type canvas_item;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec4 inputColor=texture(TEXTURE, UV);
	vec4 outputColor=vec4(inputColor.r,inputColor.g,inputColor.b,inputColor.a);
	COLOR=outputColor;
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
