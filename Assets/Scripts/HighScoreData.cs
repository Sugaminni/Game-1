using System;

// Class representing a single high score entry
public class HighScoreEntry
{
    public string name;
    public int score;
}

// Wrapper class for high score table
[Serializable]public class HighScoreTable
{
    // Array so JsonUtility can serialize it easily
    public HighScoreEntry[] entries = new HighScoreEntry[0];
}
