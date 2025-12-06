using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class HighScoreIO
{
    // File path
    static readonly string PathFile =
        Path.Combine(Application.persistentDataPath, "highscores.json");

    // Loads existing table or creates a new empty one if missing/corrupt
    public static HighScoreTable LoadOrCreate()
    {
        try
        {
            if (!File.Exists(PathFile))
            {
                // No file yet = create empty table and save it
                var empty = new HighScoreTable { entries = new HighScoreEntry[0] };
                Save(empty);
                return empty;
            }

            string json = File.ReadAllText(PathFile);
            var tableLoaded = JsonUtility.FromJson<HighScoreTable>(json);

            if (tableLoaded == null || tableLoaded.entries == null)
            {
                Debug.LogWarning("[HighScoreIO] File exists but data is invalid. Resetting.");
                var empty = new HighScoreTable { entries = new HighScoreEntry[0] };
                Save(empty);
                return empty;
            }

            return tableLoaded;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"[HighScoreIO] Load failed: {ex.Message}. Resetting file.");
            var empty = new HighScoreTable { entries = new HighScoreEntry[0] };
            Save(empty);
            return empty;
        }
    }

    // Saves table back to JSON file
    public static void Save(HighScoreTable table)
    {
        try
        {
            string json = JsonUtility.ToJson(table, true);
            File.WriteAllText(PathFile, json);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[HighScoreIO] Save failed: {ex.Message}");
        }
    }

    // Always inserts the score and keeps only the top 'max' entries
    public static void Insert(HighScoreTable t, string name, int score, int max = 5)
    {
        var list = new List<HighScoreEntry>(t.entries);

        list.Add(new HighScoreEntry { name = name, score = score });

        // Sorts descending by score
        list.Sort((a, b) => b.score.CompareTo(a.score));

        // Keeps only top 'max'
        if (list.Count > max)
            list.RemoveRange(max, list.Count - max);

        t.entries = list.ToArray();
    }
}
