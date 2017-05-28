using UnityEngine;
using System.Collections;

//Contains global variables used to store information about the game state
public class GameStats : ScriptableObject
{
    public static float playerHealth = 0;
    public static float totalDamageTaken = 0;
    public static bool  isPlayerDead = false;
    public static int totalEnemiesKilled = 0;
    public static int totalLevelsWon = 0;
    public static int remainingEnemies = 0;
}
