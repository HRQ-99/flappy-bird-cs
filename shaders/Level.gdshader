shader_type canvas_item;

uniform bool applyRedHue;
uniform vec4 redBackground;
uniform float blend;

void fragment() {
    vec4 inputColor = texture(TEXTURE, UV);
	float inputPosition=FRAGCOORD.y*SCREEN_UV.y;

    if (applyRedHue && inputPosition > 330.0) {
        COLOR=mix(inputColor,redBackground,inputColor.a * blend * 0.3* sin(TIME) );
    } else {
        COLOR = inputColor;
    }
    
}