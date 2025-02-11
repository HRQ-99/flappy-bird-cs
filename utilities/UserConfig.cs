using Godot;
using System;
using System.Collections.Generic;

public partial class UserConfig : Node {

  public ConfigFile config = new();
  const string SETTINGS_PATH = "user/settings.ini";

  public enum Sections { display, audio, input }

  public enum Options {
    Resolution, WindowMode, VSync, CustomCursor,
    MasterVolume, MusicVolume, EffectsVolume,
  }

  public UserConfig() { config.Load(SETTINGS_PATH); }

  public override void _Ready() {
    if (!FileAccess.FileExists(SETTINGS_PATH)) {
      CreateDefaultConfig();
    }
    else {
      config.Load(SETTINGS_PATH);
      LoadUserConfig();
    }
  }

  public void CreateDefaultConfig() {
    // config.SetValue(Sections.display.ToString(), Options.Resolution.ToString(), "x");
    config.SetValue(Sections.display.ToString(), Options.WindowMode.ToString(), DisplayServer.WindowMode.ExclusiveFullscreen.ToString());
    config.SetValue(Sections.display.ToString(), Options.VSync.ToString(), false);
    config.SetValue(Sections.display.ToString(), Options.CustomCursor.ToString(), true);
    config.Save(SETTINGS_PATH);
  }

  public Dictionary<string, Variant> LoadDisplayConfig() {
    Dictionary<string, Variant> displaySettings = new();

    if (!config.HasSection(Sections.display.ToString())) { CreateDefaultConfig(); }

    foreach (string key in config.GetSectionKeys(Sections.display.ToString())) {
      displaySettings[key] = config.GetValue(Sections.display.ToString(), key);
    }
    return displaySettings;
  }


  public Dictionary<string, Variant> LoadAudioConfig() {
    Dictionary<string, Variant> audioSettings = new();

    if (!config.HasSection(Sections.audio.ToString())) { CreateDefaultConfig(); }

    foreach (string key in config.GetSectionKeys(Sections.audio.ToString())) {
      audioSettings[key] = config.GetValue(Sections.audio.ToString(), key);
    }
    return audioSettings;
  }

  public void SaveDisplayConfig(string key, Variant value) {
    config.SetValue(Sections.display.ToString(), key, value);
    config.Save(SETTINGS_PATH);
  }

  public void SaveAudioConfig(string key, Variant value) {
    config.SetValue(Sections.audio.ToString(), key, value);
    config.Save(SETTINGS_PATH);
  }

  public void LoadUserConfig() {
    Dictionary<string, Variant> displaySettings = LoadDisplayConfig();

    if (Enum.TryParse<DisplayServer.WindowMode>((string)displaySettings[Options.WindowMode.ToString()], true, out DisplayServer.WindowMode mode)) {
      DisplayServer.WindowSetMode(mode);
      if (mode != DisplayServer.WindowMode.ExclusiveFullscreen) {
        string resolution = (string)displaySettings[Options.Resolution.ToString()];
        int resX = int.Parse(resolution.Substring(0, resolution.IndexOf('x')));
        int resY = int.Parse(resolution.Substring(resolution.IndexOf('x') + 1));
        DisplayServer.WindowSetSize(new Vector2I(resX, resY));
      }
    }
    else {
      DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
    }

    Dictionary<string, Variant> audioSettings = LoadAudioConfig();
    // config;
  }
}