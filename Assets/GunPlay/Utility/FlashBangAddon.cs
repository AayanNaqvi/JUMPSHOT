using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FlashBangAddon : MonoBehaviour
{
    
    public float fuseTime = 3f;
    public GameObject explosion;
    public float flashTime;
    public float radius;

    public LayerMask whatIsEnemy;
    public LayerMask whatIsObstacle;

    private Camera cam;

    [Header("Sound")]
    public AudioClip flashSound;
    public float flashVolume;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Explode", fuseTime);
        cam = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        flashVolume = EditVolume.utilVol;
    }

    public void Explode()
    {
        AudioManager.instance.Play3DSound(flashSound, transform.position, flashVolume);
        Vector3 dirToFlash = (transform.position - cam.transform.position).normalized;
        float distance = Vector3.Distance(transform.position, cam.transform.position);

        // 1. Check if there's no obstacle between player and flashbang
        if (!Physics.Raycast(cam.transform.position, dirToFlash, distance, whatIsObstacle))
        {
            // 2. Check if flashbang is within the camera's field of view
            float dot = Vector3.Dot(cam.transform.forward, dirToFlash); // 1 = directly in front, -1 = directly behind

            // Set your field of view threshold
            if (dot > 0.3f) // Adjust this threshold if needed (0.5 ≈ 60° cone in front)
            {
                BlindnessEffect.activeInstance.GoBlind();
            }
        }


        StartCoroutine(CheckForTargets(radius, flashTime * 2f));

        Destroy(gameObject);

        
        Destroy(Instantiate(explosion, transform.position, Quaternion.identity), flashTime);
    }

   

    public IEnumerator CheckForTargets( float radius, float blindDuration)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, whatIsEnemy);

        foreach (Collider hit in hits)
        {
            Vector3 dirToTarget = (hit.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, hit.transform.position);

            // Raycast to check if flash has line of sight
            if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, whatIsObstacle))
            {
                // Optional: also check target has a component that can be blinded
                ShootingTarget target = hit.GetComponent<ShootingTarget>();
                if (target != null)
                {
                    target.StartCoroutine(target.GetBlinded(blindDuration));
                }
            }
        }

        yield return null;
    }
}
