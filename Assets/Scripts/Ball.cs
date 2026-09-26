using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private GameConfig _config;
    [SerializeField] private Transform _paddle;
    [SerializeField] private float _bottomY = -10.5f;
    [SerializeField] private float _serveOffsetY = 0.3f;

    private Rigidbody2D _rigidbody;
    private bool _isServed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!_isServed)
        {
            // stick to the paddle until launch
            transform.position = _paddle.position + Vector3.up * _serveOffsetY;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Launch();
            }
            return;
        }

        if (transform.position.y < _bottomY)
        {
            GameManager.Instance.LoseLife();
            ResetToServe();
        }
    }

    private void FixedUpdate()
    {
        if (!_isServed)
        {
            return;
        }

        Vector2 direction = _rigidbody.linearVelocity.normalized;

        // anti-flat-ball guard: never let the ball travel almost horizontally
        if (Mathf.Abs(direction.y) < _config.minVerticalFraction)
        {
            direction.y = direction.y >= 0f ? _config.minVerticalFraction : -_config.minVerticalFraction;
            direction = direction.normalized;
        }

        _rigidbody.linearVelocity = direction * _config.ballSpeed;
    }

    private void Launch()
    {
        _isServed = true;
        float randomAngle = Random.Range(-20f, 20f);
        Vector2 direction = Quaternion.Euler(0f, 0f, randomAngle) * Vector2.up;
        _rigidbody.linearVelocity = direction * _config.ballSpeed;
    }

    private void ResetToServe()
    {
        _isServed = false;
        _rigidbody.linearVelocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Paddle"))
        {
            return;
        }

        // THE core control rule: bounce angle depends on WHERE the ball hit the paddle
        float hitPoint = (transform.position.x - collision.transform.position.x)
                         / collision.collider.bounds.extents.x;
        hitPoint = Mathf.Clamp(hitPoint, -1f, 1f);

        float angle = hitPoint * _config.bounceArcDeg;
        Vector2 direction = Quaternion.Euler(0f, 0f, -angle) * Vector2.up;
        _rigidbody.linearVelocity = direction * _config.ballSpeed;
    }
}