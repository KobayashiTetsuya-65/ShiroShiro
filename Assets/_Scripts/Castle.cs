using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public static Castle Instance { get; private set; }

    public event Action<int> OnHpChanged;

    [SerializeField] private ResultManager _resultManager;
    [Header("HP")]
    [SerializeField] private int _maxHp = 100;
    public int CurrentHp { get; private set; }

    [Header("見た目")]
    [SerializeField] private Image _castleImage;
    [SerializeField] private Image _overlay;
    [SerializeField] private Image _castleHpImage;
    [SerializeField] private Image _castleHpGuege;
    [SerializeField] private Sprite[] _castleSprites;
    [SerializeField] private Sprite[] _castleHpSprites;

    [Header("演出")]
    [SerializeField] private float _flashDuration = 0.1f;
    [SerializeField] private Color _damageColor = Color.red;

    private void Awake()
    {
        Instance = this;

        CurrentHp = _maxHp;
        UpdateCastleImage();
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHp <= 0) return;

        CurrentHp -= damage;
        CurrentHp = Mathf.Max(CurrentHp, 0);

        OnHpChanged?.Invoke(CurrentHp);

        UpdateCastleImage();

        PlayDamageFlash();

        if (CurrentHp == 0)
        {
            _resultManager.DisplayResult(false);
        }
    }

    private void UpdateCastleImage()
    {
        if (_castleSprites == null || _castleSprites.Length == 0)
            return;

        float hpRate = (float)CurrentHp / _maxHp;

        int index;

        if (hpRate > 0.95f)
            index = 0;
        else if (hpRate > 0.8f)
            index = 1;
        else if (hpRate > 0.6f)
            index = 2;
        else if (hpRate > 0.4f)
            index = 3;
        else if (hpRate > 0.2f)
            index = 4;
        else
            index = 5;

        index = Mathf.Clamp(index, 0, _castleSprites.Length - 1);

        _castleImage.sprite = _castleSprites[index];
        _castleHpImage.sprite = _castleHpSprites[index];
        _castleHpGuege.fillAmount = 1-hpRate;
    }

    private void PlayDamageFlash()
    {
        _overlay.DOKill();

        _overlay.DOFade(1, 0.05f)
                .OnComplete(() =>
                    _overlay.DOFade(0, 0.1f));
    }
}