using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class HighScoresPanel : MonoBehaviour
{
    public Text scoresText;

    void OnEnable()
    {
        if (scoresText == null)
            return;

        var table = HighScoreIO.LoadOrCreate();

        // Sorts descending just in case file was edited
        System.Array.Sort(table.entries,
            (a, b) => b.score.CompareTo(a.score));

        var sb = new StringBuilder();

        // Shows up to 5 entries, fill empty slots with ---
        for (int i = 0; i < 5; i++)
        {
            if (i < table.entries.Length)
            {
                var e = table.entries[i];
                sb.AppendLine($"{i + 1}. {e.name} - {e.score}");
            }
            else
            {
                sb.AppendLine($"{i + 1}. ---");
            }
        }

        scoresText.text = sb.ToString();
    }
}
