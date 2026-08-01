using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public bool IsStop => _isStop;
    public bool IsFever
    {
        get => _isFever;
        private set
        {
            _isFever = value;
        }
    }
    public int CurrentScore
    {
        get => _currentScore; 
        private set
        {
            int start = _currentScore;
            int goal = value;
            _currentScore = value;

            if (_scoreTween != null)
                _scoreTween.Kill();

            _scoreTween = DOTween.To(() => start,
                x =>
                {
                    start = x;
                    _scoreText.text = $"{start:D7}";
                },
                goal,
                _scoreDuration)
                .SetLink(gameObject);
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
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _finishText;

    [Header("-----パラメータ調整-----")]
    [SerializeField] private float _maxTime = 100f;
    [SerializeField] private float _timeSpeed = 1f;
    [SerializeField] private float _scoreDuration = 0.1f;
    [SerializeField] private float _feverTime = 10f;
    [SerializeField] private float _feverScoreMag = 1.5f;

    private bool _isFever = false;
    private bool _isStop = false;
    private int _currentScore;
    private float _currentTime;
    private float _currentFeverTime;
    private Tween _scoreTween;
    private void Awake()
    {
        Instance = this;
        _currentScore = 0;
        _currentTime = _maxTime;
        _finishText.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (_isStop || GamePauseManager.IsPaused) return;

        if (_isFever)
        {
            _currentFeverTime -= _timeSpeed * Time.deltaTime;

            if(_currentFeverTime <= 0)
            {
                FeverTime(false);
            }
        }

        _currentTime -= _timeSpeed * Time.deltaTime;
        ChangeTimerGauge();

        if(_currentTime <= 0)
        {
            _isStop = true;

            FinishAnimatoin(true);
        }
    }
    public void AddScore(int delta)
    {
        if (_isFever)
        {
            delta = (int)((float)delta *_feverScoreMag);
        }
        CurrentScore = Mathf.Max(0,CurrentScore + delta);
    }

    public void ChangeTimerGauge()
    {
        _timerGauge.DOFillAmount(_currentTime / _maxTime, 0.05f);
    }

    public void FeverTime(bool isStart = true)
    {
        IsFever = isStart;
        _currentFeverTime = _feverTime;
    }
    private void FinishAnimatoin(bool isTime)
    {
        _finishText.gameObject.SetActive(true);
        _finishText.color = new Color(
            _finishText.color.r, _finishText.color.g, _finishText.color.b, 0f);

        Sequence seq = DOTween.Sequence();
        seq.Append(_finishText.DOFade(1f, 0.1f));
        seq.Join(_finishText.rectTransform.DOScale(2f, 0.4f));
        seq.Append(_finishText.rectTransform.DOScale(1f, 0.2f));
        seq.AppendInterval(2f);
        seq.AppendCallback(() =>
        {
            _finishText.gameObject.SetActive(false);
            _resultManager.DisplayResult(isTime);
        });
    }
}
