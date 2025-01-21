using Godot;

using System;
using System.Collections.Generic;
using System.Linq;

public partial class Level : Node2D {
    bool GamePaused = false;
    int score = 0;
    private int movePipeDistanceX = 750;
    int movePipeDistanceY = 320;
    int gapBetweenPipeY = 600;
    int nextPipeLocationX = 700;
    int despawnPipeDistance = -100;

    CharacterBody2D Bird;
    StaticBody2D BackgroundSprite;

    Node2D Pipes_set;
    PackedScene pipeScene = (PackedScene)GD.Load("scenes/Pipe.tscn");
    // Dictionary<Node2D, Vector2> pipeLocations = new(4);

    public override void _Ready() {
        Bird = GetNode<CharacterBody2D>("Bird");
        Pipes_set = GetNode<Node2D>("Pipes_set");
        for (int i = 0; i <= 4; i++) {
            Node2D pipes = makePipePair();
            Pipes_set.AddChild(pipes);
            nextPipeLocationX += movePipeDistanceX;
        }
        BackgroundSprite = GetNode<StaticBody2D>("BackgroundBoundary");
    }
    Node2D firstPipe;
    public override void _Process(double delta) {
        if (BackgroundSprite.Position.X - Bird.GlobalPosition.X < -1000) {
            BackgroundSprite.MoveLocalX(2000);
        }
        foreach (Node2D pipe in Pipes_set.GetChildren().Cast<Node2D>()) {
            if (Bird.GlobalPosition.X - pipe.GlobalPosition.X > 1500) {
                // Pipes_set.RemoveChild(pipe);
                // pipe.QueueFree();
            }
        }
    }

    private static int RandomInt(float min, float max) {
        Random random = new();
        return random.Next((int)min, (int)max);
    }

    public Node2D makePipePair() {
        StaticBody2D topPipe = (StaticBody2D)pipeScene.Instantiate();
        topPipe.RotationDegrees = 180;
        topPipe.Scale = new Vector2(7, 7);
        topPipe.Position = new Vector2(nextPipeLocationX, RandomInt(movePipeDistanceY - 200, movePipeDistanceY + 200));

        StaticBody2D bottomPipe = (StaticBody2D)pipeScene.Instantiate();
        bottomPipe.Scale = new Vector2(7, 7);
        bottomPipe.Position = new Vector2(nextPipeLocationX, 900);

        Node2D pipePair = new Node2D();
        pipePair.Name = "Pipes";
        pipePair.AddChild(topPipe);
        pipePair.AddChild(bottomPipe);
        pipePair.Position = new Vector2(0, 300);
        return pipePair;
    }
    public override void _Input(InputEvent @event) {
        if (Input.IsActionJustPressed("Escapekey") && !GamePaused) {
            GetTree().Paused = true;
            GetNode<Control>("UI/PausedScreen").Visible = true;
            GamePaused = true;
        }
        else if (Input.IsActionJustPressed("Escapekey") && GamePaused) {
            GetTree().Paused = false;
            GetNode<Control>("UI/PausedScreen").Visible = false;
            GamePaused = false;
        }

    }

    public void PressedResumeButton() {
        GetTree().Paused = false;
        GetNode<Control>("UI/PausedScreen").Visible = false;
        GamePaused = false;
    }
    public void PressedExitButton() {
        GetTree().Quit();
    }

    public void ScoreManager() {
        score++;
        GetNode<RichTextLabel>("UI/ScoreContainer/Score").Text = "Score : " + score.ToString();
        Global.GlobalScore = this.score;
    }
}
