
using EZCameraShake;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GunSystem : MonoBehaviour
{

    public Animator animator;
    public float cost;
    public GameObject knife;
    
    public float damage, headShtDMG;
    public float timeBetweenShooting, spread, adsSpread, range, reloadTime, timeBetweenShots;

    public bool primary, secondary;
    public bool isFiringBurst = false;

    public int magSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    bool shooting;
    public bool readyToShoot;
    public bool reloading;
    public bool unlocked;
    [SerializeField] private bool isShotgun = false;

    public Camera cam;
    public Transform atkPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public CameraShake camShake;
    private UIElements ui;
    private AimDownSights ads;
    public GameObject canvas;
    public Sprite gunIcon;
    public Sprite gunIconShadow;
    public Vector3 imageSize;

    public bool holstered;

    
    public float shakeMagnitude, shakeTime;
    public GameObject muzzleFlash, bulletHole;
    public TrailRenderer bulletTrail;
    

    [Header("KeyBinds")]
    public KeyCode reloadKey;
    public KeyCode shootKey;

    


    private Recoil recoil;

    

    [Header("Text")]
    public TextMeshProUGUI ammoUI;
    public Slider reload;

    
   


    [Header("Sound Effects")]
    [SerializeField] private AudioClip gunShot;
    public float gunShotvolume;
    [SerializeField] private AudioClip reloadSFX;
    public float reloadVolume;
    public AudioClip hitSound;
    public float hitVol;
    public AudioClip weakHitSound;
    public float whitVol;
    public AudioClip kill;
    public float killVol;



    

    private void Start()
    {
        
        ads = GetComponent<AimDownSights>();
        recoil = GetComponent<Recoil>();
        ui = canvas.GetComponent<UIElements>();
        
        
        reload.gameObject.SetActive(false);

        bulletsLeft = magSize;
        readyToShoot = true;
    }

    private void OnEnable()
    {
        

        reloading = false;
        animator.SetBool("Reloading", false);
    }
    private void Update()
    {
        gunShotvolume = EditVolume.weaponVol;
        reloadVolume = EditVolume.weaponVol;
        hitVol = EditVolume.weaponVol;
        whitVol = EditVolume.weaponVol;
        killVol = EditVolume.weaponVol;
        
        if(reloading)
        {
            ads.aiming = false;
        }

        MyInput();

        if(isShotgun)
            ammoUI.SetText(bulletsLeft / bulletsPerTap + " / " + magSize / bulletsPerTap);
        else
        {
            ammoUI.SetText(bulletsLeft + " / " + magSize);
        }

        

    }

    private void MyInput()
    {
        if(LockInputs.inputLocked) return;
        
        if(reloading)
        {
            return;
        }

        if(allowButtonHold)
        {
            shooting = Input.GetKey(shootKey);
        }
        else
        {
            shooting = Input.GetKeyDown(shootKey);
        }


        if (bulletsLeft <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(reloadKey) && bulletsLeft < magSize && !reloading && !ui.gunMenuActive && !animator.GetBool("UtilThrow") && !ui.settingsActive)
        {
            StartCoroutine(Reload());
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0 && !animator.GetBool("Switch") && !ui.gunMenuActive && !animator.GetBool("UtilThrow") && !ui.settingsActive)
        {
            
            bulletsShot = bulletsPerTap;
            AudioManager.instance.Play2DSound(gunShot, gunShotvolume);
            Shoot();
            
            
               
            
            
            
        }

        
        
    }

    private void Shoot()
    {
        if (isFiringBurst) return; // Prevent overlapping bursts

        bulletsShot = bulletsPerTap;
        StartCoroutine(FireBurst());
    }

    private IEnumerator FireBurst()
    {
        isFiringBurst = true;
        readyToShoot = false;
        if (!isShotgun)
            animator.SetBool("Shooting", true);

        int shotsToFire = Mathf.Min(bulletsPerTap, bulletsLeft);

        if (isShotgun)
        {
            animator.SetBool("Shooting", true); // Use Trigger, not Bool
            for (int i = 0; i < shotsToFire; i++)
            {
                FireBullet();
                bulletsLeft--;
            }
            yield return new WaitForSeconds(timeBetweenShooting);
            animator.SetBool("Shooting", false);
        }
        else
        {
            //  BURST / AUTO / SEMI
            AudioManager.instance.Play2DSound(gunShot, gunShotvolume);

            for (int i = 0; i < shotsToFire; i++)
            {
                FireBullet();
                bulletsLeft--;
                bulletsShot--;
                yield return new WaitForSeconds(timeBetweenShots);
            }

            animator.SetBool("Shooting", false);
            yield return new WaitForSeconds(timeBetweenShooting);
        }

        readyToShoot = true;
        isFiringBurst = false;
    }


    private void FireBullet()
    {


        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);
        Vector3 direction = cam.transform.forward + new Vector3(x, y, z);
        Ray ray = new Ray(cam.transform.position, direction);

        RaycastHit[] hits = Physics.RaycastAll(ray, range);
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        float closestWallDist = Mathf.Infinity;
        Dictionary<Transform, (RaycastHit hit, bool isWeakspot)> enemyHits = new();
        

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Wall") && hit.distance < closestWallDist)
            {
                closestWallDist = hit.distance;
            }

            


            if (hit.collider.CompareTag("WeakSpot") || hit.collider.CompareTag("Enemy"))
            {
                Transform root = hit.collider.transform.root;


                bool isWeakspot = hit.collider.CompareTag("WeakSpot");
                if (enemyHits.ContainsKey(root))
                {
                    // Prefer weakspot over body
                    if (isWeakspot)
                        enemyHits[root] = (hit, true);
                }
                else
                {
                    enemyHits[root] = (hit, isWeakspot);
                }
            }
        }

        // From all enemyHits, find the closest one not blocked by wall
        RaycastHit? finalHit = null;
        bool finalIsWeakspot = false;
        float finalDist = Mathf.Infinity;

        foreach (var pair in enemyHits)
        {
            float dist = pair.Value.hit.distance;
            if (dist < finalDist && dist < closestWallDist)
            {
                finalHit = pair.Value.hit;
                finalIsWeakspot = pair.Value.isWeakspot;
                finalDist = dist;
            }
        }

        if (finalHit.HasValue)
        {
            EnemHealth enemy = finalHit.Value.collider.GetComponentInParent<EnemHealth>();
            ExplodingTarget explode = finalHit.Value.collider.GetComponentInParent<ExplodingTarget>();
            
            if (enemy != null)
            {
                if(finalHit.Value.collider.GetComponent<ExplodingTarget>() != null && !finalHit.Value.collider.GetComponent<ExplodingTarget>().exploded)
                {
                    StartCoroutine(finalHit.Value.collider.GetComponent<ExplodingTarget>().Explode());
                }
                
                if (finalIsWeakspot)
                {
                    
                    
                    
                    if (enemy.health - headShtDMG <= 0f)
                    {
                        HitMarker.instance.PlayHitmarker(HitMarker.HitType.Kill);
                        AudioManager.instance.Play2DSound(kill, killVol);
                    }
                    else
                    {
                        HitMarker.instance.PlayHitmarker(HitMarker.HitType.Weakspot);
                        AudioManager.instance.Play2DSound(weakHitSound, whitVol);
                    }
                    enemy.TakeDamage(headShtDMG);
                }
                else
                {
                    
                    
                    

                    if (enemy.health - damage <= 0f)
                    {
                        AudioManager.instance.Play2DSound(kill, killVol);
                        HitMarker.instance.PlayHitmarker(HitMarker.HitType.Kill);
                    }
                    else
                    {
                        HitMarker.instance.PlayHitmarker(HitMarker.HitType.Normal);
                        AudioManager.instance.Play2DSound(hitSound, hitVol *3.5f);
                    }
                    enemy.TakeDamage(damage);

                }
            }

            if (explode != null)
            {
                StartCoroutine(explode.Explode());
            }

            TrailRenderer trail = Instantiate(bulletTrail, atkPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, finalHit.Value));
        }
        else if (closestWallDist != Mathf.Infinity)
        {
            // Only wall hit
            RaycastHit wallHit = Array.Find(hits, h => h.distance == closestWallDist);
            Instantiate(bulletHole, wallHit.point, Quaternion.LookRotation(wallHit.normal));
            TrailRenderer trail = Instantiate(bulletTrail, atkPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, wallHit));
        }
        

        GameObject muzzle = Instantiate(muzzleFlash, atkPoint.position, Quaternion.identity);
        Destroy(muzzle, 0.5f);

        recoil.RecoilFire();
        StartCoroutine(camShake.Shake(shakeTime, shakeMagnitude));
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

    

    IEnumerator Reload()
    {
        reloading = true;
        animator.SetBool("Reloading", true);
        AudioManager.instance.Play2DSound(reloadSFX, reloadVolume);
        reload.gameObject.SetActive(true);
        reload.minValue = 0;
        reload.maxValue = reloadTime;
        reload.value = 0;

        float elapsed = 0f;

        while (elapsed < reloadTime)
        {
            elapsed += Time.deltaTime;
            reload.value = elapsed;

            yield return null; // This is critical!
        }

        
        animator.SetBool("Reloading", false);
        //yield return new WaitForSeconds(reloadTime - .25f);
        reload.gameObject.SetActive(false);


        bulletsLeft = magSize;
        
        reloading = false;
    }

    

}


