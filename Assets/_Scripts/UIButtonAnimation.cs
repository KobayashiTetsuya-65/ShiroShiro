using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonAnimation : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [Header("Target")]
    [SerializeField] private RectTransform target;

    [Header("Animation")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float pressScale = 0.95f;
    [SerializeField] private float duration = 0.15f;
    [SerializeField] private Ease ease = Ease.OutBack;


    private Vector3 defaultScale;

    private void Awake()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

        defaultScale = target.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        target.DOKill();
        target.DOScale(defaultScale * hoverScale, duration)
              .SetEase(ease)
              .SetUpdate(true);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        target.DOKill();
        target.DOScale(defaultScale, duration)
              .SetEase(ease)
              .SetUpdate(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        target.DOKill();
        target.DOScale(defaultScale * pressScale, duration * 0.5f)
              .SetEase(Ease.OutQuad)
              .SetUpdate(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        target.DOKill();
        target.DOScale(defaultScale * hoverScale, duration)
              .SetEase(ease)
              .SetUpdate(true);
    }
}