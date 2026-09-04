using UnityEngine;

public class GameManager : Singleton<IGameSource>, IGameSource
{
    [Header("Unity Time speed (1 = realtime)")]
    [SerializeField] private float _timeScaleMod = 3.0f;
    [SerializeField] private float _timeScaleInfo;

    [Header("Scaling options")]
    [SerializeField] private float _spaceScale = 6000; // 1 unit = SpaceScale kilometers
    [SerializeField] private float _timeScale = 100; // 1 unity unit = timeScale seconds
    [SerializeField] private float _modifiedFixedDeltaTime = 0.02f; // Default

    public float SpaceScaleMeters
    {
        get { return _spaceScale * 1000; }
    }
    public float SpaceScaleKm
    {
        get { return _spaceScale; }
    }

    public float TimeScale
    {
        get { return _timeScale; }
    }
    public float ModifiedDeltaTime
    {
        get { return _modifiedFixedDeltaTime; }
    }

    protected override void Awake()
    {
        base.Awake();
        Time.fixedDeltaTime = ModifiedDeltaTime;
    }

    private void Update()
    {
        UpdateTimeScale();
    }

    private void UpdateTimeScale()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale + _timeScaleMod, 1.0f, 99);
            _timeScaleInfo = Time.timeScale;
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale - _timeScaleMod, 1.0f, 99);
            _timeScaleInfo = Time.timeScale;
        }
    }
}

public interface IGameSource
{
    float SpaceScaleMeters { get; }
    float SpaceScaleKm { get; }
    float TimeScale {  get; }
    float ModifiedDeltaTime { get; }
}
