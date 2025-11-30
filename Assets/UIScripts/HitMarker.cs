using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{

    private CanvasGroup dmgImgCanvas;
    private Image hitmarkerImage;
    public Color normalHitColor;
    public Color weakspotHitColor;
    public Color killColor;

    public float flashDuration = 0.1f;
    public float fadeDuration = 0.5f;

    private float flashTimer = 0f;
    private float fadeTimer = 0f;

    public enum HitType
    {
        Normal,
        Weakspot,
        Kill
    }
    public static HitMarker instance;

    void Awake()
    {
        instance = this;
    }



    void Start()
    {
        dmgImgCanvas = GetComponent<CanvasGroup>();
        dmgImgCanvas.alpha = 0f;

        hitmarkerImage = GetComponentInChildren<Image>(); // or use GetComponentInChildren<Image>() if it's a child
    }

    void Update()
    {
        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            dmgImgCanvas.alpha = 1f;
        }
        else if (fadeTimer > 0f)
        {
            fadeTimer -= Time.deltaTime;
            dmgImgCanvas.alpha = fadeTimer / fadeDuration;
        }
        else
        {
            dmgImgCanvas.alpha = 0f;
        }
    }

    public void PlayHitmarker(HitType hitType)
    {
        switch (hitType)
        {
            case HitType.Normal:
                hitmarkerImage.color = normalHitColor;
                break;
            case HitType.Weakspot:
                hitmarkerImage.color = weakspotHitColor;
                break;
            case HitType.Kill:
                hitmarkerImage.color = killColor;
                break;
        }

        flashTimer = flashDuration;
        fadeTimer = fadeDuration;
        dmgImgCanvas.alpha = 1f;
        enabled = true;
    }

    
}
