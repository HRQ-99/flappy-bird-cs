using Godot;

public partial class ShieldPowerUp : Area2D, IPowerUps {

  const string ShieldPath = "scenes/PowerUps/Shield.tscn";
  PackedScene _ShieldScene = ResourceLoader.Load<PackedScene>(ShieldPath);

  public void PowerActivate(Node2D bodyEntered) {
    if (bodyEntered.IsInGroup("Bird")) {
      bodyEntered.AddChild(_ShieldScene.Instantiate<Area2D>());

      CallDeferred("set_monitoring", false);
      Visible = false;

      GetNode<AudioStreamPlayer2D>("SoundEffect").Play();
    }
  }

  public void MusicFadeOut(Node2D bodyEntered) {
    if (bodyEntered.IsInGroup("Bird")) {
      Tween musicFade = CreateTween();
      musicFade.TweenProperty(GetNode<AudioStreamPlayer>("/root/Global/Background"), "volume_db", -10, 2);
      musicFade.Finished += MusicFadeIn;
    }
  }

  public void MusicFadeIn() {
    Tween musicFade = CreateTween();
    musicFade.TweenProperty(GetNode<AudioStreamPlayer>("/root/Global/Background"), "volume_db", 0, 1.5);
  }

  public void PowerExpired() { QueueFree(); }
}