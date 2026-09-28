using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private Vector3 _basePosition;
    private Coroutine _routine;

    private void Awake()
    {
        Instance = this;
        _basePosition = transform.localPosition;
    }

    public void Shake(float strength, float duration)
    {
        if (_routine != null)
        {
            StopCoroutine(_routine);
            transform.localPosition = _basePosition;
        }
        _routine = StartCoroutine(ShakeRoutine(strength, duration));
    }

    private IEnumerator ShakeRoutine(float strength, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float falloff = 1f - elapsed / duration;
            transform.localPosition = _basePosition + (Vector3)(Random.insideUnitCircle * strength * falloff);
            yield return null;
        }

        transform.localPosition = _basePosition;
        _routine = null;
    }
}