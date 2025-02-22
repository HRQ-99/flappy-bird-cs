using Godot;

public partial class StartScreen : CanvasLayer {
  // const string StartScreenScene = "";
  const string OptionsMenuScene = "scenes/OptionsMenu.tscn";
  const string AchievementsScreenScene = "scenes/AchievementsScreen.tscn";

  CanvasLayer _startScreen;
  CanvasLayer OptionsMenu;
  CanvasLayer AchievementsScreen;

  private void StartGameButton() {
    GetTree().ChangeSceneToFile("scenes/Level.tscn");
  }

  private void OptionsMenuButton() {
    GetTree().ChangeSceneToFile("scenes/OptionsMenu.tscn");
  }

  private void AchievementsButton() {
    _startScreen.Visible = false;
    OptionsMenu.Visible = false;
    AddSibling(AchievementsScreen);
  }

  private void ExitGame() {
    GetTree().Quit();
  }

  public override void _Ready() {
    GetNode<Button>("CenterContainer/StartButton").GrabFocus();
    _startScreen = this;
    OptionsMenu = GD.Load<PackedScene>(OptionsMenuScene).Instantiate<CanvasLayer>();
    AchievementsScreen = GD.Load<PackedScene>(AchievementsScreenScene).Instantiate<CanvasLayer>();
  }

  public override void _Process(double delta) {
  }
}
