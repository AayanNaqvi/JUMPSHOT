using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioClip click;
    public float vol;

    public void makeNoise()
    {
        AudioManager.instance.Play2DSound(click, vol);
    }
    private void Update()
    {
        vol = EditVolume.envVol;
    }
}
