using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton (course pattern, Session 3). Intentionally NOT DontDestroyOnLoad:
    // going back to the menu reloads the scene, and a fresh manager = a fresh game state.
    public static GameManager Instance { get; private set; }

    private const string BestScoreKey = "BestScore";

    [SerializeField] private int _startingLives = 3;
    [SerializeField] private float _restartLockout = 0.5f;

    public int Score { get; private set; }
    public int Lives { get; private set; }
    public int BestScore { get; private set; }
    public bool IsStarted { get; private set; }
    public bool IsGameOver { get; private set; }

    public UnityEvent OnGameStarted;
    public UnityEvent OnScoreChanged;
    public UnityEvent OnLivesChanged;
    public UnityEvent OnGameOver;

    private float _gameOverTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // the game boots into the main menu: time is frozen until START is pressed
        Time.timeScale = 0f;

        #if UNITY_ANDROID || UNITY_IOS
        // mobile does not default to 60 fps (Session 7)
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        #endif
        Lives = _startingLives;
        BestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
    }

    private void Update()
    {
        // Esc on PC (= the back button on Android) quits the game.
        // Note: Application.Quit does nothing inside the editor - that's normal.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (!IsGameOver)
        {
            return;
        }

        // 0.5 s lockout so a panic key-press doesn't skip the score screen
        bool lockoutOver = Time.unscaledTime - _gameOverTime > _restartLockout;
        bool pressedContinue = Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0);

        if (lockoutOver && pressedContinue)
        {
            BackToMenu();
        }
    }

    // called by the START button on the main menu
    public void StartGame()
    {
        if (IsStarted)
        {
            return;
        }

        IsStarted = true;
        Time.timeScale = 1f;
        OnGameStarted.Invoke();
    }

    // called by the EXIT button on the main menu
    public void QuitGame()
    {
        Application.Quit();
    }

    public void AddScore(int amount)
    {
        if (IsGameOver)
        {
            return;
        }

        Score += amount;
        OnScoreChanged.Invoke();
    }

    public void LoseLife()
    {
        if (IsGameOver)
        {
            return;
        }

        Lives--;
        OnLivesChanged.Invoke();

        if (Lives <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameOver = true;
        _gameOverTime = Time.unscaledTime;
        Time.timeScale = 0f;
        AudioManager.Instance.PlayGameOver();

        if (Score > BestScore)
        {
            BestScore = Score;
            PlayerPrefs.SetInt(BestScoreKey, BestScore);
            PlayerPrefs.Save();
        }

        OnGameOver.Invoke();
    }

    private void BackToMenu()
    {
        // reloading the scene brings back the main menu with the updated best score
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}