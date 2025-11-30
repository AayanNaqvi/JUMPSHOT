using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowFPS : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float hudRefreshRate = 0.5f;
    private float timer;

    private void Update()
    {
        if(Time.unscaledTime > timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.SetText("FPS: " + fps);
            timer = Time.unscaledTime + hudRefreshRate;
        }
    }

    public void HideFPS()
    {
        gameObject.SetActive(false);
    }

    public void UnHideFPS()
    {
        gameObject.SetActive(true);
    }
}
