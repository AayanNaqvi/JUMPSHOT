using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThrowingUtil : MonoBehaviour
{

    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow1;
    public GameObject objectToThrow2;
    public Animator animator;
    private UtilityClass util1;
    private UtilityClass util2;
    private GetValueFromDropDown gvfdd;
    public GameObject utilSelect;
    private GetValueFromDropDown gvfdd2;
    public GameObject utilSelect2;
    public TextMeshProUGUI utilCount1;
    public TextMeshProUGUI utilCount2;
    public GameObject uiCanv;
    private UIElements ui;


    [Header("Icons")]
    public GameObject utilIcon1;
    public GameObject utilIcon2;
    public GameObject utilIcon1Shad;
    public GameObject utilIcon2Shad;
    public Sprite grenadeIcon;
    public Sprite grenadeIconShad;
    public Sprite incendiaryIcon;
    public Sprite incendiaryIconShad;
    public Sprite smokeIcon;
    public Sprite smokeIconShad;
    public Sprite flashIcon;
    public Sprite flashIconShad;


    [Header("Utility")]
    public GameObject grenade;
    public GameObject incendiary;
    public GameObject smoke;
    public GameObject flash;


    [Header("Settings")]
    public float throwCooldown1;
    public float throwCooldown2;

    [Header("Throwing")]
    public KeyCode throwKey1 = KeyCode.Q;
    public KeyCode throwKey2 = KeyCode.E;

    private float throwForce1;
    private float throwForce2;
    
    private float throwUpwardForce1;
    private float throwUpwardForce2;

    public bool readyToThrow1;
    public bool readyToThrow2;

    public float numUtil1;
    public float numUtil2;


    [Header("Sound Effects")]
    public AudioClip throwSFX;
    public float throwVolume;
    
    void Start()
    {
        readyToThrow1 = true;
        readyToThrow2 = true;

        util1 = objectToThrow1.GetComponent<UtilityClass>();
        util2 = objectToThrow2.GetComponent<UtilityClass>();

        gvfdd = utilSelect.GetComponent<GetValueFromDropDown>();
        gvfdd2 = utilSelect2.GetComponent<GetValueFromDropDown>();

        ui = uiCanv.GetComponent<UIElements>();
    }

    

    
    void Update()
    {
        throwVolume = EditVolume.utilVol;
        
        PickUtil();
        util1 = objectToThrow1.GetComponent<UtilityClass>();
        util2 = objectToThrow2.GetComponent<UtilityClass>();
        
        SetSettings();

        utilCount1.SetText("x" + numUtil1);
        utilCount2.SetText("x" + numUtil2);


        if (Input.GetKeyDown(throwKey1) && readyToThrow1 && !animator.GetBool("ADS") && !animator.GetBool("Reloading") && !animator.GetBool("Shooting") && numUtil1 > 0 && !ui.gunMenuActive && !ui.settingsActive)
        {
            StartCoroutine(Throw1());
        }

        if (Input.GetKeyDown(throwKey2) && readyToThrow2 && !animator.GetBool("ADS") && !animator.GetBool("Reloading") && !animator.GetBool("Shooting") && numUtil2 > 0 && !ui.gunMenuActive && !ui.settingsActive)
        {
            StartCoroutine(Throw2());
        }


        

    }
    public void PickUtil()
    {
        if (gvfdd.pickedInd == 0)
        {
            objectToThrow1 = grenade;
            utilIcon1.GetComponent<Image>().sprite = grenadeIcon;
            utilIcon1Shad.GetComponent<Image>().sprite = grenadeIconShad;
        }
        else if (gvfdd.pickedInd == 1)
        {
            objectToThrow1 = incendiary;
            utilIcon1.GetComponent<Image>().sprite = incendiaryIcon;
            utilIcon1Shad.GetComponent<Image>().sprite = incendiaryIconShad;
        }
        else if (gvfdd.pickedInd == 2)
        {
            objectToThrow1 = smoke;
            utilIcon1.GetComponent<Image>().sprite = smokeIcon;
            utilIcon1Shad.GetComponent<Image>().sprite = smokeIconShad;

        }
        else if (gvfdd.pickedInd == 3)
        {
            objectToThrow1 = flash;
            utilIcon1.GetComponent<Image>().sprite = flashIcon;
            utilIcon1Shad.GetComponent<Image>().sprite = flashIconShad;
        }


        if (gvfdd2.pickedInd == 0)
        {
            objectToThrow2 = grenade;
            utilIcon2.GetComponent<Image>().sprite = grenadeIcon;
            utilIcon2Shad.GetComponent<Image>().sprite = grenadeIconShad;
        }
        else if (gvfdd2.pickedInd == 1)
        {
            objectToThrow2 = incendiary;
            utilIcon2.GetComponent<Image>().sprite = incendiaryIcon;
            utilIcon2Shad.GetComponent<Image>().sprite = incendiaryIconShad;
        }
        else if (gvfdd2.pickedInd == 2)
        {
            objectToThrow2 = smoke;
            utilIcon2.GetComponent<Image>().sprite = smokeIcon;
            utilIcon2Shad.GetComponent<Image>().sprite = smokeIconShad;
        }
        else if (gvfdd2.pickedInd == 3)
        {
            objectToThrow2 = flash;
            utilIcon2.GetComponent<Image>().sprite = flashIcon;
            utilIcon2Shad.GetComponent<Image>().sprite = flashIconShad;
            
        }

    }
    public void SetSettings()
    {
        throwForce1 = util1.throwForce;
        throwForce2 = util2.throwForce;

        throwUpwardForce1 = util1.throwUpwardForce;
        throwUpwardForce2 = util2.throwUpwardForce;

        throwCooldown1 = util1.throwCooldown;
        throwCooldown2 = util2.throwCooldown;


    }

    private IEnumerator Throw1()
    {
        readyToThrow1 = false;
        animator.SetBool("UtilThrow", true);
        numUtil1 -= 1f;
        AudioManager.instance.Play2DSound(throwSFX, throwVolume);

        GameObject projectile = Instantiate(objectToThrow1, attackPoint.position, cam.rotation);

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        Vector3 forceToAdd = cam.transform.forward * throwForce1 + transform.up * throwUpwardForce1;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);
        animator.SetBool("UtilThrow", false);

        Invoke(nameof(ResetThrow1), throwCooldown1);
        


    }

    private void ResetThrow1()
    {
        readyToThrow1 = true;
    }


    private IEnumerator Throw2()
    {
        readyToThrow2 = false;
        animator.SetBool("UtilThrow", true);
        numUtil2 -= 1f;

        AudioManager.instance.Play2DSound(throwSFX, throwVolume);

        GameObject projectile = Instantiate(objectToThrow2, attackPoint.position, cam.rotation);

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        Vector3 forceToAdd = cam.transform.forward * throwForce2 + transform.up * throwUpwardForce2;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);
        animator.SetBool("UtilThrow", false);

        Invoke(nameof(ResetThrow2), throwCooldown2);



    }

    private void ResetThrow2()
    {
        readyToThrow2 = true;
    }



}
