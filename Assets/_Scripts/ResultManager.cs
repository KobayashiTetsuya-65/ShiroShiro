using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [Header("-----éQè∆-----")]
    [SerializeField] private Image _resultPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _resultPanel.gameObject.SetActive(false);
    }

    public void DisplayResult()
    {
        _resultPanel.gameObject.SetActive(true);
    }
}
