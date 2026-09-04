using System;
using UnityEngine;
using Utilities;

public class ChargeManager : Singleton<IChargeSource>, IChargeSource
{
    public event Action<double> OnChargeFinished;

    [Header("Charge")]
    [SerializeField] private float _chargeDuration = 5f;

    [Header("Spring")]
    [SerializeField] private double _springConstant = 500;

    [SerializeField] private double _compressionPerClick = 0.01;

    private CountdownTimer _timer;

    public int ClickCount { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _timer = new CountdownTimer(_chargeDuration);
    }

    private void Start()
    {
        GameController.Source.OnGameStateChanged += HandleStateChanged;
    }

    private void OnDestroy()
    {
        GameController.Source.OnGameStateChanged -= HandleStateChanged;
    }

    private void Update()
    {
        if (!(_timer?.IsRunning ?? false)) return;

        _timer.Tick(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClickCount++;
        }

        if (Input.GetMouseButtonDown(0))
        {
            ClickCount++;
        }

        if (_timer.IsFinished)
        {
            double energy = GetEnergy();

            OnChargeFinished?.Invoke(energy);

            GameController.Source.ChangeState(GameState.Rolling);
        }
    }

    private void HandleStateChanged(GameState state)
    {
        if (state != GameState.Charging) return;

        ClickCount = 0;

        _timer.Reset(_chargeDuration);
        _timer.Start();
    }

    public double GetEnergy()
    {
        double compression = ClickCount * _compressionPerClick;

        return 0.5 * _springConstant * compression * compression;
    }
}

public interface IChargeSource
{
    event Action<double> OnChargeFinished;

    int ClickCount { get; }
    double GetEnergy();
}
