using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMP_Text _gameOverText;

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;

        // observer pattern: the UI listens for changes, it never polls
        gameManager.OnScoreChanged.AddListener(RefreshScore);
        gameManager.OnLivesChanged.AddListener(RefreshLives);
        gameManager.OnGameOver.AddListener(ShowGameOver);

        RefreshScore();
        RefreshLives();
        _gameOverPanel.SetActive(false);
    }

    private void RefreshScore()
    {
        _scoreText.text = $"SCORE {GameManager.Instance.Score}";
    }

    private void RefreshLives()
    {
        _livesText.text = $"LIVES {GameManager.Instance.Lives}";
    }

    private void ShowGameOver()
    {
        _gameOverPanel.SetActive(true);
        _gameOverText.text =
            $"GAME OVER\n\nscore {GameManager.Instance.Score}\nbest {GameManager.Instance.BestScore}\n\npress Enter to restart";
    }
}