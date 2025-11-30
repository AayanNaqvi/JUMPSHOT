using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StartTitleAnim : MonoBehaviour
{
    [Header("Logo Elements")]
    [SerializeField] private RectTransform logoTransform;
    [SerializeField] private CanvasGroup logoCanvasGroup;

    [Header("UI Elements")]
    [SerializeField] private CanvasGroup titleText;
    [SerializeField] private CanvasGroup buttonGroup; // Play/Quit grouped under a parent with CanvasGroup
    [SerializeField] private CanvasGroup backgroundPanel;

    [Header("Flash")]
    [SerializeField] private Image flashImage; // Fullscreen white image with alpha 0 initially

    [Header("Animation Settings")]
    [SerializeField] private Vector2 logoStartPosition = new Vector2(0, 102.164f);
    [SerializeField] private Vector2 logoEndPosition = new Vector2(0f, 200f);
    [SerializeField] private float logoStartRotation = 45f;
    [SerializeField] private float logoEndRotation = 0f;
    [SerializeField] private float fadeDuration = 0.75f;
    [SerializeField] private float holdDuration = 0.5f;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private float uiFadeInDuration = 0.8f;

    [SerializeField] private AudioClip shot;
    public float shotVol;

    private void Start()
    {
        SetupInitialState();
        PlayTitleAnimation();
    }

    private void SetupInitialState()
    {
        // Set all alphas to 0
        logoCanvasGroup.alpha = 0f;
        titleText.alpha = 0f;
        buttonGroup.alpha = 0f;
        backgroundPanel.alpha = 0f;
        flashImage.color = new Color(1f, 1f, 1f, 0f); // Fully transparent white

        buttonGroup.interactable = false;
        buttonGroup.blocksRaycasts = false;

        // Set initial transform for logo
        logoTransform.anchoredPosition = logoStartPosition;
        logoTransform.localRotation = Quaternion.Euler(0f, 0f, logoStartRotation);
    }

    private void PlayTitleAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        // Step 1: Fade in logo
        sequence.Append(logoCanvasGroup.DOFade(1f, fadeDuration));

        // Step 2: Hold
        sequence.AppendInterval(holdDuration);

        // Step 3: Flash + Move Logo + Rotate
        sequence.AppendCallback(() =>
        {
            // Flash to white
            flashImage.DOFade(1f, flashDuration / 2f)
                .OnComplete(() => flashImage.DOFade(0f, flashDuration / 2f));
            AudioManager.instance.Play2DSound(shot, shotVol);
        });

        sequence.Append(logoTransform.DOAnchorPos(logoEndPosition, moveDuration).SetEase(Ease.OutCubic));
        sequence.Join(logoTransform.DOLocalRotate(new Vector3(0, 0, logoEndRotation), moveDuration).SetEase(Ease.OutCubic));

        // Step 4: Fade in other UI elements after flash
        sequence.Join(titleText.DOFade(1f, uiFadeInDuration));
        sequence.Join(buttonGroup.DOFade(1f, uiFadeInDuration));
        sequence.Join(backgroundPanel.DOFade(1f, uiFadeInDuration));

        sequence.OnComplete(() =>
        {
            buttonGroup.interactable = true;
            buttonGroup.blocksRaycasts = true;
        });
    }
}