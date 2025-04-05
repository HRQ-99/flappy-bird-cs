using Godot;

public partial class SpeedBoostPowerUp : Area2D, IPowerUps {

    BoostTrail birdBoostTrail;
    [Signal] public delegate void ApplyCameraEffectEventHandler();

    public void PowerActivate(Node2D bodyEntered) {
        if (bodyEntered.IsInGroup("Bird")) {
            Bird.SpeedMultiplier = 5;
            Bird.GravityMultiplier = 0.01f;
            Bird.Invincible = true;

            ApplyCameraEffect += ((Bird)bodyEntered).SpeedBoostCameraEffect;
            EmitSignal(SignalName.ApplyCameraEffect);
            birdBoostTrail = BoostTrail.CreateTrail();
            bodyEntered.AddChild(birdBoostTrail);

            SetDeferred("monitoring", false);
            Visible = false;
            GetNode<Timer>("Timer").Start();

            GetNode<AudioStreamPlayer2D>("SoundEffect").Play();
        }
    }

    public void MusicFadeOut(Node2D bodyEntered) {
        if (bodyEntered.IsInGroup("Bird")) {
            Tween musicFade = CreateTween();
            musicFade.TweenProperty(GetNode<AudioStreamPlayer>("/root/Global/Background"), "volume_db", -10, 2);
            musicFade.Finished += MusicFadeIn;
        }
    }

    public void MusicFadeIn() {
        Tween musicFade = CreateTween();
        musicFade.TweenProperty(GetNode<AudioStreamPlayer>("/root/Global/Background"), "volume_db", 0, 1.5);
    }

    //TODO old power's expiring overwrites current one's buff
    public void PowerExpired() {
        Bird.SpeedMultiplier = 1;
        Bird.GravityMultiplier = 1;
        Bird.Invincible = false;
        GetNode<CharacterBody2D>("/root/Level/Bird").RemoveChild(birdBoostTrail);

        QueueFree();
    }
}