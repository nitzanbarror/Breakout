using UnityEngine;

// Runs before every other script (execution order -100) so the camera size
// is already correct when Ball and Paddle read it in their own Awake.
// Fixes the "clipped play field" bug on tall phones (Session 7: never assume one resolution).
[DefaultExecutionOrder(-100)]
public class CameraFitter : MonoBehaviour
{
    [SerializeField] private float _targetHalfWidth = 5.7f;
    [SerializeField] private float _minHalfHeight = 10f;

    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        float neededHalfHeight = _targetHalfWidth / cam.aspect;
        cam.orthographicSize = Mathf.Max(_minHalfHeight, neededHalfHeight);
    }
}