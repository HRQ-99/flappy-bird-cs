using Godot;
using System.Linq;

public partial class OptionsMenu : Control {
  public Config ConfigObj = new();
  public UserConfig UserConfigObj = new();
  private OptionButton ResolutionButton;

  VBoxContainer DisplayOptions;
  VBoxContainer AudioOptions;
  VBoxContainer InputOptions;

  Variant customCursor = ResourceLoader.Load("res://art/custom_cursor.png");

  public void ChangeResolution(int index) {
    string resolutionChosen = DisplayOptions.GetNode<OptionButton>("ResolutionContainer/ResolutionOptionButton").GetItemText(index);
    switch (index) {
      case 0:
        DisplayServer.WindowSetSize(Config.Resolutions[resolutionChosen]);
        ConfigObj.GameResolution = Config.Resolutions[resolutionChosen];
        break;
      case 1:
        DisplayServer.WindowSetSize(Config.Resolutions[resolutionChosen]);
        ConfigObj.GameResolution = Config.Resolutions[resolutionChosen];
        break;
      case 2:
        DisplayServer.WindowSetSize(Config.Resolutions[resolutionChosen]);
        ConfigObj.GameResolution = Config.Resolutions[resolutionChosen];
        break;
      case 3:
        DisplayServer.WindowSetSize(Config.Resolutions[resolutionChosen]);
        ConfigObj.GameResolution = Config.Resolutions[resolutionChosen];
        break;
    }
    UserConfigObj.SaveDisplayConfig(UserConfig.Options.Resolution.ToString(), resolutionChosen);
  }

  public void ChangeWindowMode(int index) {
    string WindowModeChosen = DisplayOptions.GetNode<OptionButton>("WindowModeContainer/WindowModeOptionButton").GetItemText(index);
    DisplayOptions.GetNode<HSplitContainer>("ResolutionContainer").Visible = false;
    switch (index) {
      case 0:
        DisplayServer.WindowSetMode(Config.WindowModes[WindowModeChosen]);
        ConfigObj.WindowMode = Config.WindowModes[WindowModeChosen];
        break;
      case 1:
        DisplayServer.WindowSetMode(Config.WindowModes[WindowModeChosen]);
        ConfigObj.WindowMode = Config.WindowModes[WindowModeChosen];
        break;
      case 2:
        DisplayServer.WindowSetMode(Config.WindowModes[WindowModeChosen]);
        ConfigObj.WindowMode = Config.WindowModes[WindowModeChosen];
        DisplayOptions.GetNode<HSplitContainer>("ResolutionContainer").Visible = true;
        break;
      case 3:
        DisplayServer.WindowSetMode(Config.WindowModes[WindowModeChosen]);
        ConfigObj.WindowMode = Config.WindowModes[WindowModeChosen];
        break;
    }
    UserConfigObj.SaveDisplayConfig(UserConfig.Options.WindowMode.ToString(), WindowModeChosen.Replace(" ", ""));
  }

  public void ToggleVSync(bool enabledVSync) {
    if (enabledVSync) {
      DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);
      ConfigObj.Vsync = DisplayServer.VSyncMode.Enabled;
    }
    else {
      DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
      ConfigObj.Vsync = DisplayServer.VSyncMode.Disabled;
    }
    UserConfigObj.SaveDisplayConfig(UserConfig.Options.VSync.ToString(), enabledVSync);
  }

  public void ToggleCustomCursor(bool enabledCustomCursor) {
    if (enabledCustomCursor) {
      DisplayServer.CursorSetCustomImage((Resource)customCursor);
      // ConfigObj.CustomCursor = enabledCustomCursor;
    }
    else {
      DisplayServer.CursorSetCustomImage(null);
      // ConfigObj.CustomCursor = enabledCustomCursor;
    }
    UserConfigObj.SaveDisplayConfig(UserConfig.Options.CustomCursor.ToString(), enabledCustomCursor);
  }

  public void RestoreDefaultConfig() {
    SetCurrentSelected();
    // userConfig.creat
  }

  public void SetCurrentSelected() {
    var reX = UserConfigObj.LoadDisplayConfig();
    var keys = reX.Keys.ToArray();
    // GD.Print(keys);
    var option = DisplayOptions.GetNode<OptionButton>("ResolutionContainer/ResolutionOptionButton");
    for (int i = 0; i < option.ItemCount; i++) {
      if (option.GetItemText(i).ToLower().Equals(reX[UserConfig.Options.Resolution.ToString()].ToString())) {
        GD.Print("selected:" + i);
        option.Select(i);
      }
    }
    DisplayOptions.GetNode<CheckBox>("CustomCursorContainer/CustomCursorOptionButton").ButtonPressed = (bool)reX[UserConfig.Options.CustomCursor.ToString()];
    ToggleCustomCursor((bool)reX[UserConfig.Options.CustomCursor.ToString()]);
  }

  public override void _Ready() {
    DisplayOptions = GetNode<VBoxContainer>("TabContainer/Display/DisplayVBox");
    AudioOptions = GetNode<VBoxContainer>("TabContainer/Audio/AudioVBox");
    InputOptions = GetNode<VBoxContainer>("TabContainer/Input/InputVBox");
    SetCurrentSelected();
    // string key;
    UserConfig obj = new();
    obj.LoadDisplayConfig();
  }

  private void GoBackToTitleMenu() {
    GetTree().ChangeSceneToFile("scenes/StartScreen.tscn");
  }
  public override void _Process(double delta) {
    if (Input.IsActionJustPressed("Escapekey")) {
      GetTree().ChangeSceneToFile("scenes/StartScreen.tscn");
    }
  }
}
