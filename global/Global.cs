using Godot;

public partial class Global : Node {
  public static int GlobalAttemptNumber = 1;
  public static int GlobalScore = 0;
  //TODO change directory to user & remove conifgDire
  public static string SAVE_DIRE = "user/userSave.tres";
  public static string CONFIG_DIRE = "user/userConfig.tres";

  public static AudioStreamPlayer Music;
  public static AudioStream backgroundMusicFile = GD.Load("res://sound/music/01.02. Minuet In G.mp3") as AudioStream;
  public static AudioStream levelMusicFile = GD.Load("res://sound/music/01.01. First Movement (Allegro Con Brio) From Symphony No. 5 In C Minor, Op. 67.mp3") as AudioStream;

  public override void _Ready() {
    Music = new AudioStreamPlayer() { Name = "Background", Stream = backgroundMusicFile, Bus = "Music", };
    AddChild(Music);
    Music.Play();
    Music.ProcessMode = ProcessModeEnum.Always;
  }

  //TODO (get current music time) slowly fade current music then slowly fade in the next track 
  public void MusicSmoothTransition() {
  }

  public override void _ExitTree() {
    //check first if file exists
    // FileAccess file = FileAccess.Open("log.txt", FileAccess.ModeFlags.ReadWrite);
    // file.SeekEnd();
    // file.StoreString("closing game\n");
    Input.SetCustomMouseCursor(null);
  }
}
