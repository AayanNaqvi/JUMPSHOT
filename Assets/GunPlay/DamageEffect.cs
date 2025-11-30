using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VolumeProfile volumeProfile;
    public Volume volume;
    





    [Header("Settings")]
    [SerializeField] private float vignetteRedIntensity;
    [SerializeField] private float redVignetteShowTime;
    




    public float intensity = 0;
    
    private Vignette vignette;
    
    
    private Color myRed;
    


    public static DamageEffect Instance;


    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        
        volume.profile = volumeProfile;
        

        volumeProfile.TryGet(out vignette);

        
        
        // Set the hexadecimal color to the desired color
        if (ColorUtility.TryParseHtmlString("#610000", out myRed)) // Red
            vignette.color.value = myRed;
        vignette.intensity.value = 0f;

        
    }

    

    public static void ShowVignetteOnHit()
    {
        Instance.StopAllCoroutines();
        Instance.StartCoroutine(Instance.ShowVignetteOnHitCoroutine());
    }

    

    private IEnumerator ShowVignetteOnHitCoroutine()
    {
        // Reset Intensity and set up hit vignette
        float intensity = vignetteRedIntensity;
        vignette.intensity.value = intensity;
        vignette.color.value = myRed;

        // Show hit Vignette for a short time
        yield return new WaitForSeconds(redVignetteShowTime);

        // Start decreasing intensity (i have a normal vignette with intesity 0.25f, so i decrease to that value)
        while (intensity > 0f)
        {
            intensity -= 0.01f;
            vignette.intensity.value = intensity;
            yield return new WaitForSeconds(0.1f);
        }
    }


    


}
