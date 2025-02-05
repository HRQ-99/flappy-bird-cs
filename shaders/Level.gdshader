shader_type canvas_item;
uniform bool applyRedHue;
uniform vec4 redBackground;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec4 inputColor=texture(TEXTURE,UV);
	if (applyRedHue){
		COLOR=mix(inputColor,redBackground,0.3);
	}
	else {
		COLOR=inputColor;
		}
	}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
