using System;
using UnityEngine;

public class ScoreSystem : Singleton<IScoreSource>, IScoreSource
{
    public event Action<int> OnScoreCalculated;

    [SerializeField] private ScoreConfig _scoreConfig;

    public int CurrentScore { get; private set; }

    private void Start()
    {
        BallManager.Source.OnBallStopped += CalculateScore;
    }

    private void OnDestroy()
    {
        BallManager.Source.OnBallStopped -= CalculateScore;
    }

    private void CalculateScore(double distance)
    {
        foreach (var range in _scoreConfig.Ranges)
        {
            if (distance < range.MinDistance) continue;

            if (distance > range.MaxDistance) continue;

            CurrentScore = range.Score;

            OnScoreCalculated?.Invoke(CurrentScore);

            GameController.Source.ChangeState(GameState.Finished);

            return;
        }

        CurrentScore = 0;

        OnScoreCalculated?.Invoke(0);
    }
}

public interface IScoreSource
{
    event Action<int> OnScoreCalculated;
    int CurrentScore { get; }
}
