using Godot;

public partial class ScoreBoostPowerUp : Area2D, IPowerUps {
  double RegularScoreTime;
  double scoreBoostMultiplier = 0.5;

  Timer timerNode;

  public void PowerActivate(Node2D bodyEntered) {
    if (bodyEntered.IsInGroup("Bird")) {
      timerNode = GetParent<Node2D>().GetNode<Timer>("ScoreTimer");
      timerNode.WaitTime *= scoreBoostMultiplier;

      SetDeferred("monitoring", false);
      Visible = false;
      GetNode<Timer>("Timer").Start();
      
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

  public void PowerExpired() {
    timerNode.WaitTime /= scoreBoostMultiplier;
    
    QueueFree();
  }

}