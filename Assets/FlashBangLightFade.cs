using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBangLightFade : MonoBehaviour
{
    public Light lighter;
    
    
    // Start is called before the first frame update
    void Start()
    {
        lighter = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        lighter.intensity = Mathf.Lerp(lighter.intensity, 0f, 700f * Time.deltaTime);
    }
}
