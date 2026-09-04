using Unity.Mathematics;
using UnityEngine;

public class Vectors2D : MonoBehaviour
{
    private float positionX;
    private float positionY;

    private float startPositionX;
    private float startPositionY;

    private float totalDistance;

    private void Start()
    {
        positionX = transform.position.x;
        positionY = transform.position.y;

        startPositionX = positionX;
        startPositionY = positionY;
    }

    private void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.D)) moveX += 5f * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) moveX -= 5f * Time.deltaTime;
        if (Input.GetKey(KeyCode.W)) moveY += 5f * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) moveY -= 5f * Time.deltaTime;

        float distance = Mathf.Sqrt(moveX * moveX + moveY * moveY);
        totalDistance += distance;

        positionX += moveX;
        positionY += moveY;

        transform.position = new Vector2 (positionX, positionY);

        float displacementX = positionX - startPositionX;
        float displacementY = positionY - startPositionY;

        float displacementMagnitude = Mathf.Sqrt(displacementX * displacementX + displacementY * displacementY);

        Debug.Log("Total distance:" + totalDistance);
        Debug.Log("Displacement" + displacementMagnitude);

        Debug.DrawLine(new Vector3(startPositionX, startPositionY), new Vector3(positionX, positionY), Color.yellow);
    }
}
