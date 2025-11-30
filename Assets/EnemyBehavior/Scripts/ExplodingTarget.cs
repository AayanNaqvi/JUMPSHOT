using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingTarget : MonoBehaviour
{

    public float maxExplosionDamage;
    public float explosionRadius;
    public LayerMask damageableLayers;

    

   

    public GameObject explosionEffect;
    public GameObject playerBody;
    public GameObject fireEmit;

    public bool exploded = false;

    [Header("Sound")]
    public AudioClip explode;
    public float expVol;

    private void Start()
    {
        playerBody = GameObject.FindWithTag("BodyShot");
        
        
    
    }

    private void Update()
    {
        expVol = EditVolume.enemVol;
    }


    void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("Player"))
        {
            StartCoroutine(Explode());
        }
    }

    public void TakeDamage(float dmg)
    {
        if (!exploded)
        {
            StartCoroutine(Explode());
        }
    }

    public IEnumerator Explode()
    {
        fireEmit.SetActive(false);
        exploded = true;
        Debug.Log("Explode");

        // 1. Visual effect
        GameObject exploder = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        AudioManager.instance.Play3DSound(explode, transform.position, expVol);

        // 2. AoE damage check
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider col in hits)
        {
            // 3. Skip self
            if (col.gameObject == gameObject)
                continue;

            Vector3 targetPos = col.bounds.center;
            float distance = Vector3.Distance(transform.position, targetPos);

            // 4. Check line of sight (raycast from explosion to target)
            Vector3 dir = (targetPos - transform.position).normalized;
            if (Physics.Raycast(transform.position, dir, out RaycastHit hitInfo, explosionRadius))
            {
                if (hitInfo.collider != col) continue; // Blocked by something

                float damagePercent = Mathf.Clamp01(1f - (distance / explosionRadius));
                float damage = maxExplosionDamage * damagePercent;

                // 5. Damage player
                if (col.CompareTag("BodyShot"))
                {
                    Debug.Log($"Explosion hit player: {damage:F1} dmg at {distance:F2} units");
                    col.GetComponent<HealthAndDamage>()?.TakeDamage(damage, transform.position);
                }

                // 6. Damage other enemies
                else if (col.CompareTag("Enemy"))
                {
                    Debug.Log($"Explosion hit enemy: {damage:F1} dmg at {distance:F2} units");
                    col.GetComponent<EnemHealth>()?.TakeDamage(damage);
                }
            }
        }

        // 7. Respawn logic
        GetComponent<EnemHealth>()?.Die();
        yield return new WaitForSeconds(0.2f);
        fireEmit.SetActive(true);
        //Destroy(gameObject);

        // 8. Clean up explosion effect
        Destroy(exploder, 5f);
    }





}
