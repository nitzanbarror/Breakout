using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WallController : MonoBehaviour
{
    [SerializeField] private Brick _brickPrefab;
    [SerializeField] private int _columns = 7;
    [SerializeField] private int _startRows = 4;
    [SerializeField] private float _cellWidth = 1.6f;
    [SerializeField] private float _cellHeight = 0.6f;
    [SerializeField] private float _topY = 8.5f;
    [SerializeField] private Color[] _rowColors;

    private ObjectPool<Brick> _pool;
    private readonly List<Brick> _activeBricks = new List<Brick>();

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
        for (int row = 0; row < _startRows; row++)
        {
            SpawnRow(row);
        }
    }

    private Brick CreateBrick()
    {
        Brick brick = Instantiate(_brickPrefab, transform);
        brick.Init(this);
        return brick;
    }

    private void SpawnRow(int rowIndex)
    {
        float y = _topY - rowIndex * _cellHeight;
        float firstX = -(_columns - 1) * _cellWidth * 0.5f;

        for (int col = 0; col < _columns; col++)
        {
            Brick brick = _pool.Get();
            brick.transform.position = new Vector3(firstX + col * _cellWidth, y, 0f);
            brick.SetColor(_rowColors[rowIndex % _rowColors.Length]);
            _activeBricks.Add(brick);
        }
    }

    public void OnBrickDestroyed(Brick brick)
    {
        _activeBricks.Remove(brick);
        _pool.Release(brick);
    }
}