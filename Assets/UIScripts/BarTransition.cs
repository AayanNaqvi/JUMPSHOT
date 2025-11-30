using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BarTransition : MonoBehaviour
{
    public RectTransform leftBar;
    public RectTransform rightBar;
    public float barWidth = 500f;
    public float duration = 0.5f;

    private Vector2 leftClosedPos;
    private Vector2 rightClosedPos;
    private Vector2 leftOpenPos;
    private Vector2 rightOpenPos;

    void Awake()
    {
        // Store positions
        leftClosedPos = new Vector2(0, 0);
        rightClosedPos = new Vector2(0, 0);

        leftOpenPos = new Vector2(-barWidth, 0);
        rightOpenPos = new Vector2(barWidth, 0);
    }

    public void OpenBars(System.Action onComplete = null) // Reveal screen
    {
        // Make sure bars start in the closed position BEFORE animation
        leftBar.anchoredPosition = leftClosedPos;
        rightBar.anchoredPosition = rightClosedPos;

        Sequence seq = DOTween.Sequence();

        // Move them to open positions
        seq.Append(leftBar.DOAnchorPos(leftOpenPos, duration).SetEase(Ease.OutQuad));
        seq.Join(rightBar.DOAnchorPos(rightOpenPos, duration).SetEase(Ease.OutQuad));

        if (onComplete != null)
            seq.OnComplete(() => onComplete());
    }

    public void CloseBars(System.Action onComplete = null) // Hide screen
    {
        leftBar.anchoredPosition = leftOpenPos;
        rightBar.anchoredPosition = rightOpenPos;

        Sequence seq = DOTween.Sequence();
        seq.Append(leftBar.DOAnchorPos(leftClosedPos, duration).SetEase(Ease.InQuad));
        seq.Join(rightBar.DOAnchorPos(rightClosedPos, duration).SetEase(Ease.InQuad));
        if (onComplete != null)
            seq.OnComplete(() => onComplete());
    }
}
