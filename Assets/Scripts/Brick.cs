using UnityEngine;

public class Brick : MonoBehaviour
{
    private WallController _wall;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(WallController wall)
    {
        _wall = wall;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            _wall.OnBrickDestroyed(this);
        }
    }
}