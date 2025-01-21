using Godot;

using System;

public partial class StartScreen : Control {
	private void StartGame() {
		GetTree().ChangeSceneToFile("scenes/Level.tscn");
	}

	private void OptionsMenu() {

	}

	private void AchievementsButton() {

	}
	private void ExitGame() {
		GetTree().Quit();
	}

    public override void _Ready() {
		GetNode<Button>("CenterContainer/StartButton").GrabFocus();
    }
    public override void _Process(double delta) {
		
	}
}
