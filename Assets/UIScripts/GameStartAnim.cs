using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class GameStartAnim : MonoBehaviour
{
    public float fadeDuration = 1f;

    private void Start()
    {
        StartCoroutine(StartAnim());

        

        
    }

    private IEnumerator StartAnim()
    {
        yield return new WaitForSeconds(1f);
        Image img = GetComponent<Image>();


        // Start fully opaque
        Color c = img.color;
        c.a = 1f;
        img.color = c;
        // Fade out to transparent
        img.DOFade(0f, fadeDuration).SetEase(Ease.OutQuad)
            .OnComplete(() => gameObject.SetActive(false));
    }
}
