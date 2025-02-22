using Godot;
using System.Linq;

public partial class Level : Node2D {

  bool GamePaused = false;
  int score = 0;
  float movePipeDistanceX = 800;
  int bottomPipePositionY = 50;
  int gapBetweenPipeY = 10;
  float nextPipeLocationX = 450;
  float minimumSpaceBetweenPipes = 55;
  const int MaxPipeNumbers = 5;

  CharacterBody2D Bird;
  Node2D PipesSet;
  Control PausedScreen;
  readonly PackedScene pipeScene = (PackedScene)GD.Load("scenes/Pipe.tscn");

  ShaderMaterial backgroundShaderMaterial;
  Sprite2D backgroundSprite;

  [Signal] public delegate void IncreaseDifficultyEventHandler();
  [Signal] public delegate void DifficultyIncreasedEventHandler();

  public override void _Ready() {
    GetTree().Root.GetNode<Node>("Global").EmitSignal(nameof(Global.MusicChanged));
    // Global.Music.Stream = Global.levelMusicFile;
    // Global.Music.Play();
    Bird = GetNode<CharacterBody2D>("Bird");
    PipesSet = GetNode<Node2D>("PipesSet");
    for (int i = 0; i <= MaxPipeNumbers; i++) {
      Node2D pipes = makePipePair();
      PipesSet.AddChild(pipes);
      pipes.GlobalPosition = new Vector2(nextPipeLocationX, pipes.GlobalPosition.Y);
      nextPipeLocationX += movePipeDistanceX;
    }

    PausedScreen = GetNode<Control>("UI/PausedScreenLayer/PausedScreen/PausedScreenButtonContainer");
    backgroundSprite = GetNode<Sprite2D>("UI/BackgroundLayer/FlappyBackground");
    backgroundShaderMaterial = (ShaderMaterial)backgroundSprite.Material;

    Input.MouseMode = Input.MouseModeEnum.Captured;

    IncreaseDifficulty += DifficultyManager.IncreaseDifficulty;
    IncreaseDifficulty += () => EmitSignal(SignalName.DifficultyIncreased);
    DifficultyIncreased += LevelIncreaseDifficulty;

    // string[] powerUps;
    // for (int i = 0; i < (powerUps = System.IO.Directory.GetFiles(powerUpScenesPath)).Length; i++) {
    //   powerUpScenes.Add(GD.Load<PackedScene>(powerUps[i]));
    // }
  }

  public override void _Process(double delta) {
    if (Bird.Position.Y > 1000) {
      Color redBackground = new(0.97f, 0.0f, 0.0f, 1.0f);
      backgroundShaderMaterial.SetShaderParameter("applyRedHue", true);
      backgroundShaderMaterial.SetShaderParameter("redBackground", redBackground);
      backgroundShaderMaterial.SetShaderParameter("blend", Bird.Position.Y * 0.0008);
    }
    else {
      backgroundShaderMaterial.SetShaderParameter("applyRedHue", false);
    }

    foreach (Node2D pipe in PipesSet.GetChildren().Cast<Node2D>()) {
      if (Bird.GlobalPosition.X - pipe.GlobalPosition.X > 1500) {
        PipesSet.RemoveChild(pipe);
        pipe.QueueFree();
        Node2D nextPipe = makePipePair();
        PipesSet.AddChild(nextPipe);
        nextPipe.GlobalPosition = new Vector2(nextPipeLocationX, nextPipe.GlobalPosition.Y);
        nextPipeLocationX += movePipeDistanceX;
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
      Scale = new Vector2(5, 7),
    };

    float currentGapLessThanMinimum = bottomPipe.Position.Y - topPipe.Position.Y - minimumSpaceBetweenPipes;
    if (currentGapLessThanMinimum < -10) {
      bottomPipe.Position = new Vector2(bottomPipe.Position.X, bottomPipe.Position.Y - currentGapLessThanMinimum);
    }
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

      switch (GamePaused) {
        case true:
          Input.MouseMode = Input.MouseModeEnum.Visible;
          break;
        case false:
          Input.MouseMode = Input.MouseModeEnum.Captured;
          break;
      }
    }

    if (Input.IsActionJustPressed("Restart")) {
      PressedRestartButton();
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
    GetTree().Root.GetNode<Node>("Global").EmitSignal(nameof(Global.MusicChanged));
    GetTree().Paused = false;
    GetTree().ChangeSceneToFile("scenes/StartScreen.tscn");
  }

  private void PressedExitButton() {
    GetTree().Quit();
  }

  private void ScoreManager() {
    score++;
    GetNode<RichTextLabel>("UI/ScoreContainer/Score").Text = "Score : " + score.ToString();
    Global.GlobalScore = score;

    if (score >= DifficultyManager.DifficultyIncreaseTriggerScores[DifficultyManager.DifficultyStage]) {
      EmitSignal(SignalName.IncreaseDifficulty);
    }
  }

  public void LevelIncreaseDifficulty() {
    movePipeDistanceX = DifficultyManager.PipeGap[DifficultyManager.DifficultyStage];
    GetNode<Timer>("PowerUpSpawnTimer").WaitTime = DifficultyManager.PowerSpawnTime[DifficultyManager.DifficultyStage];
    minimumSpaceBetweenPipes = DifficultyManager.PipeMinimumGap[DifficultyManager.DifficultyStage];

    ((Bird)Bird).IncreaseBirdMoveSpeed();
  }

  private void ChangeShader() {
    // background.Material
  }

  //use groups to check if power is in pipes, then move the power forward
  private void SpawnPowerUp() {
    int powerUpIndex = RandomInt(0, IPowerUps.powerUpsEnumList.Count - 1);
    PackedScene chosenPowerUp = IPowerUps.PowerUpScenes[IPowerUps.powerUpsEnumList[powerUpIndex]];
    Area2D powerUp = chosenPowerUp.Instantiate<Area2D>();

    powerUp.Position = new Vector2(nextPipeLocationX + 40, 500);
    AddChild(powerUp);
  }
}
