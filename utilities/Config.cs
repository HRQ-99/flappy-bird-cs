using Godot;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[Tool]
public partial class Config : Resource {

    public static readonly Dictionary<string, Vector2I> Resolutions = new() {
    { "1280x720", new Vector2I(1280, 720) },
    { "1920x1080", new Vector2I(1920,1080)},
    { "2560x1440", new Vector2I(2560, 1440)},
    { "3840x2160", new  Vector2I(3840, 2160)},
      };

    public static readonly ReadOnlyDictionary<string, DisplayServer.WindowMode> WindowModes = new(new Dictionary<string, DisplayServer.WindowMode>{
     {"Exclusive Fullscreen", DisplayServer.WindowMode.ExclusiveFullscreen},
     {"Fullscreen", DisplayServer.WindowMode.Fullscreen},
     {"Windowed", DisplayServer.WindowMode.Windowed},
     {"Maximised", DisplayServer.WindowMode.Maximized},
    });

    public static readonly Dictionary<string, string> InputKeys = new() {
    { "Flap","Flap" },
    { "Upkey","Up (UI)" },
    { "Downkey","Down (UI)" },
    { "Restart","Play Again" },
    { "GodMode","Toggle Godmode" },
  };

    public static List<string> InputkeyArray = new(InputKeys.Keys);

    public static Dictionary<string, Key> DefaultInputKeys = new() {
    { "Flap", Key.Up},
    { "Upkey",Key.Up},
    { "Downkey",Key.Down},
    { "Restart",Key.Space},
    { "GodMode",Key.F2},
  };

    [Export] public Vector2I GameResolution = DisplayServer.ScreenGetSize();
    [Export] public DisplayServer.WindowMode WindowMode = DisplayServer.WindowMode.ExclusiveFullscreen;
    [Export] public DisplayServer.VSyncMode Vsync = DisplayServer.VSyncMode.Disabled;
    [Export] public bool CustomCursor = true;
    [Export] public int MasterVolume = 100;
    [Export] public int MusicVolume = 100;
    [Export] public int EffectsVolume = 100;
    // [Export] public string VolumeOutputDevice;

    public Config() {
    }
}