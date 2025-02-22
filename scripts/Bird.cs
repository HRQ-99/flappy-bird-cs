using Godot;
using System.Linq;

public partial class Bird : CharacterBody2D {

  [Export] float fallSpeed = 100f;
  [Export] float flySpeed = -1500f;
  [Export] float moveSpeed = 150f;
  [Export] float diveMultiplier = 5;

  static CharacterBody2D s_bird;
  public static bool Invincible = false;
  public static float SpeedMultiplier = 1.0f;
  public static float gravityMultiplier = 1.0f;

  public static bool PipeDestroyerActive = false;
  public static bool ShieldActive = false;

  enum RotationDirection { Up = -1, Down = 1 }

  Area2D _shield { set; get; }

  public override void _Ready() {
    s_bird = this;
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

    MoveAndSlide();
    int collisionCount = s_bird.GetSlideCollisionCount();

    if (!Invincible && collisionCount > 0) {
      KinematicCollision2D lastHit = s_bird.GetLastSlideCollision();

      if (lastHit != null) {
        Node collider = lastHit.GetCollider() as Node;
        if (collider.Name != "Boundary") {
          Input.MouseMode = Input.MouseModeEnum.Visible;
          SaveScore();
          GetTree().ChangeSceneToFile("scenes/EndScreen.tscn");
        }
      }
    }

    if (PipeDestroyerActive && collisionCount > 0) {
      KinematicCollision2D lastHit = s_bird.GetLastSlideCollision();

      if (lastHit != null) {
        Node2D collider = lastHit.GetCollider() as Node2D;
        if (collider.IsInGroup("Pipe")) { collider.QueueFree(); }
      }
    }
  }

  static void SaveScore() {
    if (FileAccess.FileExists(Global.SAVE_DIRE)) {
      if (GD.Load(Global.SAVE_DIRE) is SavedGame saveFile) {
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

  void SpriteRotation(int direction) {
    Tween rotateSprite = CreateTween();
    rotateSprite.TweenProperty(this, "rotation", 0.25 * direction, 0.15);
    rotateSprite.TweenProperty(this, "rotation", 0, 0.25);
  }

  public void IncreaseBirdMoveSpeed() {
    moveSpeed = DifficultyManager.BirdMoveSpeed[DifficultyManager.DifficultyStage];
  }

  public void ActivateShield() {
    ShieldActive = true;
    Invincible = true;
  }

  public void ShieldExpired() {
    ShieldActive = false;
    Invincible = false;
  }
}
