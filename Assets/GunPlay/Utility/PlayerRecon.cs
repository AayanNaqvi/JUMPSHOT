using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRecon : MonoBehaviour
{
    public KeyCode reconKey;
    public float reconCooldown = 6f;
    public GameObject reconEffect;
    public GameObject player;
    public Slider sonarSlider;

    private float lastReconTime = -Mathf.Infinity;
    private bool isOnCooldown;

    public GameObject canvas;
    private UIElements ui;


    [Header("Sound")]
    public AudioClip recon;
    public float volume;


    private void Start()
    {
        sonarSlider.gameObject.SetActive(false);
        ui = canvas.GetComponent<UIElements>();
    }

    void Update()
    {
        volume = EditVolume.utilVol;
        
        if (Input.GetKeyDown(reconKey) && !ui.settingsActive && !ui.gunMenuActive)
        {
            TryFireReconPulse();
            
        }


        if (isOnCooldown)
        {
            float timeElapsed = Time.time - lastReconTime;
            float fillAmount = Mathf.Clamp01(timeElapsed / reconCooldown);
            sonarSlider.value = 1f - fillAmount;

            if (fillAmount >= 1f)
            {
                isOnCooldown = false;
                sonarSlider.gameObject.SetActive(false);
            }
        }
    }

    void TryFireReconPulse()
    {
        if (Time.time >= lastReconTime + reconCooldown)
        {
            FireReconPulse();
            lastReconTime = Time.time;
            isOnCooldown = true;
            sonarSlider.gameObject.SetActive(true);
            sonarSlider.value = sonarSlider.maxValue;
            AudioManager.instance.Play2DSound(recon, volume);
        }
        else
        {
            float timeLeft = (lastReconTime + reconCooldown) - Time.time;
            Debug.Log($"Recon on cooldown: {timeLeft:F1} seconds left");
        }
    }

    void FireReconPulse()
    {
        
        GameObject rc = Instantiate(reconEffect, player.transform.position, Quaternion.identity);
        Debug.Log("Spawning recon effect at: " + player.transform.position);
        SonarReveal[] enemies = FindObjectsOfType<SonarReveal>();
        foreach (SonarReveal enemy in enemies)
        {
            enemy.Reveal();
        }

        Destroy(rc, 3f);

        Debug.Log("Recon pulse activated!");
    }

    
}
