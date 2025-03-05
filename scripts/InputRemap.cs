using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class InputRemap : VBoxContainer {
  PackedScene InputRemapScene = GD.Load<PackedScene>("scenes/RemapKeys.tscn");

  public override void _Ready() {
    foreach (KeyValuePair<string, string> keyPair in Config.InputKeys) {
      var remapRow = InputRemapScene.Instantiate();
      AddChild(remapRow);
      remapRow.GetNodeOrNull<Label>("KeyLabel").Text = keyPair.Value;
      Godot.Collections.Array<InputEvent> eventName = InputMap.ActionGetEvents(keyPair.Key);
      if (eventName.Count() > 0) {
          GD.Print( eventName[0].AsText().TrimSuffix(" (Physical)"));
        remapRow.GetNodeOrNull<Button>("KeyButton").Text = eventName[0].AsText().TrimSuffix(" (Physical)");
      }
      remapRow.GetNode<Button>("KeyButton").Pressed += RemapKey;
    }
  }

  public override void _Process(double delta) {
  }

  void RemapKey() {
    GD.Print("pressed");
  }

}