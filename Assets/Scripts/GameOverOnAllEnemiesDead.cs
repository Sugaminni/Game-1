using UnityEngine;

public class GameOverOnAllEnemiesDead : MonoBehaviour
{
    private bool gameOverTriggered = false;

    void Update()
    {
        // Only checks during actual gameplay
        var gsm = Object.FindFirstObjectByType<GameStateManager>();
        if (gsm == null || gsm.State != GameState.Game)
            return;

        if (gameOverTriggered)
            return;

        // Finds all enemies of type EnemyBase
        EnemyBase[] enemies = Object.FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);

        // Game over if none remain
        if (enemies.Length == 0)
        {
            gameOverTriggered = true;

            int finalScore = 100; 

            EnterInitialsPanel.SetPendingScore(finalScore);
            gsm.SetState(GameState.EnterInitials);
        }
    }
}
