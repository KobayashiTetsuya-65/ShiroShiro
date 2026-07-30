using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [Header("-----参照-----")]
    [SerializeField] private Image _resultPanel;
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("-----パラメータ調整-----")]
    [SerializeField] private float _duration = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _resultPanel.gameObject.SetActive(false);
    }

    public void DisplayResult()
    {
        _resultPanel.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() =>
        {
            int score = 0;
            DOTween.To(() => score,
                x =>
                {
                    score = x;
                    _scoreText.text = $"{score:D7}";
                },
                ScoreManager.Instance.CurrentScore,
                _duration);
        });
    }
}
