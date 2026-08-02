using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;

    [SerializeField] private string[] _countdownTexts = { "3", "2", "1" };
    [SerializeField] private string _startText = "START!";

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
        _canvasGroup = _countdownText.GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = _countdownText.gameObject.AddComponent<CanvasGroup>();
        }

        _countdownText.gameObject.SetActive(false);
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
        _countdownText.gameObject.SetActive(true);

        DOTween.Kill(_countdownText.transform);
        DOTween.Kill(_canvasGroup);

        Sequence sequence = DOTween.Sequence();
        sequence.SetUpdate(true);

        foreach (var text in _countdownTexts)
        {
            AppendStep(sequence, text, SEType.Count);
        }

        AppendStep(sequence, _startText,SEType.Start);

        sequence.OnComplete(() =>
        {
            _countdownText.gameObject.SetActive(false);
            GamePauseManager.SetPaused(false);
            AudioManager.Instance.PlayBGM(BGMType.InGame);
            OnCountdownFinished?.Invoke();
        });
    }

    private void AppendStep(Sequence sequence, string text, SEType type)
    {
        sequence.AppendCallback(() =>
        {
            _countdownText.text = text;
            _countdownText.transform.localScale = _startScale;
            _canvasGroup.alpha = 1f;
            AudioManager.Instance.PlaySE(type);
        });

        sequence.Append(
            _countdownText.transform
                .DOScale(_punchScale, _scaleUpDuration)
                .SetEase(Ease.OutBack));

        sequence.Append(
            _countdownText.transform
                .DOScale(Vector3.one, 0.1f));

        sequence.AppendInterval(_holdDuration);

        sequence.Append(
            _canvasGroup.DOFade(0f, _fadeOutDuration));
    }
}