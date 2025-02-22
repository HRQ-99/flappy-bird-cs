using Godot;

public partial class PipeDestroyerPowerUp : Area2D, IPowerUps {
  const float birdScaleMultiplier = 8;
  const float birdSpeedMultiplier = 2;

  public void PowerActivate(Node2D bodyEntered) {
    if (bodyEntered.IsInGroup("Bird")) {
      Bird.Invincible = true;
      Bird.SpeedMultiplier *= birdSpeedMultiplier;
      Bird.PipeDestroyerActive = true;

      Tween birdScaleTween = CreateTween();
      birdScaleTween.TweenProperty(bodyEntered, "scale", bodyEntered.Scale * birdScaleMultiplier, 1);

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
    Tween birdScaleTween = CreateTween();
    CharacterBody2D bird = GetNode<CharacterBody2D>("/root/Level/Bird");

    birdScaleTween.TweenProperty(bird, "scale", bird.Scale / birdScaleMultiplier, 1);
    Bird.PipeDestroyerActive = false;
    GetNode<AudioStreamPlayer2D>("SoundEffect").Play();
    Bird.SpeedMultiplier /= birdSpeedMultiplier;
    birdScaleTween.Finished += DisableInvinciblity;
  }

  private void DisableInvinciblity() {
    Bird.Invincible = false;
    QueueFree();
  }
}
