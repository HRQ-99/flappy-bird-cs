using Godot;

public partial class EndScreen : Control {
  public override void _Ready() {
    GetNode<RichTextLabel>("EndScreenCenterContainer/EndScreenVBox/Score").AppendText(Global.GlobalScore.ToString() + ".");
  }

  public override void _Process(double delta) {
    if (Input.IsActionJustPressed("Restart")) {
      GetTree().ChangeSceneToFile("scenes/Level.tscn");
    }

    if (Input.IsActionJustPressed("Escapekey")) {
      GetTree().Quit();
    }
  }
}
