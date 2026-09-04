using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void Start()
    {
        ScoreSystem.Source.OnScoreCalculated += UpdateUI;
    }

    private void OnDestroy()
    {
        ScoreSystem.Source.OnScoreCalculated -= UpdateUI;
    }

    private void UpdateUI(int score)
    {
        Debug.Log($"Score: {score}");
    }
}
