using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int CurrentScore
    {
        get => _currentScore; 
        private set
        {
            _currentScore = value;
        }
    }

    private int _currentScore;
    private void Awake()
    {
        Instance = this;
        _currentScore = 0;
    }
    public void AddScore(int delta)
    {
        CurrentScore = Mathf.Max(0,CurrentScore + delta);
    }
}
