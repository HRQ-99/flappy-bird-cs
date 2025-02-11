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
    }
    // GetNode<Timer>("Timer").Start();
  }

  //TODO old power's expiring overwrites current one's buff
  public void PowerExpired() {
    Bird.SpeedMultiplier = 1;
    Bird.gravityMultiplier = 1;
    Bird.Invincible = false;
    birdBoostTrail = null;
    QueueFree();
  }
}
