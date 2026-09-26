using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Breakout/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("Ball")]
    public float ballSpeed = 7f;
    public float bounceArcDeg = 60f;
    [Range(0f, 1f)] public float minVerticalFraction = 0.2f;

    [Header("Paddle")]
    public float paddleSpeed = 9f;
    public float horizontalLimit = 4.6f;
    public float paddleWideScale = 2.6f;
    public float wideDuration = 8f;

    [Header("Wall - the twist")]
    public float descentSpeed = 0.15f;
    public float rowInterval = 11f;
    public float rowIntervalDecay = 0.97f;
    public float rowIntervalMin = 5f;

    [Header("Power-ups")]
    [Range(0f, 1f)] public float powerUpChance = 0.14f;
    public float powerUpFallSpeed = 3f;
}