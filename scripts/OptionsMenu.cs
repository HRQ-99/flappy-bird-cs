using Godot;
using System.Collections.Generic;

public partial class OptionsMenu : Control {
  public Config ConfigObj = new();
  public UserConfig UserConfigObj = new();
  private OptionButton _resolutionButton;

  VBoxContainer _displayOptions;
  VBoxContainer _audioOptions;
  VBoxContainer _inputOptions;

  Variant _customCursor = ResourceLoader.Load("res://art/custom_cursor.png");

  enum AudioBuses { Master, Music, Effects }

  public void ChangeResolution(int index) {
    string resolutionChosen = _displayOptions.GetNode<OptionButton>("ResolutionContainer/ResolutionOptionButton").GetItemText(index);
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
    string windowModeChosen = _displayOptions.GetNode<OptionButton>("WindowModeContainer/WindowModeOptionButton").GetItemText(index);
    _displayOptions.GetNode<HSplitContainer>("ResolutionContainer").Visible = false;
    switch (index) {
      case 0:
        DisplayServer.WindowSetMode(Config.WindowModes[windowModeChosen]);
        ConfigObj.WindowMode = Config.WindowModes[windowModeChosen];
        break;
      case 1:
        DisplayServer.WindowSetMode(Config.WindowModes[windowModeChosen]);
        ConfigObj.WindowMode = Config.WindowModes[windowModeChosen];
        break;
      case 2:
        DisplayServer.WindowSetMode(Config.WindowModes[windowModeChosen]);
        ConfigObj.WindowMode = Config.WindowModes[windowModeChosen];
        _displayOptions.GetNode<HSplitContainer>("ResolutionContainer").Visible = true;
        break;
      case 3:
        DisplayServer.WindowSetMode(Config.WindowModes[windowModeChosen]);
        ConfigObj.WindowMode = Config.WindowModes[windowModeChosen];
        break;
    }
    UserConfigObj.SaveDisplayConfig(UserConfig.Options.WindowMode.ToString(), windowModeChosen.Replace(" ", ""));
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
      DisplayServer.CursorSetCustomImage((Resource)_customCursor);
    }
    else {
      DisplayServer.CursorSetCustomImage(null);
    }
    UserConfigObj.SaveDisplayConfig(UserConfig.Options.CustomCursor.ToString(), enabledCustomCursor);
  }

  private void ChangeMasterVolume(bool valueChanged) {
    if (valueChanged) {
      float sliderValue = (float)_audioOptions.GetNode<HSlider>("MasterVolumeContainer/MasterVolumeSlider").Value / 100;
      int newVolume = (int)Mathf.LinearToDb(sliderValue);
      UserConfigObj.SaveAudioConfig(UserConfig.Options.MasterVolume.ToString(), sliderValue);
      AudioServer.SetBusVolumeDb((int)AudioBuses.Master, newVolume);
    }
  }

  private void ChangeMusicVolume(bool valueChanged) {
    if (valueChanged) {
      float sliderValue = (float)_audioOptions.GetNode<HSlider>("MusicVolumeContainer/MusicVolumeSlider").Value / 100;
      int newVolume = (int)Mathf.LinearToDb(sliderValue);
      UserConfigObj.SaveAudioConfig(UserConfig.Options.MusicVolume.ToString(), sliderValue);
      AudioServer.SetBusVolumeDb((int)AudioBuses.Music, newVolume);
    }
  }

  private void ChangeEffectsVolume(bool valueChanged) {
    if (valueChanged) {
      float sliderValue = (float)_audioOptions.GetNode<HSlider>("EffectsVolumeContainer/EffectsVolumeSlider").Value / 100;
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
    OptionButton option = _displayOptions.GetNode<OptionButton>("ResolutionContainer/ResolutionOptionButton");
    for (int i = 0; i < option.ItemCount; i++) {
      if (option.GetItemText(i).ToLower().Equals(displaySettings[UserConfig.Options.Resolution.ToString()].ToString())) {
        option.Select(i);
      }
    }
    _displayOptions.GetNode<CheckBox>("V-SyncContainer/VsyncOptionButton").ButtonPressed = (bool)displaySettings[UserConfig.Options.VSync.ToString()];
    ToggleVSync((bool)displaySettings[UserConfig.Options.VSync.ToString()]);

    _displayOptions.GetNode<CheckBox>("CustomCursorContainer/CustomCursorOptionButton").ButtonPressed = (bool)displaySettings[UserConfig.Options.CustomCursor.ToString()];
    ToggleCustomCursor((bool)displaySettings[UserConfig.Options.CustomCursor.ToString()]);

    Dictionary<string, Variant> audioSettings = UserConfigObj.LoadAudioConfig();
    _audioOptions.GetNode<HSlider>("MasterVolumeContainer/MasterVolumeSlider").Value = (double)audioSettings[UserConfig.Options.MasterVolume.ToString()] * 100;
    _audioOptions.GetNode<HSlider>("MusicVolumeContainer/MusicVolumeSlider").Value = (double)audioSettings[UserConfig.Options.MusicVolume.ToString()] * 100;
    _audioOptions.GetNode<HSlider>("EffectsVolumeContainer/EffectsVolumeSlider").Value = (double)audioSettings[UserConfig.Options.EffectsVolume.ToString()] * 100;

    AudioServer.SetBusVolumeDb((int)AudioBuses.Master, Mathf.LinearToDb((float)audioSettings[UserConfig.Options.MasterVolume.ToString()]));
    AudioServer.SetBusVolumeDb((int)AudioBuses.Music, Mathf.LinearToDb((float)audioSettings[UserConfig.Options.MusicVolume.ToString()]));
    AudioServer.SetBusVolumeDb((int)AudioBuses.Effects, Mathf.LinearToDb((float)audioSettings[UserConfig.Options.EffectsVolume.ToString()]));
  }

  public override void _Ready() {
    _displayOptions = GetNode<VBoxContainer>("TabContainer/Display/DisplayVBox");
    _audioOptions = GetNode<VBoxContainer>("TabContainer/Audio/AudioVBox");
    _inputOptions = GetNode<VBoxContainer>("TabContainer/Input/InputVBox");
    SetCurrentSelected();
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