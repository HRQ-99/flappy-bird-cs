using System.Threading.Tasks;
using Godot;

public partial class BoostTrail : Line2D {
  Curve2D curve;
  const int MaxPoints = 300;

  public override void _Ready() {
    curve = new Curve2D();
  }

  public override void _Process(double delta) {
    curve.AddPoint(GetNode<CharacterBody2D>("/root/Level/Bird").GlobalPosition);

    if (curve.GetBakedPoints().Length > MaxPoints) {
      curve.RemovePoint(0);
    }
    Points = curve.GetBakedPoints();
  }

  public async Task StopFunctionAsync() {
    SetProcess(false);
    Tween tween = GetTree().CreateTween();
    tween.TweenProperty(this, "modulate:a", 0, .01);
    tween.TweenProperty(this, "scale", 0, 0.03);
    await ToSignal(this, Tween.SignalName.Finished);
    QueueFree();
  }

  public static BoostTrail CreateTrail() {
    PackedScene trailScene = GD.Load<PackedScene>("uid://dgme4gg62lprm");
    return trailScene.Instantiate<BoostTrail>();
  }
}
