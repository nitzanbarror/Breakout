using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WallController : MonoBehaviour
{
    [SerializeField] private GameConfig _config;

    [Header("Grid layout")]
    [SerializeField] private Brick _brickPrefab;
    [SerializeField] private int _columns = 7;
    [SerializeField] private int _startRows = 4;
    [SerializeField] private float _cellWidth = 1.6f;
    [SerializeField] private float _cellHeight = 0.6f;
    [SerializeField] private float _topY = 8.5f;
    [SerializeField] private float _deathLineY = -5f;
    [SerializeField] private Color[] _rowColors;

    private ObjectPool<Brick> _pool;
    private readonly List<Brick> _activeBricks = new List<Brick>();
    private float _currentInterval;
    private int _rowsSpawned;
    private bool _gameOver;

    private void Awake()
    {
        // course pattern: bricks are recycled, never destroyed during play
        _pool = new ObjectPool<Brick>(
            createFunc: CreateBrick,
            actionOnGet: brick => brick.gameObject.SetActive(true),
            actionOnRelease: brick => brick.gameObject.SetActive(false),
            defaultCapacity: 64);
    }

    private void Start()
    {
        _currentInterval = _config.rowInterval;

        for (int row = 0; row < _startRows; row++)
        {
            SpawnRow(_topY - row * _cellHeight);
        }

        StartCoroutine(SpawnRowsRoutine());
    }

    private void Update()
    {
        if (_gameOver)
        {
            return;
        }

        // the whole wall creeps down, always, at a constant speed
        transform.position += Vector3.down * (_config.descentSpeed * Time.deltaTime);

        // defeat: any brick touching the death line
        foreach (Brick brick in _activeBricks)
        {
            if (brick.transform.position.y - _cellHeight * 0.5f <= _deathLineY)
            {
                GameOver();
                return;
            }
        }
    }

    private IEnumerator SpawnRowsRoutine()
    {
        while (!_gameOver)
        {
            yield return new WaitForSeconds(_currentInterval);
            SpawnRowOnTop();
            _currentInterval = Mathf.Max(_config.rowIntervalMin, _currentInterval * _config.rowIntervalDecay);
        }
    }

    private Brick CreateBrick()
    {
        Brick brick = Instantiate(_brickPrefab, transform);
        brick.Init(this);
        return brick;
    }

    private void SpawnRowOnTop()
    {
        float y = _topY;

        if (_activeBricks.Count > 0)
        {
            float highest = float.MinValue;
            foreach (Brick brick in _activeBricks)
            {
                if (brick.transform.position.y > highest)
                {
                    highest = brick.transform.position.y;
                }
            }
            y = highest + _cellHeight;
        }

        SpawnRow(y);
    }

    private void SpawnRow(float y)
    {
        float firstX = -(_columns - 1) * _cellWidth * 0.5f;

        for (int col = 0; col < _columns; col++)
        {
            Brick brick = _pool.Get();
            brick.transform.position = new Vector3(firstX + col * _cellWidth, y, 0f);
            brick.SetColor(_rowColors[_rowsSpawned % _rowColors.Length]);
            _activeBricks.Add(brick);
        }

        _rowsSpawned++;
    }

    public void OnBrickDestroyed(Brick brick)
    {
        _activeBricks.Remove(brick);
        _pool.Release(brick);

        // +10 per brick, +50 bonus when it was the last brick of its row
        bool rowCleared = true;
        foreach (Brick other in _activeBricks)
        {
            if (Mathf.Abs(other.transform.position.y - brick.transform.position.y) < 0.1f)
            {
                rowCleared = false;
                break;
            }
        }

        GameManager.Instance.AddScore(rowCleared ? 60 : 10);
    }

    private void GameOver()
    {
        _gameOver = true;
        GameManager.Instance.GameOver();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-6f, _deathLineY, 0f), new Vector3(6f, _deathLineY, 0f));
    }
}