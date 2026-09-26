using System.Collections;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField] private GameConfig _config;

    private Coroutine _wideRoutine;
    private float _normalWidth;

    private void Awake()
    {
        _normalWidth = transform.localScale.x;
    }

    private void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");

        Vector3 position = transform.position;
        position.x += input * _config.paddleSpeed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, -_config.horizontalLimit, _config.horizontalLimit);
        transform.position = position;
    }

    // called by the Wide power-up (part B of this step)
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