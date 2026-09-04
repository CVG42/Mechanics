using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] private float _velocityX = 0;
    [SerializeField] private float _velocityY = 0;

    private float _positionX;
    private float _positionY;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _positionX = transform.position.x;
        _positionY = transform.position.y;
    }

    private void Update()
    {
        _positionX += _velocityX * Time.deltaTime;
        _positionY += _velocityY * Time.deltaTime;

        _rigidbody.MovePosition(new Vector2(_positionX, _positionY));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;

        float dot = CustomEquations.DotProduct(new Vector2(_velocityX, _velocityY), normal);

        _velocityX = _velocityX - 2f * dot * normal.x;
        _velocityY = _velocityY - 2f * dot * normal.y;
    }
}
