using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ShootingTarget : MonoBehaviour
{
    [SerializeField] private float range;
    public bool enraged = false;
    public bool isBlinded = false;
    public bool readyToFire = true;
    public GameObject currentFlashEffect;
    [SerializeField] private float damage;
    [SerializeField] private float fireRate;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private LayerMask whatIsPlayer;
    public LayerMask whatIsGround;

    private Coroutine lookCoroutine;
    private Transform currentTarget;

    public bool aggression;


    [Header("Effects")]
    public TrailRenderer bulletTrail;
    public GameObject eyeAnchor;

    public GameObject flashedEffect;


    private void Start()
    {
        Invoke("TurnOnAgr", 2.5f);
        currentFlashEffect = null;
    }

    private void Update()
    {
        if (!aggression)
            return;
        
        
        Collider[] hits = null;

        if (!isBlinded)
        {
            hits = CheckForPlayer();
        }
            
        
        if (hits != null && hits.Length > 0)
        {
            currentTarget = hits[0].transform;

            if (lookCoroutine == null)
            {
                lookCoroutine = StartCoroutine(SmoothLookAt());
            }

            if (readyToFire)
            {
                ShootAtPlayer(hits);
            }
        }
        else
        {
            currentTarget = null;

            if (lookCoroutine != null)
            {
                StopCoroutine(lookCoroutine);
                lookCoroutine = null;
            }
        }

        Debug.DrawRay(eyeAnchor.transform.position, eyeAnchor.transform.forward * 3, Color.red);
    }

    private void TurnOnAgr()
    {
        aggression = true;
    }

    private void ShootAtPlayer(Collider[] hit)
    {
        readyToFire = false;
        Vector3 dirToPlayer = (hit[0].transform.position - transform.position).normalized;

        if (Physics.Raycast(new Ray(transform.position, dirToPlayer), out RaycastHit hitter, range))
        {
            if (hitter.collider.CompareTag("BodyShot"))
            {
                if (!Physics.Raycast(transform.position, dirToPlayer, out RaycastHit wallHit, Vector3.Distance(transform.position, hitter.point), whatIsGround))
                {
                    // Start smooth eye look
                    if (lookCoroutine != null) StopCoroutine(lookCoroutine);
                    lookCoroutine = StartCoroutine(SmoothLookAt());

                    // Damage and bullet effect
                    hitter.collider.GetComponent<HealthAndDamage>()?.TakeDamage(damage, transform.position);
                    TrailRenderer trail = Instantiate(bulletTrail, transform.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, hitter));
                }
            }
        }

        Invoke("ResetShot", fireRate);
    }
    private IEnumerator SmoothLookAt()
    {
        while (currentTarget != null)
        {
            Vector3 dir = currentTarget.position - eyeAnchor.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            //eyeAnchor.transform.rotation = Quaternion.Slerp(eyeAnchor.transform.rotation, targetRotation * Quaternion.Euler(0, 180, 0), rotationSpeed * Time.deltaTime);
            eyeAnchor.transform.rotation = Quaternion.Slerp(
                eyeAnchor.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            yield return null;
        }
    }


    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (trail.transform.position != hit.point)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;


        Destroy(trail.gameObject, trail.time);
    }


    private void ResetShot()
    {
        readyToFire = true;
    }


    private Collider[] CheckForPlayer()
    {
        
        Collider[] hits = Physics.OverlapSphere(transform.position, range, whatIsPlayer);

        if (hits.Length > 0)
        {
            enraged = true;
        }
        else
        {
            enraged = false;
        }
        return hits;
    }


    
    public IEnumerator GetBlinded(float duration)
    {
        isBlinded = true;
        readyToFire = false;
        currentFlashEffect = Instantiate(flashedEffect, transform.position, Quaternion.identity);
        
            

        Destroy(currentFlashEffect, duration - 0.5f);
      

        yield return new WaitForSeconds(duration);

        isBlinded = false;
        readyToFire = true;
    }
   

    

}
