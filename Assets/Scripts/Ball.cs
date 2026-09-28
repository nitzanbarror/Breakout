using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private GameConfig _config;
    [SerializeField] private Transform _paddle;
    [SerializeField] private float _bottomY = -10.5f;
    [SerializeField] private float _serveOffsetY = 0.3f;

    private Rigidbody2D _rigidbody;
    private TrailRenderer _trail;
    private bool _isServed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _trail = GetComponent<TrailRenderer>();
        _trail.emitting = false;
    }

    private void Update()
    {
        if (!_isServed)
        {
            // stick to the paddle until launch
            transform.position = _paddle.position + Vector3.up * _serveOffsetY;

            bool launchPressed = Input.GetKeyDown(KeyCode.Space)
                || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);

            if (launchPressed)
            {
                Launch();
            }
            return;
        }

        if (transform.position.y < _bottomY)
        {
            GameManager.Instance.LoseLife();
            CameraShake.Instance.Shake(0.25f, 0.35f);
            AudioManager.Instance.PlayLifeLost();
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
        _trail.emitting = true;
        AudioManager.Instance.PlayLaunch();

        float randomAngle = Random.Range(-20f, 20f);
        Vector2 direction = Quaternion.Euler(0f, 0f, randomAngle) * Vector2.up;
        _rigidbody.linearVelocity = direction * _config.ballSpeed;
    }

    private void ResetToServe()
    {
        _isServed = false;
        _rigidbody.linearVelocity = Vector2.zero;

        // no ugly streak when the ball teleports back to the paddle
        _trail.Clear();
        _trail.emitting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // bounce blip on paddle and walls; bricks make their own sound
        bool hitBrick = collision.gameObject.GetComponent<Brick>() != null;
        if (!hitBrick)
        {
            AudioManager.Instance.PlayBounce();
        }

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