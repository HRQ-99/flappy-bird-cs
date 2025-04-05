using Godot;
using System.Linq;

public partial class Bird : CharacterBody2D {

  [Export] float _fallSpeed = 100f;
  [Export] float _flySpeed = -1500f;
  [Export] float _moveSpeed = 150f;
  [Export] float _diveMultiplier = 5;

   CharacterBody2D _bird;
   //Area2D _shield;

  // TODO make static getter/setter that also toggles the Godmode label
  public static bool Invincible = false;
  public static float SpeedMultiplier = 1.0f;
  public static float GravityMultiplier = 1.0f;

  public static bool PipeDestroyerActive = false;
  public static bool ShieldActive = false;

  enum RotationDirection { Up = -1, Down = 1 }


  public override void _Ready() {
    _bird = this;
    Invincible = false;
    SpeedMultiplier = 1;
    GravityMultiplier = 1;
  }

  public override void _PhysicsProcess(double delta) {
    Velocity = new Vector2(_moveSpeed * SpeedMultiplier, _fallSpeed * GravityMultiplier);

    if (Input.IsActionJustPressed("Flap")) {
      SpriteRotation((int)RotationDirection.Up);
      Velocity = new Vector2(0, _flySpeed);
    }
    else if (Input.IsActionPressed("Dive")) {
      SpriteRotation((int)RotationDirection.Down);
      Velocity = new Vector2(_moveSpeed, _fallSpeed * _diveMultiplier);
    }

    if (Input.IsActionJustPressed("GodMode")) {
      Invincible = !Invincible;
      GetTree().CurrentScene.GetNode<RichTextLabel>("UI/ScoreContainer/Godmode").Visible = Invincible;
    }

    MoveAndSlide();
    int collisionCount = _bird.GetSlideCollisionCount();

    if (!Invincible && collisionCount > 0) {
      KinematicCollision2D lastHit = _bird.GetLastSlideCollision();

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
      KinematicCollision2D lastHit = _bird.GetLastSlideCollision();

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
    _moveSpeed = DifficultyManager.BirdMoveSpeed[DifficultyManager.DifficultyStage];
  }

  public static void ActivateShield() {
    ShieldActive = true;
    Invincible = true;
  }

  public static void ShieldExpired() {
    ShieldActive = false;
    Invincible = false;
  }

  public void SpeedBoostCameraEffect(){
      Camera2D birdCamera= GetNode<Camera2D>("Camera2D");
      float initalXOffset=birdCamera.Offset.X;
      Tween cameraTween=CreateTween();
      cameraTween.TweenProperty(birdCamera, "offset", new Vector2(initalXOffset+400,0), 1.5);
      cameraTween.TweenProperty(birdCamera, "offset", new Vector2(initalXOffset,0), 6);
  }
}