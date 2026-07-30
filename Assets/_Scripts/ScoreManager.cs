using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public bool IsStop => _isStop;
    public int CurrentScore
    {
        get => _currentScore; 
        private set
        {
            _currentScore = value;
        }
    }

    public float CurrentTime
    {
        get => _currentTime;
        private set
        {
            _currentTime = value;
        }
    }

    [Header("-----参照-----")]
    [SerializeField] private ResultManager _resultManager;
    [SerializeField] private Image _timerGauge;

    [Header("-----パラメータ調整-----")]
    [SerializeField] private float _maxTime = 100f;
    [SerializeField] private float _timeSpeed = 1f;

    private bool _isStop = false;
    private int _currentScore;
    private float _currentTime;
    private void Awake()
    {
        Instance = this;
        _currentScore = 0;
        _currentTime = _maxTime;
    }
    private void Update()
    {
        if (_isStop || GamePauseManager.IsPaused) return;

        _currentTime -= _timeSpeed * Time.deltaTime;
        ChangeTimerGauge();

        if(_currentTime <= 0)
        {
            _isStop = true;

            _resultManager.DisplayResult();
        }
    }
    public void AddScore(int delta)
    {
        CurrentScore = Mathf.Max(0,CurrentScore + delta);
    }

    public void ChangeTimerGauge()
    {
        _timerGauge.DOFillAmount(_currentTime / _maxTime, 0.05f);
    }
}
