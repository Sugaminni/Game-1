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

        if (table.entries != null && table.entries.Length > 0)
        {
            Debug.Log($"[EnterInitialsPanel] Top after insert: {table.entries[0].name} - {table.entries[0].score}");
        }
        else
        {
            Debug.Log("[EnterInitialsPanel] Table entries empty after insert.");
        }

        HighScoreIO.Save(table);

        var gsm = Object.FindFirstObjectByType<GameStateManager>();
        if (gsm != null)
            gsm.SetState(GameState.Intro);
    }

    // Called by Back button
    public void OnBack()
    {
        var gsm = Object.FindFirstObjectByType<GameStateManager>();
        if (gsm != null)
            gsm.SetState(GameState.Intro);
        else
            Debug.LogWarning("[EnterInitialsPanel] No GameStateManager found.");
    }
}
