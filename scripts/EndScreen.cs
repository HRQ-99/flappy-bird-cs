using Godot;

using System;

public partial class EndScreen : Control {
	public override void _Ready() {
		GetNode<RichTextLabel>("Score").AppendText(Global.GlobalScore.ToString());
	}

	public override void _Process(double delta) {
		if (Input.IsActionJustPressed("PlayAgain")) {
			GetTree().ChangeSceneToFile("scenes/Level.tscn");
		}
		if (Input.IsActionJustPressed("Escapekey")) {
			GetTree().Quit();
		}
	}
}
