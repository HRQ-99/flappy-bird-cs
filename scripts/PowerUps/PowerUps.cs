using Godot;

public interface PowerUps {

  enum PowerUpsList {
    SpeedBoost, ScoreBoost, Gun
  }

  protected abstract void PowerActivate(Node2D bodyEntered);
  protected abstract void MusicFade(Node2D bodyEntered);
  protected abstract void MusicFadeIn();
  protected abstract void PowerExpired();
}