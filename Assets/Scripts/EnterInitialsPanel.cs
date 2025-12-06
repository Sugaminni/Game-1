using UnityEngine;
using UnityEngine.UI;

public class EnterInitialsPanel : MonoBehaviour
{
    public InputField inputField;

    // Score from the last run
    public static int PendingScore;

    public static void SetPendingScore(int score)
    {
        PendingScore = score;
    }

    // Called by Submit button
    public void OnSubmit()
    {
        string name = inputField != null ? inputField.text : "";

        if (string.IsNullOrWhiteSpace(name))
            name = "AAA"; // fallback

        var table = HighScoreIO.LoadOrCreate();
        HighScoreIO.Insert(table, name, PendingScore);
        HighScoreIO.Save(table);

        // Back to Intro
        FindObjectOfType<GameStateManager>().SetState(GameState.Intro);
    }

    // Called by Back button
    public void OnBack()
    {
        FindObjectOfType<GameStateManager>().SetState(GameState.Intro);
    }
}
