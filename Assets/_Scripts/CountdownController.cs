using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    [SerializeField] private Image _countdownImage;
    [SerializeField] private Sprite[] _numberSprites; 
    [SerializeField] private Sprite _startSprite;      

    [SerializeField] private float _scaleUpDuration = 0.15f;
    [SerializeField] private float _holdDuration = 0.5f;
    [SerializeField] private float _fadeOutDuration = 0.2f;
    [SerializeField] private Vector3 _startScale = Vector3.zero;
    [SerializeField] private Vector3 _punchScale = new Vector3(1.3f, 1.3f, 1.3f);

    [SerializeField] private bool _playOnStart = false;

    public event Action OnCountdownFinished;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = _countdownImage.GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = _countdownImage.gameObject.AddComponent<CanvasGroup>();
        }

        _countdownImage.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (_playOnStart)
        {
            PlayCountdown();
        }
    }

    public void PlayCountdown()
    {
        GamePauseManager.SetPaused(true);
        _countdownImage.gameObject.SetActive(true);

        DOTween.Kill(_countdownImage.transform);
        DOTween.Kill(_canvasGroup);

        Sequence sequence = DOTween.Sequence();
        sequence.SetUpdate(true);

        foreach (var sprite in _numberSprites)
        {
            AppendStep(sequence, sprite, "a");
        }

        AppendStep(sequence, _startSprite, "a");

        sequence.OnComplete(() =>
        {
            _countdownImage.gameObject.SetActive(false);
            GamePauseManager.SetPaused(false);
            OnCountdownFinished?.Invoke();
        });
    }

    private void AppendStep(Sequence sequence, Sprite sprite, string soundId)
    {
        sequence.AppendCallback(() =>
        {
            _countdownImage.sprite = sprite;
            _countdownImage.transform.localScale = _startScale;
            _canvasGroup.alpha = 1f;
        });

        sequence.Append(_countdownImage.transform.DOScale(_punchScale, _scaleUpDuration).SetEase(Ease.OutBack));
        sequence.Append(_countdownImage.transform.DOScale(Vector3.one, 0.1f));
        sequence.AppendInterval(_holdDuration);
        sequence.Append(_canvasGroup.DOFade(0f, _fadeOutDuration));
    }
}