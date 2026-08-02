using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public bool IsStop => _isStop;
    public bool IsFever => _isFever;
    /// <summary>
    /// 現在のスコア。
    /// UI表示更新機能付き
    /// </summary>
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

    public int CurrentCombo
    {
        get => _currentCombo;
        private set
        {
            _currentCombo = value;

            if (value == 0)
            {
                _isCombo = false;
            }
            else
            {
                _isFade = false;
                _isCombo = true;
                _currentComboTime = _comboDuration;
                _comboText.text = value.ToString();
                _comboText.DOKill();
                _comboT.DOKill();
                _comboTr.DOKill();
                _comboText.DOFade(1f,0.05f);
                _comboT.DOFade(1f, 0.05f);
                _comboTr.DOScale(_comboMaxScale, 0.1f)
                    .OnComplete(() =>
                    {
                        _comboTr.DOScale(1f, 0.1f);
                    });
            }  
        }
    }

    public float CurrentTime=> _currentTime;

    [Header("-----参照-----")]
    [SerializeField] private ResultManager _resultManager;
    [SerializeField] private Image _timerGauge;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _finishText;
    [SerializeField] private TextMeshProUGUI _comboText;
    [SerializeField] private TextMeshProUGUI _comboT;
    [SerializeField] private Transform _comboTr;

    [Header("-----パラメータ調整-----")]
    [SerializeField] private float _maxTime = 100f;
    [SerializeField] private float _timeSpeed = 1f;
    [SerializeField] private float _scoreDuration = 0.1f;
    [SerializeField] private bool _isInfinite = false;
    [Header("フィーバー")]
    [SerializeField] private float _feverTime = 10f;
    [SerializeField] private float _feverScoreMag = 1.5f;
    [Header("コンボ")]
    [SerializeField] private float _comboDuration = 1f;
    [SerializeField] private float _comboScoreMag = 0.1f;
    [SerializeField] private float _comboMaxScale = 1.4f;

    private bool _isFever = false;
    private bool _isStop = false;
    private bool _isCombo = false;
    private bool _isFade = false;
    private int _currentScore,_currentCombo = 0;
    private float _currentTime, _currentFeverTime;
    private float _currentComboTime = 0;
    private Tween _scoreTween;
    private void Awake()
    {
        Instance = this;
        _currentScore = 0;
        _currentTime = _maxTime;
        _finishText.gameObject.SetActive(false);
        Color text = _comboText.color;
        _comboText.color = new Color(text.r, text.g, text.b, 0f);
        Color img = _comboT.color;
        _comboT.color = new Color(img.r, img.g, img.b, 0f);
    }
    private void Update()
    {
        if (_isStop || GamePauseManager.IsPaused) return;

        float time = _timeSpeed * Time.deltaTime;

        if (_isFever)
        {
            _currentFeverTime -= time;

            if(_currentFeverTime <= 0)
            {
                FeverTime(false);
            }
        }

        if (_isCombo)
        {
            _currentComboTime -= time;

            if (_currentComboTime <= _comboDuration / 2 && !_isFade)
            {
                _comboT.DOFade(0f, _currentComboTime);
                _comboText.DOFade(0f, _currentComboTime);
                _isFade = true;
            }
            if (_currentComboTime <= 0)
            {
                CurrentCombo = 0;
            }
        }

        if (_isInfinite) return;

        _currentTime -= time;
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
            delta = (int)(delta *_feverScoreMag);
        }
        if (_isCombo)
        {
            delta += (int)(delta * _comboScoreMag);
        }
        CurrentScore = Mathf.Clamp(CurrentScore + delta,0,9999999);
    }

    public void ChangeTimerGauge()
    {
        _timerGauge.DOFillAmount(_currentTime / _maxTime, 0.05f);
    }

    public void FeverTime(bool isStart = true)
    {
        AudioManager.Instance.PlaySE(SEType.Fever);
        _isFever = isStart;
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

    public void AddCombo()
    {
        CurrentCombo++;
    }
}
