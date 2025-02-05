using Godot;

public partial class StartScreen : CanvasLayer {
  private void StartGame() {
    GetTree().ChangeSceneToFile("scenes/Level.tscn");
  }

  private void OptionsMenu() {
    GetTree().ChangeSceneToFile("scenes/OptionsMenu.tscn");
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
