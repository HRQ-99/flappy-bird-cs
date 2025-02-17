using Godot;
using System.Collections.Generic;

public partial class OptionsMenu : Control {
  public Config ConfigObj = new();
  public UserConfig UserConfigObj = new();
  private OptionButton ResolutionButton;

  VBoxContainer DisplayOptions;
  VBoxContainer AudioOptions;
  VBoxContainer InputOptions;

  Variant customCursor = ResourceLoader.Load("res://art/custom_cursor.png");

  enum AudioBuses { Master, Music, Effects }

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
    }
    else {
      DisplayServer.CursorSetCustomImage(null);
    }
    UserConfigObj.SaveDisplayConfig(UserConfig.Options.CustomCursor.ToString(), enabledCustomCursor);
  }

  private void ChangeMasterVolume(bool valueChanged) {
    if (valueChanged) {
      float sliderValue = (float)AudioOptions.GetNode<HSlider>("MasterVolumeContainer/MasterVolumeSlider").Value / 100;
      int newVolume = (int)Mathf.LinearToDb(sliderValue);
      UserConfigObj.SaveAudioConfig(UserConfig.Options.MasterVolume.ToString(), sliderValue);
      AudioServer.SetBusVolumeDb((int)AudioBuses.Master, newVolume);
    }
  }

  private void ChangeMusicVolume(bool valueChanged) {
    if (valueChanged) {
      float sliderValue = (float)AudioOptions.GetNode<HSlider>("MusicVolumeContainer/MusicVolumeSlider").Value / 100;
      int newVolume = (int)Mathf.LinearToDb(sliderValue);
      UserConfigObj.SaveAudioConfig(UserConfig.Options.MusicVolume.ToString(), sliderValue);
      AudioServer.SetBusVolumeDb((int)AudioBuses.Music, newVolume);
    }
  }

  private void ChangeEffectsVolume(bool valueChanged) {
    if (valueChanged) {
      float sliderValue = (float)AudioOptions.GetNode<HSlider>("EffectsVolumeContainer/EffectsVolumeSlider").Value / 100;
      int newVolume = (int)Mathf.LinearToDb(sliderValue);
      UserConfigObj.SaveAudioConfig(UserConfig.Options.EffectsVolume.ToString(), sliderValue);
      AudioServer.SetBusVolumeDb((int)AudioBuses.Effects, newVolume);
    }
  }

  public void RestoreDefaultConfig() {
    //TODO implement this
    // SetCurrentSelected();
    // userConfig.creat
  }

  //if resolution was not found problem comes and others are not set
  public void SetCurrentSelected() {
    Dictionary<string, Variant> displaySettings = UserConfigObj.LoadDisplayConfig();
    OptionButton option = DisplayOptions.GetNode<OptionButton>("ResolutionContainer/ResolutionOptionButton");
    for (int i = 0; i < option.ItemCount; i++) {
      if (option.GetItemText(i).ToLower().Equals(displaySettings[UserConfig.Options.Resolution.ToString()].ToString())) {
        option.Select(i);
      }
    }
    DisplayOptions.GetNode<CheckBox>("V-SyncContainer/VsyncOptionButton").ButtonPressed = (bool)displaySettings[UserConfig.Options.VSync.ToString()];
    ToggleVSync((bool)displaySettings[UserConfig.Options.VSync.ToString()]);

    DisplayOptions.GetNode<CheckBox>("CustomCursorContainer/CustomCursorOptionButton").ButtonPressed = (bool)displaySettings[UserConfig.Options.CustomCursor.ToString()];
    ToggleCustomCursor((bool)displaySettings[UserConfig.Options.CustomCursor.ToString()]);

    Dictionary<string, Variant> AudioSettings = UserConfigObj.LoadAudioConfig();
    AudioOptions.GetNode<HSlider>("MasterVolumeContainer/MasterVolumeSlider").Value = (double)AudioSettings[UserConfig.Options.MasterVolume.ToString()] * 100;
    AudioOptions.GetNode<HSlider>("MusicVolumeContainer/MusicVolumeSlider").Value = (double)AudioSettings[UserConfig.Options.MusicVolume.ToString()] * 100;
    AudioOptions.GetNode<HSlider>("EffectsVolumeContainer/EffectsVolumeSlider").Value = (double)AudioSettings[UserConfig.Options.EffectsVolume.ToString()] * 100;

    AudioServer.SetBusVolumeDb((int)AudioBuses.Master, Mathf.LinearToDb((float)AudioSettings[UserConfig.Options.MasterVolume.ToString()]));
    AudioServer.SetBusVolumeDb((int)AudioBuses.Music, Mathf.LinearToDb((float)AudioSettings[UserConfig.Options.MusicVolume.ToString()]));
    AudioServer.SetBusVolumeDb((int)AudioBuses.Effects, Mathf.LinearToDb((float)AudioSettings[UserConfig.Options.EffectsVolume.ToString()]));
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
