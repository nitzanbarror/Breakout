using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton (course pattern, Session 3). Intentionally NOT DontDestroyOnLoad:
    // restart reloads the scene, and a fresh manager = a fresh game state.
    public static GameManager Instance { get; private set; }

    private const string BestScoreKey = "BestScore";

    [SerializeField] private int _startingLives = 3;
    [SerializeField] private float _restartLockout = 0.5f;

    public int Score { get; private set; }
    public int Lives { get; private set; }
    public int BestScore { get; private set; }
    public bool IsGameOver { get; private set; }

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

        Time.timeScale = 1f;
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
        if (!IsGameOver)
        {
            return;
        }

        // 0.5 s lockout so a panic key-press doesn't skip the score screen
        bool lockoutOver = Time.unscaledTime - _gameOverTime > _restartLockout;
        bool pressedRestart = Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0);

        if (lockoutOver && pressedRestart)
        {
            Restart();
        }
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

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}