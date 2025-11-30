using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;

public class UIElements : MonoBehaviour
{

    public bool gunMenuActive = false;
    public KeyCode gunMenuKey = KeyCode.B;
    public GameObject gunMenu;

    public bool settingsActive = false;
    public KeyCode settingsKey = KeyCode.Escape;
    public GameObject settingMenu;

    public GameObject audioButton;
    public GameObject sensStuff;
    public GameObject controlsStuff;
    public GameObject audioStuff;
    public GameObject otherStuff;
    private bool prevSettingsActive = false;

    public GameObject gun;
    private GunSystem gs;
    private WeaponSwitching ws;
    public GameObject knife;

    

    [Header("Sound")]
    public AudioClip open;
    public float openVol;


    // Update is called once per frame
    void Update()
    {
        openVol = EditVolume.envVol;
        ws = gun.GetComponent<WeaponSwitching>();
        if(ws.selectedWeapon != 2)
            gs = gun.GetComponentInChildren<GunSystem>();
        
        
        
        

            MyInput();
        if(gunMenuActive && !settingsActive)
        {
            gunMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(settingsActive && !gunMenuActive)
        {
            settingMenu.SetActive(true);
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (!prevSettingsActive)
            {
                EventSystem.current.SetSelectedGameObject(null); // clear first
                EventSystem.current.SetSelectedGameObject(audioButton); // auto-select
            }

        }
        else
        {
            gunMenu.SetActive(false);
            controlsStuff.SetActive(false);
            sensStuff.SetActive(false);
            otherStuff.SetActive(false);
            
            settingMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        prevSettingsActive = settingsActive;






    }

    private void MyInput()
    {

        if (Input.GetKeyDown(settingsKey) && !settingsActive)
        {
            audioStuff.SetActive(true);
        }
        

        if (Input.GetKeyDown(gunMenuKey) && !settingsActive && ((gs.readyToShoot && !gs.reloading)))
        {
            gunMenuActive = !gunMenuActive;
            AudioManager.instance.Play2DSound(open, openVol);
        }
        if (Input.GetKeyDown(settingsKey) && !gunMenuActive && gs.readyToShoot && !gs.reloading)
        {
            settingsActive = !settingsActive;
            AudioManager.instance.Play2DSound(open, openVol);
        }

        
    }
}
