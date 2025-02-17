using Godot;

public partial class SpeedBoostPowerUp : Area2D, PowerUps {

  BoostTrail birdBoostTrail;

  public void PowerActivate(Node2D bodyEntered) {
    if (bodyEntered.IsInGroup("Bird")) {
      Bird.SpeedMultiplier = 5;
      Bird.gravityMultiplier = 0.01f;
      Bird.Invincible = true;
      birdBoostTrail = BoostTrail.CreateTrail();
      CharacterBody2D bird = GetNode<CharacterBody2D>("/root/Level/Bird");
      bird.AddChild(birdBoostTrail);

      GetNode<AudioStreamPlayer2D>("SoundEffect").Play();
      Visible = false;
      GetNode<Timer>("Timer").Start();
    }
  }

  public void MusicFade(Node2D bodyEntered) {
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

  //TODO old power's expiring overwrites current one's buff
  public void PowerExpired() {
    Bird.SpeedMultiplier = 1;
    Bird.gravityMultiplier = 1;
    Bird.Invincible = false;
    GetNode<CharacterBody2D>("/root/Level/Bird").RemoveChild(birdBoostTrail);
    QueueFree();
  }
}
