using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private TMP_Text _menuBestText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMP_Text _gameOverText;

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;

        // observer pattern: the UI listens for changes, it never polls
        gameManager.OnGameStarted.AddListener(HideMenu);
        gameManager.OnScoreChanged.AddListener(RefreshScore);
        gameManager.OnLivesChanged.AddListener(RefreshLives);
        gameManager.OnGameOver.AddListener(ShowGameOver);

        RefreshScore();
        RefreshLives();
        _menuBestText.text = $"BEST {gameManager.BestScore}";
        _menuPanel.SetActive(true);
        _gameOverPanel.SetActive(false);
    }

    private void HideMenu()
    {
        _menuPanel.SetActive(false);
    }

    private void RefreshScore()
    {
        _scoreText.text = $"SCORE {GameManager.Instance.Score}";
    }

    private void RefreshLives()
    {
        _livesText.text = new string('\u2665', GameManager.Instance.Lives);
    }

    private void ShowGameOver()
    {
        // the hint matches the device: no "Enter" key exists on a phone
        string continueHint = Application.isMobilePlatform ? "tap for menu" : "press Enter for menu";

        _gameOverPanel.SetActive(true);
        _gameOverText.text =
            $"GAME OVER\n\nscore {GameManager.Instance.Score}\nbest {GameManager.Instance.BestScore}\n\n{continueHint}";
    }
}