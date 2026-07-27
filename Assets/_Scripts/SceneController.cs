using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    public SceneName CurrentScene => _currentScene;
    [SerializeField] private Image _fadePanel;
    [SerializeField] private float _fadeDuration = 0.5f;
    private SceneName _currentScene;
    private bool _isFade = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
    }

    public void SceneChange(SceneName name)
    {
        if (_isFade) return;
        _isFade = true;
        FadePanel(false, async () =>
        {
            await SceneManager.LoadSceneAsync($"{name}");
            _currentScene = name;
            _isFade = false;
            FadePanel(true);
        });
    }

    public void FadePanel(bool isFadeIN, System.Action onComplate = null, float duration = 0f, Ease ease = Ease.Unset)
    {
        if (duration == 0f) duration = _fadeDuration;
        float to = isFadeIN ? 0f : 1f;
        float start = isFadeIN ? 1f : 0f;
        _fadePanel.color = new Color(0f, 0f, 0f, start);
        _fadePanel.raycastTarget = true;
        _fadePanel.DOFade(to, duration)
            .SetEase(ease)
            .OnComplete(() =>
            {
                _fadePanel.raycastTarget = false;
                onComplate?.Invoke();
            }).SetAutoKill(true);
    }
}
public enum SceneName
{
    Title,
    InGame,
    Result
}
