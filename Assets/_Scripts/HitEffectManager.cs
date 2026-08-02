using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitEffectManager : MonoBehaviour
{
    public static HitEffectManager Instance { get; private set; }

    [SerializeField] private Image _hitPrefab;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private Camera _worldCamera;
    [SerializeField] private float _lifeTime = 0.25f;
    [SerializeField] private int _poolSize = 20;
    [SerializeField] private Canvas _canvas;

    private readonly Queue<Image> _pool = new();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < _poolSize; i++)
        {
            Image hit = Instantiate(_hitPrefab, _parent);
            hit.gameObject.SetActive(false);
            _pool.Enqueue(hit);
        }
    }

    public void Play(Vector3 worldPosition)
    {
        Image hit = Get();

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
            _canvas.worldCamera,
            worldPosition);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            screenPos,
            _canvas.worldCamera,
            out Vector2 localPos);

        hit.rectTransform.SetParent(_parent, false);
        hit.rectTransform.anchoredPosition = localPos;

        hit.gameObject.SetActive(true);

        StartCoroutine(Return(hit));
    }

    private Image Get()
    {
        if (_pool.Count > 0)
            return _pool.Dequeue();

        Image hit = Instantiate(_hitPrefab, _parent);
        hit.gameObject.SetActive(false);

        return hit;
    }

    private IEnumerator Return(Image hit)
    {
        yield return new WaitForSeconds(_lifeTime);

        hit.gameObject.SetActive(false);
        _pool.Enqueue(hit);
    }
}