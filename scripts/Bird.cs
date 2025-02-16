using Godot;
using System.Linq;

public partial class Bird : CharacterBody2D {
  private CharacterBody2D birdCollision;
  [Export] private float fallSpeed = 100f;
  [Export] private float flySpeed = -1500f;
  [Export] private float moveSpeed = 150f;
  [Export] private float diveMultiplier = 5;

  public static bool Invincible = false;
  public static float SpeedMultiplier = 1.0f;
  public static float gravityMultiplier = 1.0f;

  enum RotationDirection { Up = -1, Down = 1 }

  public override void _Ready() {
    birdCollision = this;
    Invincible = false;
    SpeedMultiplier = 1;
    gravityMultiplier = 1;
  }

  public override void _PhysicsProcess(double delta) {
    Velocity = new Vector2(moveSpeed * SpeedMultiplier, fallSpeed * gravityMultiplier);

    if (Input.IsActionJustPressed("Flap")) {
      SpriteRotation((int)RotationDirection.Up);
      Velocity = new Vector2(0, flySpeed);
    }
    else if (Input.IsActionJustPressed("Dive")) {
      SpriteRotation((int)RotationDirection.Down);
      Velocity = new Vector2(moveSpeed, fallSpeed * diveMultiplier);
    }

    if (Input.IsActionJustPressed("GodMode")) {
      Invincible = !Invincible;
      GetTree().CurrentScene.GetNode<RichTextLabel>("UI/ScoreContainer/Godmode").Visible = Invincible;
    }

    if (MoveAndSlide() && !Invincible) {
      birdCollision.GetSlideCollision(0);
      KinematicCollision2D lastHit = birdCollision.GetLastSlideCollision();

      if (lastHit != null) {
        Node collider = lastHit.GetCollider() as Node;
        if (collider.Name != "Boundary") {
          Input.MouseMode = Input.MouseModeEnum.Visible;
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

  private void SpriteRotation(int direction) {
    Tween rotateSprite = CreateTween();
    rotateSprite.TweenProperty(this, "rotation", 0.25 * direction, 0.15);
    rotateSprite.TweenProperty(this, "rotation", 0, 0.25);
  }

  public void IncreaseBirdMoveSpeed() {
    moveSpeed = DifficultyManager.BirdMoveSpeed[DifficultyManager.DifficultyStage];
  }
}
