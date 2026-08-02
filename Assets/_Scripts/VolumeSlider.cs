using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private string _exposedName = "BGM";

    private Slider _slider;

    // シーンをまたいで保持
    private static readonly System.Collections.Generic.Dictionary<string, float> _volumeValues = new();

    private void Awake()
    {
        _slider = GetComponent<Slider>();

        // 初回は最大音量
        if (!_volumeValues.ContainsKey(_exposedName))
        {
            _volumeValues[_exposedName] = 1f;
        }

        _slider.SetValueWithoutNotify(_volumeValues[_exposedName]);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetVolume(_exposedName, _slider.value);
        }

        _slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        _volumeValues[_exposedName] = value;

        AudioManager.Instance.SetVolume(_exposedName, value);
    }
}