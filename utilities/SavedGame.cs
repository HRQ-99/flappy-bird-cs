using System.Collections.Generic;

using Godot;
using Godot.Collections;

public partial class SavedGame : Resource {
    [Export]public Array<int> attemptNumber = new();
    [Export]public Array<int> score = new();
}