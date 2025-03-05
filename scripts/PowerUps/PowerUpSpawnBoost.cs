using Godot;

public partial class PowerUpSpawnBoost : Area2D, IPowerUps {

    float SpawnTimeMultiplier = .75f;

    public void PowerActivate(Node2D bodyEntered) {
        if (bodyEntered.IsInGroup("Bird")) {
            GetTree().CurrentScene.GetNode<Timer>("PowerUpSpawnTimer").WaitTime *= SpawnTimeMultiplier;

            SetDeferred("monitoring", false);
            Visible = false;
            GetNode<Timer>("Timer").Start();

            GetNode<AudioStreamPlayer2D>("SoundEffect").Play();
        }
    }

    public void MusicFadeOut(Node2D bodyEntered) {
        if (bodyEntered.IsInGroup("Bird")) {
            Tween musicFade = CreateTween();
            musicFade.TweenProperty(GetNode<AudioStreamPlayer>("/root/Global/Background"), "volume_db", -10, 4);
            musicFade.Finished += MusicFadeIn;
        }
    }

    public void MusicFadeIn() {
        Tween musicFade = CreateTween();
        musicFade.TweenProperty(GetNode<AudioStreamPlayer>("/root/Global/Background"), "volume_db", 0, 1.5);
    }

    public void PowerExpired() {
        GetTree().CurrentScene.GetNode<Timer>("PowerUpSpawnTimer").WaitTime /= SpawnTimeMultiplier;

        QueueFree();
    }
}