using Godot;
using Godot.Collections;
using System.Linq;

public partial class Level : Node2D {
  private bool GamePaused = false;
  private int score = 0;
  private int movePipeDistanceX = 800;
  private int bottomPipePositionY = 50;
  private int gapBetweenPipeY = 10;
  private int nextPipeLocationX = 450;
  private int DifficultyStage = 0;
  private int minimumSpaceBetweenPipes = 150;

  private const int MaxPipeNumbers = 7;

  CharacterBody2D Bird;
  StaticBody2D BackgroundBoundaries;
  Node2D PipesSet;
  Control PausedScreen;
  readonly PackedScene pipeScene = (PackedScene)GD.Load("scenes/Pipe.tscn");

  private ShaderMaterial backgroundShaderMaterial;
  private Sprite2D backgroundSprite;

  public override void _Ready() {
    Bird = GetNode<CharacterBody2D>("Bird");
    PipesSet = GetNode<Node2D>("PipesSet");
    for (int i = 0; i <= 4; i++) {
      Node2D pipes = makePipePair();
      PipesSet.AddChild(pipes);
      pipes.GlobalPosition = new Vector2(nextPipeLocationX, pipes.GlobalPosition.Y);
      nextPipeLocationX += movePipeDistanceX;
}

    BackgroundBoundaries = GetNode<StaticBody2D>("BackgroundBoundary");
    PausedScreen = GetNode<Control>("UI/PausedScreenLayer/PausedScreen/PausedScreenButtonContainer");
    backgroundSprite = GetNode<Sprite2D>("UI/BackgroundLayer/FlappyBackground");
    backgroundShaderMaterial = (ShaderMaterial)backgroundSprite.Material;
  }

  public override void _Process(double delta) {
    if (Bird.Position.Y > 1000) {
      Color redBackground = new(0.97f, 0.2f, 0.2f, 0.9f);
      backgroundShaderMaterial.SetShaderParameter("applyRedHue", true);
      backgroundShaderMaterial.SetShaderParameter("redBackground", redBackground);
      CreateTween();
      //TODO creeat tween
    }
    else {
      backgroundShaderMaterial.SetShaderParameter("applyRedHue", false);
    }
    if (BackgroundBoundaries.GlobalPosition.X - Bird.GlobalPosition.X < -1000) {
      BackgroundBoundaries.MoveLocalX(2000);
    }
    foreach (Node2D pipe in PipesSet.GetChildren().Cast<Node2D>()) {
      if (Bird.GlobalPosition.X - pipe.GlobalPosition.X > 1500) {
        GD.Print(pipe.Position, pipe.GlobalPosition);
        PipesSet.RemoveChild(pipe);
        pipe.QueueFree();
      }
    }
  }

  private static int RandomInt(float min, float max) {
    RandomNumberGenerator RNG = new();
    return RNG.RandiRange((int)min, (int)max);
  }
  //TODO make a minimun space b/w pipes
  private Node2D makePipePair() {
    StaticBody2D topPipe = (StaticBody2D)pipeScene.Instantiate();
    topPipe.RotationDegrees = 180;
    topPipe.Position = new Vector2(0, RandomInt(0 - gapBetweenPipeY, 0 + gapBetweenPipeY));

    StaticBody2D bottomPipe = (StaticBody2D)pipeScene.Instantiate();
    bottomPipe.Position = new Vector2(0, RandomInt(bottomPipePositionY - gapBetweenPipeY, bottomPipePositionY + gapBetweenPipeY));

    Node2D pipePair = new() {
      GlobalPosition = new Vector2(0, 300),
      Scale = new Vector2(4, 6)
    };

    pipePair.AddChild(topPipe);
    pipePair.AddChild(bottomPipe);
    return pipePair;
  }

  public override void _Input(InputEvent @event) {
    if (Input.IsActionJustPressed("Escapekey")) {
      PausedScreen.GetNode<Button>("ResumeButton").GrabFocus();
      GetTree().Paused = !GamePaused;
      PausedScreen.Visible = !GamePaused;
      GamePaused = !GamePaused;
    }
  }

  private void PressedResumeButton() {
    GetTree().Paused = false;
    PausedScreen.Visible = false;
    GamePaused = false;
  }

  private void PressedRestartButton() {
    GamePaused = false;
    GetTree().Paused = false;
    GetTree().ReloadCurrentScene();
  }

  private void PressedBackToTitleButton() {
    GetTree().Paused = false;
    GetTree().ChangeSceneToFile("scenes/StartScreen.tscn");
  }

  private void PressedExitButton() {
    GetTree().Quit();
  }

  private void ScoreManager() {
    score++;
    GetNode<RichTextLabel>("UI/ScoreContainer/Score").Text = "Score : " + score.ToString();
    Global.GlobalScore = this.score;
  }

  //TODO implement this
  private void IncreaseDifficulty() {
    DifficultyStage++;
    Bird birdObj = new();
    birdObj.IncreaseBirdMoveSpeed(DifficultyStage);

    Array<int> pipeGapChanges = new Array<int> { 190, 150 };
  }

  private void ChangeShader() {
    // background.Material
  }
}
