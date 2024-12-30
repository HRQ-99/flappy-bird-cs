using Godot;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

public partial class Level : Node2D
{
    Node2D Bird;

    // Called when the node enters the scene tree for the first time.
    Node2D Pipes1;
    Node2D Pipes2;
    Node2D Pipes3;
    Node2D Pipes4;

    List<Node2D> Pipes = new(4);
    public override void _Ready()
    {
        Bird = GetNode<Node2D>("Bird");
        Pipes1 = GetNode<Node2D>("Pipes/Pipes1");
        Pipes2 = GetNode<Node2D>("Pipes/Pipes2");
        Pipes3 = GetNode<Node2D>("Pipes/Pipes3");
        Pipes4 = GetNode<Node2D>("Pipes/Pipes4");

        Pipes.Add(Pipes1);
        Pipes.Add(Pipes2);
        Pipes.Add(Pipes3);
        Pipes.Add(Pipes4);

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        foreach (var pipe in Pipes)
        {
            pipe.MoveLocalX(-10);
            if (pipe.Position.X <= 0)
                pipe.MoveLocalX(1000);

            if (CheckCollision(Bird, pipe))
            {
                GD.Print("Game Over");
            }
        }

    }

    public bool CheckCollision(Node2D bird, Node2D pipe)
    {
        Area2D birdHitbox = bird.GetChild<Area2D>(1);
        Area2D pipeHitbox = pipe.GetChild<Area2D>(1);

        return birdHitbox.OverlapsArea(pipeHitbox);
    }
}
