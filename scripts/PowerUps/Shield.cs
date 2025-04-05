using Godot;

public partial class Shield : Area2D {

  [Signal] public delegate void ActivateShieldEventHandler();
  [Signal] public delegate void DeactivateShieldEventHandler();

  public override void _Ready() {
    ActivateShield += Bird.ActivateShield;
    DeactivateShield += Bird.ShieldExpired;
    EmitSignal(SignalName.ActivateShield);

    GetNode<Timer>("Timer").Start();
  }

  void PowerExpired() {
    Tween tween = CreateTween();
    tween.TweenProperty(this, "scale", new Vector2(0, 0), 0.5);
    tween.Finished += Expired;
  }

  void Expired() {
    EmitSignal(SignalName.DeactivateShield);
    QueueFree();
  }

  void HitPipe(Node2D bodyEntered) {
    if (bodyEntered.IsInGroup("Pipe")) {
      bodyEntered.QueueFree();
      EmitSignal(SignalName.DeactivateShield);
      QueueFree();
    }
  }
}