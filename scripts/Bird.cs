using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class Bird : CharacterBody2D {
  private CharacterBody2D birdCollision;
  [Export] private float fallSpeed = 100f;
  [Export] private float flySpeed = -1500f;
  [Export] private float moveSpeed = 150f;
  [Export] private bool Invincible = false;

  public override void _Ready() {
    birdCollision = this;
  }

  public override void _PhysicsProcess(double delta) {
    Velocity = new Vector2(moveSpeed, fallSpeed);

    if (Input.IsActionJustPressed("Flap")) {
      Velocity = new Vector2(0, flySpeed);
    }
    if (Input.IsActionJustPressed("GodMode")) {
      Invincible = !Invincible;
      GetTree().CurrentScene.GetNode<RichTextLabel>("UI/ScoreContainer/Godmode").Visible = Invincible;
    }

    if (MoveAndSlide() && !Invincible) {
      birdCollision.GetSlideCollision(0);
      var lastHit = birdCollision.GetLastSlideCollision();
      if (lastHit != null) {
        var collider = lastHit.GetCollider() as Node;
        if (collider.Name != "BackgroundBoundary") {
          SaveScore();
          GetTree().ChangeSceneToFile("scenes/EndScreen.tscn");
        }
      }
    }
  }

  public static void SaveScore() {
    if (FileAccess.FileExists(Global.SAVE_DIRE)) {
      SavedGame saveFile = GD.Load(Global.SAVE_DIRE) as SavedGame;
      if (saveFile != null) {
        saveFile.attemptNumber.Add(saveFile.attemptNumber.Count > 0 ? saveFile.attemptNumber.Last() + 1 : 1);
        saveFile.score.Add(Global.GlobalScore);
        ResourceSaver.Save(saveFile, Global.SAVE_DIRE);
        return;
      }
    }

    SavedGame newSave = new();
    newSave.attemptNumber.Add(Global.GlobalAttemptNumber);
    newSave.score.Add(Global.GlobalScore);
    ResourceSaver.Save(newSave, Global.SAVE_DIRE);
  }

  public void IncreaseBirdMoveSpeed(int stage) {
    Array<float> moveSpeedChanges = new Array<float> { 170f, 185f, 200f };
    moveSpeed = moveSpeedChanges[stage];
  }
}
