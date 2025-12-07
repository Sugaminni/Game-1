using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class HighScoreEntry
{
    public string name;
    public int score;
}

[System.Serializable]
public class HighScoreTable
{
    public HighScoreEntry[] entries = new HighScoreEntry[0];
}

public static class HighScoreIO
{
    static readonly string PathFile =
        System.IO.Path.Combine(Application.persistentDataPath, "highscores.json");

    // Loads the high score table from file or creates a new one if none exists
    public static HighScoreTable LoadOrCreate()
    {
        try
        {
            if (!File.Exists(PathFile))
            {
                Debug.Log($"[HighScoreIO] No file, creating new at {PathFile}");
                var empty = new HighScoreTable { entries = new HighScoreEntry[0] };
                Save(empty);
                return empty;
            }

            string json = File.ReadAllText(PathFile);
            var table = JsonUtility.FromJson<HighScoreTable>(json);

            if (table == null || table.entries == null)
            {
                Debug.LogWarning("[HighScoreIO] Parsed null/invalid table, resetting.");
                table = new HighScoreTable { entries = new HighScoreEntry[0] };
                Save(table);
            }

            return table;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"[HighScoreIO] Load failed: {ex.Message}. Resetting file.");
            var empty = new HighScoreTable { entries = new HighScoreEntry[0] };
            Save(empty);
            return empty;
        }
    }

    // Saves the high score table to file
    public static void Save(HighScoreTable table)
    {
        try
        {
            string json = JsonUtility.ToJson(table, true);
            File.WriteAllText(PathFile, json);
            Debug.Log($"[HighScoreIO] Saved {table.entries.Length} entries to {PathFile}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[HighScoreIO] Save failed: {ex.Message}");
        }
    }

    // Inserts a new high score entry, keeping the list sorted and capped at 'max' entries
    public static void Insert(HighScoreTable t, string name, int score, int max = 5)
    {
        if (t.entries == null)
            t.entries = new HighScoreEntry[0];

        var list = new List<HighScoreEntry>(t.entries);
        list.Add(new HighScoreEntry { name = name, score = score });

        list.Sort((a, b) => b.score.CompareTo(a.score));

        if (list.Count > max)
            list.RemoveRange(max, list.Count - max);

        t.entries = list.ToArray();
    }
}
