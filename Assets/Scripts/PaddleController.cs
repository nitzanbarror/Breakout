using System.Collections;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField] private GameConfig _config;

    private Coroutine _wideRoutine;
    private float _normalWidth;
    private float _worldScreenWidth;

    private void Awake()
    {
        _normalWidth = transform.localScale.x;

        Camera mainCamera = Camera.main;
        _worldScreenWidth = mainCamera.orthographicSize * 2f * mainCamera.aspect;
    }

    private void Update()
    {
        Vector3 position = transform.position;
        position.x += ReadMoveDelta();
        position.x = Mathf.Clamp(position.x, -_config.horizontalLimit, _config.horizontalLimit);
        transform.position = position;
    }

    private float ReadMoveDelta()
    {
        // touch (Android): drag anywhere = relative movement, so the finger never covers the paddle
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float worldPerPixel = _worldScreenWidth / Screen.width;
                return touch.deltaPosition.x * worldPerPixel * _config.touchSensitivity;
            }
            return 0f;
        }

        // keyboard (PC)
        return Input.GetAxisRaw("Horizontal") * _config.paddleSpeed * Time.deltaTime;
    }

    // called by the Wide power-up
    public void ActivateWide()
    {
        if (_wideRoutine != null)
        {
            StopCoroutine(_wideRoutine);
        }
        _wideRoutine = StartCoroutine(WideRoutine());
    }

    private IEnumerator WideRoutine()
    {
        SetWidth(_config.paddleWideScale);
        yield return new WaitForSeconds(_config.wideDuration);
        SetWidth(_normalWidth);
        _wideRoutine = null;
    }

    private void SetWidth(float width)
    {
        Vector3 scale = transform.localScale;
        scale.x = width;
        transform.localScale = scale;
    }
}