using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LightFade : MonoBehaviour
{
    public bool fadeOut;
    float timer;
    float delay = 10f;

    private Light fireLight;

    void Start()
    {
        fadeOut = false;
        fireLight = GetComponent<Light>();
        timer = delay;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            fadeOut = true;
        }
        if(fadeOut)
        {
            LightFader();
        }
    }
    void LightFader()
    {
        fireLight.intensity = Mathf.Lerp(fireLight.intensity, 0f, 5f * Time.deltaTime);
    }
}
