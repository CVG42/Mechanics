using UnityEngine;

public class BallPlayerController : MonoBehaviour
{
    private CustomBallRigidbody _body;

    private void Awake()
    {
        _body = GetComponent<CustomBallRigidbody>();
    }

    private void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        input = Vector2.ClampMagnitude(input, 1f);
        _body.SetMoveInput(input);
    }
}
