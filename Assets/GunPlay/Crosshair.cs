using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private RectTransform crosshair;

    public float restingSize;
    public float maxSize;
    
    public float speed;
    private float currentSize;
    


    public GameObject gun;
    private GunSystem gs;

    public GameObject gunHolder;
    private WeaponSwitching ws;
    private AimDownSights ads;



    private void Start()
    {
        
        ws = gunHolder.GetComponent<WeaponSwitching>();

        if(ws.selectedWeapon != 2)
        {
            gs = gun.GetComponentInChildren<GunSystem>();
            ads = gun.GetComponentInChildren<AimDownSights>();
        }
        
        crosshair = GetComponent<RectTransform>();
    }

    private void Update()
    {



        if (ws.selectedWeapon != 2)
        {
            gs = gun.GetComponentInChildren<GunSystem>();
            ads = gun.GetComponentInChildren<AimDownSights>();


            if (!gs.readyToShoot)
            {
                if(!ads.aiming)
                {
                    currentSize = Mathf.Lerp(currentSize, gs.spread * 1000f + 50f, Time.deltaTime * speed);
                }
                else
                {
                    currentSize = Mathf.Lerp(currentSize, gs.spread * 1000f + 25f, Time.deltaTime * speed);
                }
                
            }
            else
            {
                currentSize = Mathf.Lerp(currentSize, gs.spread * 1000f, Time.deltaTime * speed);
            }
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, 20f, Time.deltaTime * speed);
        }
        


        crosshair.sizeDelta = new Vector2 (currentSize, currentSize);
    }

    

}
