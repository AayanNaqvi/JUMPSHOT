using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetKeyBinds : MonoBehaviour
{

    [Header("Util Key Objects")]
    public GameObject utilThrower;
    public GameObject utilButton1;
    public GameObject utilButton2;
    public GameObject reconButton;
    private ThrowingUtil tU;
    private PlayerRecon pr;

    [Header("Movement Objects")]
    public GameObject playerMover;
    public GameObject slideButton;
    public GameObject jumpButton;
    public GameObject sprintButton;
    public GameObject crouchButton;
    private PlayerMovement pm;
    private Sliding s;

    [Header("Buy Objects")]
    public GameObject buyMenuButton;
    public GameObject buyMenuObj;
    private UIElements ui;

    [Header("WeaponSwitch Objects")]
    public GameObject weaponSwitcher;
    public GameObject primaryButton;
    public GameObject secondaryButton;
    public GameObject knifeButton;
    private WeaponSwitching ws;

    [Header("Gunplay Objects")]
    public GameObject weaponHolder;
    public GameObject attackButton;
    public GameObject aimButton;
    public GameObject reloadButton;
    private KnifeAttack ka;
    private GunSystem gs;
    private AimDownSights ads;

    private void Start()
    {
        tU = utilThrower.GetComponent<ThrowingUtil>();
        pr = utilThrower.GetComponent<PlayerRecon>();
        pm = playerMover.GetComponent<PlayerMovement>();
        s = playerMover.GetComponent<Sliding>();
        ui = buyMenuObj.GetComponent<UIElements>();
        ws = weaponSwitcher.GetComponent<WeaponSwitching>();

        ka = weaponHolder.GetComponentInChildren<KnifeAttack>();
        gs = weaponHolder.GetComponentInChildren<GunSystem>();
        ads = weaponHolder.GetComponentInChildren<AimDownSights>();
    }

    private void Update()
    {
        //Thingy
        ka = weaponHolder.GetComponentInChildren<KnifeAttack>();
        gs = weaponHolder.GetComponentInChildren<GunSystem>();
        ads = weaponHolder.GetComponentInChildren<AimDownSights>();




        //Util
        tU.throwKey1 = utilButton1.GetComponent<KeyBinds>().selectedKey;
        tU.throwKey2 = utilButton2.GetComponent<KeyBinds>().selectedKey;
        pr.reconKey = reconButton.GetComponent<KeyBinds>().selectedKey;

        //Movement
        pm.jumpKey = jumpButton.GetComponent<KeyBinds>().selectedKey;
        s.slideKey = slideButton.GetComponent<KeyBinds>().selectedKey;
        pm.sprintKey = sprintButton.GetComponent<KeyBinds>().selectedKey;
        pm.crouchKey = crouchButton.GetComponent<KeyBinds>().selectedKey;

        //Buy Menu
        ui.gunMenuKey = buyMenuButton.GetComponent<KeyBinds>().selectedKey;

        //Switching
        ws.primary = primaryButton.GetComponent<KeyBinds>().selectedKey;
        ws.secondary = secondaryButton.GetComponent<KeyBinds>().selectedKey;
        ws.knife = knifeButton.GetComponent<KeyBinds>().selectedKey;

        if(gs != null)
        {
            gs.reloadKey = reloadButton.GetComponent<KeyBinds>().selectedKey;
            ads.adsKey = aimButton.GetComponent<KeyBinds>().selectedKey;
            gs.shootKey = attackButton.GetComponent<KeyBinds>().selectedKey;
        }
        else
        {
            ka.attackKey = attackButton.GetComponent<KeyBinds>().selectedKey;
        }

    }
}
