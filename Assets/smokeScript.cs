using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smokeScript : MonoBehaviour
{
    public Animator animator;
    public float duration;
    public float timer;

    private bool hasPlayedSound = false;


    [Header("Sound")]
    public AudioClip clear;
    public float clearVol;

    void Start()
    {
        timer = duration;
        
    }

    // Update is called once per frame
    void Update()
    {
        clearVol = EditVolume.utilVol;
        timer -= Time.deltaTime;
        if(timer <= 0 && !hasPlayedSound)
        {
            hasPlayedSound = true;
            animator.SetBool("SmokeClear", true);
            AudioManager.instance.Play3DSound(clear, transform.position, clearVol);
            Destroy(gameObject, 1f);
        }

    }

    
}
