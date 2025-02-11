using Godot;

public interface PowerUps {

  enum PowerUpsList {
    SpeedBoost, ScoreBoost, Gun
  }
  abstract void PowerActivate(Node2D bodyEntered);
  abstract void PowerExpired();
}