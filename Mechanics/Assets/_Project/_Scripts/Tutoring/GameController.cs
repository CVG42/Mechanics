using System;
using UnityEngine;

public class GameController : Singleton<IManagerSource>, IManagerSource
{
    public event Action<GameState> OnGameStateChanged;

    public GameState CurrentState { get; private set; }

    private void Start()
    {
        ChangeState(GameState.Waiting);
    }

    public void ChangeState(GameState state)
    {
        CurrentState = state;

        OnGameStateChanged?.Invoke(state);
    }
}

public enum GameState
{
    Waiting,
    Charging,
    Launching,
    Rolling,
    Finished
}

public interface IManagerSource
{
    event Action<GameState> OnGameStateChanged;
    GameState CurrentState { get; }
    void ChangeState(GameState state);
}
