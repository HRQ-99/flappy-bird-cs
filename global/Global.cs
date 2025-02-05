using Godot;

using System;

public partial class Global : Node {
  public static int GlobalAttemptNumber = 1;
  public static int GlobalScore = 0;
  //TODO change directory to user
  public static string SAVE_DIRE = "user/userSave.tres";
  public static string CONFIG_DIRE = "user/userConfig.tres";

  public override void _Ready() {
  }

  public override void _ExitTree() {
    //check first if file exists
    // FileAccess file = FileAccess.Open("log.txt", FileAccess.ModeFlags.ReadWrite);
    // file.SeekEnd();
    // file.StoreString("closing game\n");
    Input.SetCustomMouseCursor(null);
  }
}
