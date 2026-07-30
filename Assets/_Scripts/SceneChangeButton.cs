using UnityEngine;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] private Button _sceneChangeButton;
    [SerializeField] private SceneName _sceneName;
    private void Start()
    {
        _sceneChangeButton.onClick.AddListener(ScenChange);
    }

    private void ScenChange()
    {
        SceneController.Instance.SceneChange(_sceneName);
    }
}
