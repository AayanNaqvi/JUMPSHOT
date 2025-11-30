using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // For DOTween base

public class EditVolume : MonoBehaviour
{
    public static EditVolume instance;
    public static float playerVol;
    public static float weaponVol;
    public static float utilVol;
    public static float enemVol;
    public static float envVol;
    public static float musVol;
    
    [Header("Sliders and Text")]
    public Slider gunSlider;
    public Slider utilSlider;
    public Slider enemSlider;
    public Slider playerSlider;
    public Slider envSlider;
    public Slider musicSlider;
    public TextMeshProUGUI gunText;
    public TextMeshProUGUI utilText;
    public TextMeshProUGUI enemText;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI envText;
    public TextMeshProUGUI musText;

    public AudioSource amb;
    public AudioSource music;




    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
        if (music != null && musicSlider != null)
        {
            float targetVolume = musicSlider.value; // Get current slider value

            music.volume = 0f; // Start muted
            

            music.DOFade(targetVolume, 2f);
        }



    }

    // Update is called once per frame
    void Update()
    {

        if (amb != null)
        {
            amb.volume = envVol;
        }
        if (music != null)
            music.volume = musVol;

        SetVolumeBasedOnSlider();


        SetTextBasedOnSlider(gunText, "WEAPONS:", weaponVol);
        SetTextBasedOnSlider(utilText, "UTILITY:", utilVol);
        SetTextBasedOnSlider(playerText, "PLAYER:", playerVol);
        SetTextBasedOnSlider(enemText, "ENEMIES:", enemVol);
        SetTextBasedOnSlider(envText, "ENVIRONMENT:", envVol);
        SetTextBasedOnSlider(musText, "MUSIC:", musVol);

    }

    private void SetVolumeBasedOnSlider()
    {


        if (gunSlider != null)
            weaponVol = gunSlider.value;
        if (playerSlider != null)
            playerVol = playerSlider.value;
        if (utilSlider != null)
            utilVol = utilSlider.value;
        if (enemSlider != null)
            enemVol = enemSlider.value;
        if (envSlider != null)
            envVol = envSlider.value;
        if (musicSlider != null)
            musVol = musicSlider.value;
        
    }
    




    private void SetTextBasedOnSlider(TextMeshProUGUI text, string letter, float volVal)
    {
        if (text == null)
            return;
        
        
        if(volVal == 0)
        {
            text.SetText(letter + " NONE");
        }
        else if(Mathf.RoundToInt(volVal * 5f) == 0f)
        {
            text.SetText(letter + " VERY LOW");
        }
        else if (Mathf.RoundToInt(volVal * 5f) == 1f)
        {
            text.SetText(letter + " LOW");
        }
        else if (Mathf.RoundToInt(volVal * 5f) == 2f)
        {
            text.SetText(letter + " MEDIUM");
        }
        else if (Mathf.RoundToInt(volVal * 5f) == 3f)
        {
            text.SetText(letter + " HIGH");
        }
        else if (Mathf.RoundToInt(volVal * 5f) == 4f)
        {
            text.SetText(letter + " VERY HIGH");
        }



    }
}
