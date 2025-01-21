using System;
using System.Collections.Generic;

using Godot;
public partial class Bird : CharacterBody2D {
	private CharacterBody2D birdCollision;
	[Export] private float fallSpeed = 300f;
	[Export] private float flySpeed = -3500f;
	[Export] public bool Invincible = false;

	public override void _Ready() {
		birdCollision = this;
	}

	public override void _PhysicsProcess(double delta) {
		Velocity = new Vector2(150, fallSpeed);

		if (Input.IsActionJustPressed("Flap")) {
			Velocity = new Vector2(0, flySpeed);
		}
		if (Input.IsActionJustPressed("GodMode")) {
			Invincible = !Invincible;
			GetTree().CurrentScene.GetNode<RichTextLabel>("UI/ScoreContainer/Godmode").Visible = Invincible;
		}

		if (MoveAndSlide() && !Invincible) {
			birdCollision.GetSlideCollision(0);
			var lastHit = birdCollision.GetLastSlideCollision();
			if (lastHit != null) {
				var collider = lastHit.GetCollider() as Node;
				if (collider.Name != "BackgroundBoundary") {
					SaveScore();
					GetTree().ChangeSceneToFile("scenes/EndScreen.tscn");
				}
			}
		}
	}

	public static void SaveScore() {
		SavedGame game = new();
		game.attemptNumber = 2;
		game.score = Global.GlobalScore;

		ResourceSaver.Save(game, "user/userSave.tres");
	}
}
