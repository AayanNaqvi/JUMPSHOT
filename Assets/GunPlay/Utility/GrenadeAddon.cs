using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GrenadeAddon : MonoBehaviour
{
    private bool targetHit;

    public GameObject explosionEffect;

    public float damage1Layer, damage2Layer, damage3Layer;
    public float dmgRadius1, dmgRadius2, dmgRadius3;

    

    public RaycastHit hit1;
    public LayerMask whatIsGround;

    public LayerMask whatIsEnemy;
    public float countdown;
    public float fuseTime = 3f;
    public float force = 10f;

    bool hasExploded = false;


    [Header("Sound Effects")]
    public AudioClip explosion;
    public float expVol;
    public AudioClip hitSound;
    public float hitVol;
    public AudioClip kill;
    public float killVol;

    void Start()
    {
        countdown = fuseTime;
    }

    public void Update()
    {
        expVol = EditVolume.utilVol;
        hitVol = EditVolume.weaponVol;
        killVol = EditVolume.weaponVol;

        countdown -= Time.deltaTime;
        if(countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }

    }

    public void Explode()
    {
        AudioManager.instance.Play3DSound(explosion, transform.position, expVol);
        GameObject exploder = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(exploder, 5f);

        

        CalcDMG(damage1Layer, dmgRadius1);
        CalcDMG(damage2Layer, dmgRadius2);
        CalcDMG(damage3Layer, dmgRadius3);



        Destroy(gameObject);
        


        
    }

    private void CalcDMG(float dmgLayer, float dmgRad)
    {

        
        Collider[] colliders = Physics.OverlapSphere(transform.position, dmgRad);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, dmgRad, 1f, ForceMode.Impulse);
                Debug.Log($"Force applied to {nearbyObject.name}");
            }
            HealthAndDamage player = nearbyObject.GetComponent<HealthAndDamage>();
            EnemHealth enemy = nearbyObject.GetComponent<EnemHealth>();
            



            Vector3 heading = nearbyObject.transform.position - transform.position;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;
            RaycastHit hit;

            if (!Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), direction, out hit, distance, whatIsGround))
            {
                if (player != null)
                {
                    player.TakeDamage(dmgLayer / 5f, transform.position);
                    
                }
                if(enemy != null)
                {
                    if (enemy.GetComponent<ExplodingTarget>() != null)
                    {
                        ExplodingTarget ed = enemy.GetComponent<ExplodingTarget>();
                        StartCoroutine(ed.Explode());
                    }
                    else
                    {
                        

                        if (enemy.health - dmgLayer <= 0f)
                        {
                            AudioManager.instance.Play2DSound(kill, killVol);
                            HitMarker.instance.PlayHitmarker(HitMarker.HitType.Kill);
                        }
                        else
                        {
                            HitMarker.instance.PlayHitmarker(HitMarker.HitType.Normal);
                            AudioManager.instance.Play2DSound(hitSound, hitVol);
                        }
                        enemy.TakeDamage(dmgLayer);
                    }
                    
                }
            }


        }
    }


    

}
