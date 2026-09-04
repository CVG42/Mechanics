using UnityEngine;

public class RoundController : MonoBehaviour
{
    private void Update()
    {
        if (GameController.Source.CurrentState != GameState.Waiting) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameController.Source.ChangeState(GameState.Charging);
        }
    }
}
