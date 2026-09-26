using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField] private float _speed = 9f;
    [SerializeField] private float _horizontalLimit = 4.6f;

    private void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");

        Vector3 position = transform.position;
        position.x += input * _speed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, -_horizontalLimit, _horizontalLimit);
        transform.position = position;
    }
}