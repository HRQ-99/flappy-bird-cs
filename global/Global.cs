using Godot;

public partial class Global : Node {
  public static int GlobalAttemptNumber = 1;
  public static int GlobalScore = 0;
  public static string SAVE_DIRE = "user://userSave.tres";

  public static AudioStreamPlayer BackgroundMusic;
  public static AudioStream backgroundMusicFile = GD.Load("res://sound/music/01.02. Minuet In G.mp3") as AudioStream;
  public static AudioStream levelMusicFile = GD.Load("res://sound/music/01.01. First Movement (Allegro Con Brio) From Symphony No. 5 In C Minor, Op. 67.mp3") as AudioStream;

  public override void _Ready() {
    BackgroundMusic = new AudioStreamPlayer() { Name = "Background", Stream = backgroundMusicFile, Bus = "Music", };
    AddChild(BackgroundMusic);
    BackgroundMusic.Play();
    BackgroundMusic.ProcessMode = ProcessModeEnum.Always;
    MusicChanged += MusicSmoothTransition;
  }

  [Signal] public delegate void MusicChangedEventHandler();

  public void MusicSmoothTransition() {
    Tween musicFade = CreateTween();
    musicFade.TweenProperty(BackgroundMusic, "volume_db", -30, 4);
    musicFade.Finished += MusicFadeIn;
  }

  public void MusicFadeIn() {
    Node currentScene = GetTree().CurrentScene;
    if (currentScene.IsInGroup("Level")) {
      BackgroundMusic.Stream = levelMusicFile;
    }
    else {
      BackgroundMusic.Stream = backgroundMusicFile;
    }
    BackgroundMusic.Play(5);
    Tween musicFade = CreateTween();
    musicFade.TweenProperty(BackgroundMusic, "volume_db", 0, 2);
  }

  public override void _ExitTree() {
    Input.SetCustomMouseCursor(null);
    BackgroundMusic.Stream = null;
    BackgroundMusic.QueueFree();
  }
}
