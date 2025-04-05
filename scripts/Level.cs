using Godot;
using System.Linq;

public partial class Level : Node2D {

  bool _gamePaused = false;
  int _score = 0;
  float _movePipeDistanceX = 800;
  int _bottomPipePositionY = 50;
  int _gapBetweenPipeY = 10;
  float _nextPipeLocationX = 450;
  float _minimumSpaceBetweenPipes = 55;
  const int MaxPipeNumbers = 5;

  CharacterBody2D _bird;
  Node2D _pipesSet;
  Control _pausedScreen;
  readonly PackedScene _pipeScene = (PackedScene)GD.Load("scenes/Pipe.tscn");

  ShaderMaterial _backgroundShaderMaterial;
  Sprite2D _backgroundSprite;

  [Signal] public delegate void IncreaseDifficultyEventHandler();
  [Signal] public delegate void DifficultyIncreasedEventHandler();

  public override void _Ready() {
	GetTree().Root.GetNode<Node>("Global").EmitSignal(nameof(Global.MusicChanged));
	// Global.Music.Stream = Global.levelMusicFile;
	// Global.Music.Play();
	_bird = GetNode<CharacterBody2D>("Bird");
	_pipesSet = GetNode<Node2D>("PipesSet");
	for (int i = 0; i <= MaxPipeNumbers; i++) {
	  Node2D pipes = MakePipePair();
	  _pipesSet.AddChild(pipes);
	  pipes.GlobalPosition = new Vector2(_nextPipeLocationX, pipes.GlobalPosition.Y);
	  _nextPipeLocationX += _movePipeDistanceX;
	}

	_pausedScreen = GetNode<Control>("UI/PausedScreenLayer/PausedScreen/PausedScreenButtonContainer");
	_backgroundSprite = GetNode<Sprite2D>("UI/BackgroundLayer/FlappyBackground");
	_backgroundShaderMaterial = (ShaderMaterial)_backgroundSprite.Material;

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
	if (_bird.Position.Y > 1000) {
	  Color redBackground = new(0.97f, 0.0f, 0.0f, 1.0f);
	  _backgroundShaderMaterial.SetShaderParameter("applyRedHue", true);
	  _backgroundShaderMaterial.SetShaderParameter("redBackground", redBackground);
	  _backgroundShaderMaterial.SetShaderParameter("blend", _bird.Position.Y * 0.0008);
	}
	else {
	  _backgroundShaderMaterial.SetShaderParameter("applyRedHue", false);
	}

	foreach (Node2D pipe in _pipesSet.GetChildren().Cast<Node2D>()) {
	  if (_bird.GlobalPosition.X - pipe.GlobalPosition.X > 1500) {
		_pipesSet.RemoveChild(pipe);
		pipe.QueueFree();
		Node2D nextPipe = MakePipePair();
		_pipesSet.AddChild(nextPipe);
		nextPipe.GlobalPosition = new Vector2(_nextPipeLocationX, nextPipe.GlobalPosition.Y);
		_nextPipeLocationX += _movePipeDistanceX;
	  }
	}

  }

  private static int RandomInt(float min, float max) {
	return new RandomNumberGenerator().RandiRange((int)min, (int)max);
  }

  //TODO make a minimun space b/w pipes
  private Node2D MakePipePair() {
	StaticBody2D topPipe = (StaticBody2D)_pipeScene.Instantiate();
	topPipe.RotationDegrees = 180;
	topPipe.Position = new Vector2(0, RandomInt(0 - _gapBetweenPipeY, 0 + _gapBetweenPipeY));

	StaticBody2D bottomPipe = (StaticBody2D)_pipeScene.Instantiate();
	bottomPipe.Position = new Vector2(0, RandomInt(_bottomPipePositionY - _gapBetweenPipeY, _bottomPipePositionY + _gapBetweenPipeY));

	Node2D pipePair = new() {
	  GlobalPosition = new Vector2(0, 300),
	  Scale = new Vector2(5, 7),
	};

	float currentGapLessThanMinimum = bottomPipe.Position.Y - topPipe.Position.Y - _minimumSpaceBetweenPipes;
	if (currentGapLessThanMinimum < -10) {
	  bottomPipe.Position = new Vector2(bottomPipe.Position.X, bottomPipe.Position.Y - currentGapLessThanMinimum);
	}
	pipePair.AddChild(topPipe);
	pipePair.AddChild(bottomPipe);
	return pipePair;
  }

  public override void _Input(InputEvent @event) {
	if (Input.IsActionJustPressed("Escapekey")) {
	  _pausedScreen.GetNode<Button>("ResumeButton").GrabFocus();
	  GetTree().Paused = !_gamePaused;
	  _pausedScreen.Visible = !_gamePaused;
	  _gamePaused = !_gamePaused;

	  switch (_gamePaused) {
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
	_pausedScreen.Visible = false;
	_gamePaused = false;
  }

  private void PressedRestartButton() {
	_gamePaused = false;
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
	_score++;
	GetNode<RichTextLabel>("UI/ScoreContainer/Score").Text = "Score : " + _score.ToString();
	Global.GlobalScore = _score;

	if (_score >= DifficultyManager.DifficultyIncreaseTriggerScores[DifficultyManager.DifficultyStage]) {
	  EmitSignal(SignalName.IncreaseDifficulty);
	}
  }

  public void LevelIncreaseDifficulty() {
	_movePipeDistanceX = DifficultyManager.PipeGap[DifficultyManager.DifficultyStage];
	GetNode<Timer>("PowerUpSpawnTimer").WaitTime = DifficultyManager.PowerSpawnTime[DifficultyManager.DifficultyStage];
	_minimumSpaceBetweenPipes = DifficultyManager.PipeMinimumGap[DifficultyManager.DifficultyStage];

	((Bird)_bird).IncreaseBirdMoveSpeed();
  }

  private void ChangeShader() {
	// background.Material
  }

  //use groups to check if power is in pipes, then move the power forward
  private void SpawnPowerUp() {
	int powerUpIndex = RandomInt(0, IPowerUps.PowerUpsEnumList.Count - 1);
	PackedScene chosenPowerUp = IPowerUps.PowerUpScenes[IPowerUps.PowerUpsEnumList[powerUpIndex]];
	Area2D powerUp = chosenPowerUp.Instantiate<Area2D>();

	powerUp.Position = new Vector2(_nextPipeLocationX + (_movePipeDistanceX / 2), 500);
	AddChild(powerUp);
  }
}
