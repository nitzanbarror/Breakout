using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private GameConfig _config;
    private WallController _wall;

    public void Init(GameConfig config, WallController wall)
    {
        _config = config;
        _wall = wall;
    }

    private void Update()
    {
        transform.position += Vector3.down * (_config.powerUpFallSpeed * Time.deltaTime);

        // fell past the paddle - back to the pool
        if (transform.position.y < -11f)
        {
            _wall.ReleasePowerUp(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Paddle"))
        {
            return;
        }

        other.GetComponent<PaddleController>().ActivateWide();
        _wall.ReleasePowerUp(this);
    }
}