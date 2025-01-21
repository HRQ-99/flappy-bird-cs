using Godot;
[Tool]
public partial class GameSaver : Node {
    public void OnSave() {
        SavedGame savedGameObj = new();
        ResourceLoader.Load("user / userSave.tres");
    }
    public void OnLoad() {

    }
}