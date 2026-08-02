using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [Header("-----参照-----")]
    [SerializeField] private Image _resultPanel;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timeUpText;
    [SerializeField] private TextMeshProUGUI _gameoverText;
    [SerializeField] private RectTransform _high;

    [Header("-----パラメータ調整-----")]
    [SerializeField] private float _duration = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timeUpText.gameObject.SetActive(false);
        _gameoverText.gameObject.SetActive(false);
        _resultPanel.gameObject.SetActive(false);
    }

    public void DisplayResult(bool isTime)
    {
        GamePauseManager.SetPaused(true);
        _resultPanel.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.2f);

        TextMeshProUGUI resultText = isTime ? _timeUpText : _gameoverText;
        bool update = false;
        string text = resultText.text;
        resultText.text = "";
        resultText.gameObject.SetActive(true);
        seq.AppendCallback(() =>
        {
            ScoreMode scoreMode = ScoreManager.Instance.IsInfinite switch
            {
                false => ScoreMode.Normal,
                true => ScoreMode.Endless,
            };
            update = HighScore.TrySubmit(scoreMode, ScoreManager.Instance.CurrentScore);
            Debug.Log(ScoreManager.Instance.CurrentScore);
        });

        int length = 0;
        seq.Append(
            DOTween.To(() => length,
                x =>
                {
                    length = x;
                    resultText.text = text.Substring(0, length);
                },
                text.Length,
                1.2f));

        int score = 0;
        seq.Append(
            DOTween.To(() => score,
                x =>
                {
                    score = x;
                    _scoreText.text = $"{score:D7}";
                },
                ScoreManager.Instance.CurrentScore,
                _duration));
        seq.OnComplete(() =>
        {
            if (update)
            {
                AudioManager.Instance.PlaySE(SEType.HighScore);
                _high.gameObject.SetActive(true);
                _high.DOScale(2f, 0.4f)
                .OnComplete(() =>
                {
                    _high.DOScale(1f, 0.3f);
                });
            }
        });
    }
}
