using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public GameState CurrentState { get; private set; }
    public int Score { get; private set; }

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    private void Start()
    {
        // Initialize the game state
        ChangeState(GameState.MainMenu);
    }

    private void Update()
    {
        // Example input handling for pausing the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Playing)
            {
                PauseGame();
            }
            else if (CurrentState == GameState.Paused)
            {
                ResumeGame();
            }
        }
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        // Handle state-specific logic
        switch (newState)
        {
            case GameState.MainMenu:
                Debug.Log("Game is in Main Menu.");
                break;
            case GameState.Playing:
                Debug.Log("Game is now Playing.");
                break;
            case GameState.Paused:
                Debug.Log("Game is Paused.");
                Time.timeScale = 0f; // Freeze game time
                break;
            case GameState.GameOver:
                Debug.Log("Game Over.");
                break;
        }
    }

    public void StartGame()
    {
        Score = 0; // Reset score
        ChangeState(GameState.Playing);
    }

    public void PauseGame()
    {
        ChangeState(GameState.Paused);
    }

    public void ResumeGame()
    {
        ChangeState(GameState.Playing);
        Time.timeScale = 1f; // Resume game time
    }

    public void EndGame()
    {
        ChangeState(GameState.GameOver);
    }

    public void AddScore(int points)
    {
        Score += points;
        Debug.Log($"Score: {Score}");
    }
}
