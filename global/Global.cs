using Godot;

public partial class Global : Node {
  public static int GlobalAttemptNumber = 1;
  public static int GlobalScore = 0;
  //TODO change directory to user & remove conifgDire
  public static string SAVE_DIRE = "user/userSave.tres";

  public static AudioStreamPlayer Music;
  public static AudioStream backgroundMusicFile = GD.Load("res://sound/music/01.02. Minuet In G.mp3") as AudioStream;
  public static AudioStream levelMusicFile = GD.Load("res://sound/music/01.01. First Movement (Allegro Con Brio) From Symphony No. 5 In C Minor, Op. 67.mp3") as AudioStream;

  public override void _Ready() {
    Music = new AudioStreamPlayer() { Name = "Background", Stream = backgroundMusicFile, Bus = "Music", };
    AddChild(Music);
    Music.Play();
    Music.ProcessMode = ProcessModeEnum.Always;
    MusicChanged += MusicSmoothTransition;
  }

  [Signal] public delegate void MusicChangedEventHandler();

  //TODO (get current music time) slowly fade current music then slowly fade in the next track 
  public void MusicSmoothTransition() {
    Tween musicFade = CreateTween();
    musicFade.TweenProperty(Music, "volume_db", -30, 4);
    musicFade.Finished += MusicFadeIn;
  }

  public void MusicFadeIn() {
    Node currentScene = GetTree().CurrentScene;
    if (currentScene.IsInGroup("Level")) {
      Music.Stream = levelMusicFile;
    }
    else {
      Music.Stream = backgroundMusicFile;
    }
    Music.Play(5);
    Tween musicFade = CreateTween();
    musicFade.TweenProperty(Music, "volume_db", 0, 2);
  }

  public override void _EnterTree() {
    foreach (Node nodes in GetTree().Root.GetChildren()) {
      foreach (Godot.Collections.Dictionary props in nodes.GetPropertyList()) {
        foreach (System.Collections.Generic.KeyValuePair<Variant, Variant> values in props) {
          GD.Print(values);
        }
      }
      GD.Print("next node");
    }
  }
  public override void _ExitTree() {
    Input.SetCustomMouseCursor(null);
    Music.Stream = null;
    Music.QueueFree();
  }
}
