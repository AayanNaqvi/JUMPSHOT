using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAddon : MonoBehaviour
{
    public GameObject smokeEffect;
    public float duration;

    public float delay = 3f;
    public float countdown;


    public LayerMask whatIsGround;
    public float utilHeight;
    bool grounded;

    

    private GameObject smoke;

    [Header("Sound")]
    public AudioClip explosion;
    public float explosionVol;
    


    // Start is called before the first frame update
    void Start()
    {
        
        countdown = delay;
    }

    

    // Update is called once per frame
    void Update()
    {
        explosionVol = EditVolume.utilVol;
        countdown -= Time.deltaTime;
        grounded = Physics.Raycast(transform.position, Vector3.down, utilHeight * 0.5f + 0.1f, whatIsGround);


        if (countdown <= 0 || grounded)
        {
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    public void Explode()
    {
        smoke = Instantiate(smokeEffect, transform.position, Quaternion.identity);
        AudioManager.instance.Play3DSound(explosion, transform.position, explosionVol);


        
        Destroy(smoke, duration);
        Destroy(gameObject);
        

    }

    
}
