
using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    public Animator animator;

    public GameObject gun;
    private GunSystem gs;


    private UIElements ui;
    public GameObject canvar;

    public Image weaponIcon;
    public Image weaponIconShadow;

    public Image weapon2Icon;
    public Image weapon2IconShadow;

    public Image weapon3Icon;
    public Image weapon3IconShadow;

    public KeyCode primary;
    public KeyCode secondary;
    public KeyCode knife;

    public GameObject[] guns;
    public Button[] gunButtons;

    private PointCollector pc;
    public GameObject player;

    [SerializeField] private AudioClip switchPrim;
    public float switchPrimVol;
    [SerializeField] private AudioClip switchSec;
    public float switchSecVol;
    [SerializeField] private AudioClip switchKni;
    public float switchKniVol;




    // Start is called before the first frame update
    void Start()
    {
        ui = canvar.GetComponent<UIElements>();
        pc = player.GetComponent<PointCollector>();

        if (selectedWeapon != 2)
        {
            gs = gun.GetComponentInChildren<GunSystem>();
        }
        
        SelectWeapon();

        
    }

    // Update is called once per frame
    void Update()
    {
        switchKniVol = EditVolume.weaponVol;
        switchPrimVol = EditVolume.weaponVol;
        switchSecVol = EditVolume.weaponVol;

        if(animator.GetBool("Switch"))
        {
            return;
        }
        
        
        if (selectedWeapon != 2)
        {
            gs = gun.GetComponentInChildren<GunSystem>();
        }

        if(gs.reloading)
        {
            return;
        }

        int previousSelectedWeapon = selectedWeapon;

        if (!animator.GetBool("KnifeAttack") && !ui.gunMenuActive && !animator.GetBool("Switch") && !ui.settingsActive)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && gs.readyToShoot)
            {
                if (selectedWeapon >= 2)
                {
                    
                    selectedWeapon = 0;
                }
                else
                {
                    
                    selectedWeapon++;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f && gs.readyToShoot)
            {
                if (selectedWeapon <= 0)
                {
                    
                    selectedWeapon = 2;
                }
                else
                {
                    
                    selectedWeapon--;
                }
            }


            if (Input.GetKeyDown(primary) && gs.readyToShoot)
            {
                
                selectedWeapon = 0;

            }
            if (Input.GetKeyDown(secondary) && transform.childCount >= 2 && gs.readyToShoot)
            {
                
                selectedWeapon = 1;
            }
            if (Input.GetKeyDown(knife) && transform.childCount >= 3 && gs.readyToShoot)
            {
                
                selectedWeapon = 2;
            }

            if (previousSelectedWeapon != selectedWeapon)
            {
                
                SelectWeapon();
            }
        }

        if (selectedWeapon != 2)
        {
            weaponIcon.sprite = GetComponentInChildren<GunSystem>().gunIcon;
            weaponIcon.GetComponent<RectTransform>().localScale = GetComponentInChildren<GunSystem>().imageSize;
            weaponIconShadow.sprite = GetComponentInChildren<GunSystem>().gunIconShadow;
            weaponIconShadow.GetComponent<RectTransform>().localScale = GetComponentInChildren<GunSystem>().imageSize;


            if (selectedWeapon == 0)
            {
                weapon2Icon.sprite = gun.transform.GetChild(1).GetComponent<GunSystem>().gunIcon;
                weapon2Icon.GetComponent<RectTransform>().localScale = gun.transform.GetChild(1).GetComponent<GunSystem>().imageSize;
                weapon2IconShadow.sprite = gun.transform.GetChild(1).GetComponent<GunSystem>().gunIconShadow;
                weapon2IconShadow.GetComponent<RectTransform>().localScale = gun.transform.GetChild(1).GetComponent<GunSystem>().imageSize;

                weapon3Icon.sprite = gun.transform.GetChild(2).GetComponent<KnifeAttack>().knifeIcon;
                weapon3Icon.GetComponent<RectTransform>().localScale = gun.transform.GetChild(2).GetComponent<KnifeAttack>().imageSize;
                weapon3IconShadow.sprite = gun.transform.GetChild(2).GetComponent<KnifeAttack>().knifeIconShadow;
                weapon3IconShadow.GetComponent<RectTransform>().localScale = gun.transform.GetChild(2).GetComponent<KnifeAttack>().imageSize;
            }
            else if(selectedWeapon == 1)
            {
                weapon2Icon.sprite = gun.transform.GetChild(0).GetComponent<GunSystem>().gunIcon;
                weapon2Icon.GetComponent<RectTransform>().localScale = gun.transform.GetChild(0).GetComponent<GunSystem>().imageSize;
                weapon2IconShadow.sprite = gun.transform.GetChild(0).GetComponent<GunSystem>().gunIconShadow;
                weapon2IconShadow.GetComponent<RectTransform>().localScale = gun.transform.GetChild(0).GetComponent<GunSystem>().imageSize;

                weapon3Icon.sprite = gun.transform.GetChild(2).GetComponent<KnifeAttack>().knifeIcon;
                weapon3Icon.GetComponent<RectTransform>().localScale = gun.transform.GetChild(2).GetComponent<KnifeAttack>().imageSize;
                weapon3IconShadow.sprite = gun.transform.GetChild(2).GetComponent<KnifeAttack>().knifeIconShadow;
                weapon3IconShadow.GetComponent<RectTransform>().localScale = gun.transform.GetChild(2).GetComponent<KnifeAttack>().imageSize;
            }
        
            




        }
        else
        {
            weaponIcon.sprite = guns[2].GetComponent<KnifeAttack>().knifeIcon;
            weaponIcon.GetComponent<RectTransform>().localScale = guns[2].GetComponent<KnifeAttack>().imageSize;
            weaponIconShadow.sprite = guns[2].GetComponent<KnifeAttack>().knifeIconShadow;
            weaponIconShadow.GetComponent<RectTransform>().localScale = guns[2].GetComponent<KnifeAttack>().imageSize;

            weapon2Icon.sprite = gun.transform.GetChild(1).GetComponent<GunSystem>().gunIcon;
            weapon2Icon.GetComponent<RectTransform>().localScale = gun.transform.GetChild(1).GetComponent<GunSystem>().imageSize;
            weapon2IconShadow.sprite = gun.transform.GetChild(1).GetComponent<GunSystem>().gunIconShadow;
            weapon2IconShadow.GetComponent<RectTransform>().localScale = gun.transform.GetChild(1).GetComponent<GunSystem>().imageSize;

            weapon3Icon.sprite = gun.transform.GetChild(0).GetComponent<GunSystem>().gunIcon;
            weapon3Icon.GetComponent<RectTransform>().localScale = gun.transform.GetChild(0).GetComponent<GunSystem>().imageSize;
            weapon3IconShadow.sprite = gun.transform.GetChild(0).GetComponent<GunSystem>().gunIconShadow;
            weapon3IconShadow.GetComponent<RectTransform>().localScale = gun.transform.GetChild(0).GetComponent<GunSystem>().imageSize;
        }

        
        

        foreach(Button b in gunButtons)
        {
            if(!b.GetComponent<ButtonCost>().unlocked && pc.totalPoints < b.GetComponent<ButtonCost>().cost)
            {
                b.interactable = false;
            }
            else
            {
                b.interactable = true;
            }
        }
    }
    
    void SelectWeapon()
    {
        
        int i = 0;
        //StartCoroutine(WeaponSwitchOut());
        foreach (Transform weapon in transform)
        {
            if(i == selectedWeapon)
            {
                
                
                
                
                
                StartCoroutine(WeaponEquip());
                weapon.gameObject.SetActive(true);




                // 🟡 Play switch sound based on selected weapon
                if (selectedWeapon == 0)
                {
                    AudioManager.instance.Play2DSound(switchPrim, switchPrimVol);
                }
                else if (selectedWeapon == 1)
                {
                    AudioManager.instance.Play2DSound(switchSec, switchSecVol);
                }
                else if (selectedWeapon == 2)
                {
                    AudioManager.instance.Play2DSound(switchKni, switchKniVol);
                }

                if (selectedWeapon != 2)
                {
                    weaponIcon.sprite = GetComponentInChildren<GunSystem>().gunIcon;
                    weaponIcon.GetComponent<RectTransform>().localScale = GetComponentInChildren<GunSystem>().imageSize;
                    weaponIconShadow.sprite = GetComponentInChildren<GunSystem>().gunIconShadow;
                    weaponIconShadow.GetComponent<RectTransform>().localScale = GetComponentInChildren<GunSystem>().imageSize;
                }
                



            }
            else
            {
                
                weapon.gameObject.SetActive(false);
                
            }
            i++;
        }
    }

    IEnumerator WeaponEquip()
    {
        
        animator.SetBool("Switch", true);

        

        yield return new WaitForSeconds(0.3f);
        
        animator.SetBool("Switch", false);
    }

    




    public void SwapWeaponSecondary(GameObject swapGun)
    {
        if (swapGun.GetComponent<GunSystem>() != null)
        {
            if (swapGun.GetComponent<GunSystem>().cost > pc.totalPoints && !swapGun.GetComponent<GunSystem>().unlocked)
            {
                return;
            }
        }


        //Transform newGun = gun.transform.GetChild();
        
        if (!swapGun.GetComponent<GunSystem>().unlocked)
        {
            pc.totalPoints -= swapGun.GetComponent<GunSystem>().cost;
            swapGun.GetComponent<GunSystem>().unlocked = true;
        }
        

        swapGun.transform.SetSiblingIndex(1);


        

        selectedWeapon = 1;
        SelectWeapon();

    }

    public void SwapOutSecondary(GameObject oldGun)
    {
        




        oldGun.transform.SetSiblingIndex(4);
    }

    public void SwapWeaponPrimary(GameObject swapGun)
    {
        if(swapGun.GetComponent<GunSystem>() != null)
        {
            if(swapGun.GetComponent<GunSystem>().cost > pc.totalPoints && !swapGun.GetComponent<GunSystem>().unlocked)
            {
                return;
            }
        }


        if (!swapGun.GetComponent<GunSystem>().unlocked)
        {
            pc.totalPoints -= swapGun.GetComponent<GunSystem>().cost;
            swapGun.GetComponent<GunSystem>().unlocked = true;
        }
        swapGun.transform.SetSiblingIndex(0);

        
        selectedWeapon = 0;

        SelectWeapon();
    }

    public void SwapOutPrimary(GameObject oldGun)
    {
        

        oldGun.transform.SetSiblingIndex(5);
    }



}
