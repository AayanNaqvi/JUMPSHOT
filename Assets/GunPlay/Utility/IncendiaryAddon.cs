using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncendiaryAddon : MonoBehaviour
{
    public GameObject explosionEffect;
    public GameObject fireEffect;

    public float delay = 3f;
   
    public float countdown;

    

    
    public LayerMask whatIsGround;
    public float utilHeight;
    bool grounded;



    [Header("Sound Effects")]
    public AudioClip explosionSFX;
    public float expVol;




    void Start()
    {
        countdown = delay;
        
    }

   


    void Update()
    {
        expVol = EditVolume.utilVol;
        countdown -= Time.deltaTime;


        grounded = Physics.Raycast(transform.position, Vector3.down, utilHeight * 0.5f + 0.1f, whatIsGround);

        
        if(grounded)
        {
            Explode();
        }
        
        
        

        
        



    }


    private void Explode()
    {

        AudioManager.instance.Play3DSound(explosionSFX, transform.position, expVol);

        GameObject explosion = Instantiate(explosionEffect, transform.position + new Vector3(0f, .3f, 0f), transform.rotation);
        Destroy(explosion, 5f);
        
        GameObject fire = Instantiate(fireEffect, transform.position, Quaternion.identity);

        
        
        

        Destroy(gameObject);





        Destroy(fire, 14f);
        //fireStarted = false;    
    }

    

    

    

}
