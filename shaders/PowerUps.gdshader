shader_type canvas_item;

void vertex() {
	VERTEX.y+=sin(TIME)*100.0;
}

void fragment() { }
