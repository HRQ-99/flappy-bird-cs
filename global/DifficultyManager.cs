using Godot;

public partial class DifficultyManager : Node {

  public static int DifficultyStage;
  public static float[] DifficultyIncreaseTriggerScores = { 50, 100, 125, 150, 200 };

  public static float[] BirdMoveSpeed = { 150f, 165f, 180f, 200f, 250f };
  public static float[] PipeGap = { 800f, 775f, 760f, 750f, 810f };
  public static float[] PowerSpawnTime = { 90, 85, 80, 75, 72 };
  public static float[] PipeMinimumGap = { 55, 50, 48, 45, 42 };

  public override void _Ready() {
    DifficultyStage = 0;
  }

  public static void IncreaseDifficulty() { DifficultyStage++; }

}