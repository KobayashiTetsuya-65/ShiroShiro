using TMPro;
using UnityEngine;

public class HighScoreText : MonoBehaviour
{
    [SerializeField] private ScoreMode _scoreMode;
    [SerializeField] private TextMeshProUGUI _text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text.text = $"{HighScore.Get(_scoreMode):D7}";
    }
}
