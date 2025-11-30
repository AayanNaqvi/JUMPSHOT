using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AimDownSights : MonoBehaviour
{
   

    [Header("References")]
    public PlayerCam cam;
    public GunCam gunCamera;
    private GunSystem gs;
    public Animator animator;
    public GameObject player;
    private PlayerMovement pm;
    public GameObject sniperScope;
    public GameObject gunCam;
    private UIElements ui;
    public GameObject canvas;

    
    
    public KeyCode adsKey = KeyCode.Mouse1;

    public bool scopedWeapon;
    public bool aiming = false;
    private float regSpread;
    

    [Header("Sound Effects")]
    [SerializeField] private AudioClip aimIn;
    public float aimVolume;




    // Start is called before the first frame update
    void Start()
    {
        
        ui = canvas.GetComponent<UIElements>();
        gs = GetComponent<GunSystem>();
        pm = player.GetComponent<PlayerMovement>();
        regSpread = gs.spread;

    }

    // Update is called once per frame
    void Update()
    {
        aimVolume = EditVolume.weaponVol;
        if (LockInputs.inputLocked) return;

        bool prevAiming = aiming;

        MyInput();
        if (aiming && !prevAiming)
        {
            AudioManager.instance.Play2DSound(aimIn, aimVolume);
        }



        if (!scopedWeapon)
        {
            gunCam.SetActive(true);
            sniperScope.SetActive(false);

            if (aiming)
            {
                animator.SetBool("ADS", true);
                gs.spread = gs.adsSpread;
                cam.DoFov(30, 0.15f);
                gunCamera.DoFov(15, 0.2f);
                return;

            }
            else
            {
                animator.SetBool("ADS", false);
                gs.spread = regSpread;
                cam.DoFov(60, 0.15f);
                gunCamera.DoFov(60, 0.2f);
                return;
            }
        }
        else
        {
            if (aiming && gs.readyToShoot)
            {
                StartCoroutine(ScopeIn());
                gs.spread = gs.adsSpread;
               
                return;

            }
            else
            {
                ScopeOut();
                gs.spread = regSpread;
                cam.DoFov(60, 0.15f);
                gunCamera.DoFov(60, 0.2f);

            }

            
        }
        

        if(gs.reloading)
        {
            aiming = false;
        }


        
    }

    private void MyInput()
    {

        if (LockInputs.inputLocked) return;

        if (!scopedWeapon)
        {
            if (Input.GetKeyDown(adsKey) && !gs.reloading && !ui.settingsActive)
            {
                aiming = true;
            }
            else if (Input.GetKeyUp(adsKey) && !gs.reloading)
            {
                aiming = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(adsKey) && !gs.reloading && !ui.settingsActive)
            {
                aiming = !aiming;
            }
            
        }
        

        
    }

    void ScopeOut()
    {
        animator.SetBool("ADS", false);
        aiming = false;
        gunCam.SetActive(true);
        sniperScope.SetActive(false);
    }

    IEnumerator ScopeIn()
    {
        
        animator.SetBool("ADS", true);
        cam.DoFov(15, 0.15f);
        gunCamera.DoFov(15, 0.2f);
        yield return new WaitForSeconds(.15f);
        sniperScope.SetActive(true);
        gunCam.SetActive(false);

    }
}
