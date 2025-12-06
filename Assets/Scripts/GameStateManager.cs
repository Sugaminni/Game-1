using UnityEngine;

// Enum representing different game states
public enum GameState
{
    Intro,
    HighScores,
    Credits,
    Instructions,
    Game,
    EnterInitials
}

public class GameStateManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject introPanel;
    public GameObject highScoresPanel;
    public GameObject creditsPanel;
    public GameObject instructionsPanel;
    public GameObject enterInitialsPanel;
    public GameObject gameHUD;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip introMusic;
    public AudioClip gameMusic;

    public GameState State { get; private set; } = GameState.Intro;

    void Start()
    {
        SetState(GameState.Intro);
    }

    public void SetState(GameState next)
    {
        State = next;

        // Toggles panels
        if (introPanel) introPanel.SetActive(next == GameState.Intro);
        if (highScoresPanel) highScoresPanel.SetActive(next == GameState.HighScores);
        if (creditsPanel) creditsPanel.SetActive(next == GameState.Credits);
        if (instructionsPanel) instructionsPanel.SetActive(next == GameState.Instructions);
        if (enterInitialsPanel) enterInitialsPanel.SetActive(next == GameState.EnterInitials);
        if (gameHUD) gameHUD.SetActive(next == GameState.Game);

        // Pauses world unless actually playing
        Time.timeScale = (next == GameState.Game) ? 1f : 0f;

        // Manages cursor visibility
        if (next == GameState.Game)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Swaps music
        if (musicSource != null)
        {
            AudioClip clip = (next == GameState.Game) ? gameMusic : introMusic;
            if (clip != null && musicSource.clip != clip)
            {
                musicSource.clip = clip;
                musicSource.loop = true;
                musicSource.Play();
            }
        }
    }

    public void BtnEnterInitials()
    {
        SetState(GameState.EnterInitials);
    }

    // Button hooks for UI

    public void BtnStartGame()
    {
        // Tries to apply JSON start config (player + pickups)
        var loader = FindObjectOfType<StartConfigLoader>();
        if (loader != null)
        {
            loader.ApplyFromFile();
        }
        else
        {
            Debug.LogWarning("[GameStateManager] No StartConfigLoader found in scene.");
        }

        // Starts the game
        SetState(GameState.Game);
    }

    public void BtnShowHighScores() => SetState(GameState.HighScores);
    public void BtnShowCredits() => SetState(GameState.Credits);
    public void BtnShowInstructions() => SetState(GameState.Instructions);
    public void BtnBackToIntro() => SetState(GameState.Intro);

    public void BtnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
