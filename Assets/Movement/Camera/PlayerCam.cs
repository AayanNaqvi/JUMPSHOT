using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    private float currSensX;
    private float currSensY;

    public float adsSensX;
    public float adsSensY;

    public Transform orientation;
    public Transform camHolder;
    public Transform playerBody;

    private AimDownSights ads;
    public GameObject gunHolder;
    

    float xRotation;
    float yRotation;

    private UIElements ui;
    public GameObject canvas;

    public Slider sensSlider;
    public TextMeshProUGUI sensText;
    public Slider adsSlider;
    public TextMeshProUGUI adsText;





    private void Start()
    {
        currSensX = sensX;
        currSensY = sensY;

        sensSlider.value = sensX;
        sensText.SetText("SENSITIVITY: " + Mathf.RoundToInt(sensX));

        adsSlider.value = adsSensX;
        adsText.SetText("AIM SENSITIVITY: " + Mathf.RoundToInt(adsSensX));

        ads = gunHolder.GetComponentInChildren<AimDownSights>();






        ui  = canvas.GetComponent<UIElements>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {

        sensX = sensSlider.value;
        sensY = sensSlider.value;
        sensText.SetText("SENSITIVITY: " + Mathf.RoundToInt(sensX));

        adsSensX = adsSlider.value;
        adsSensX = adsSlider.value;
        adsText.SetText("AIM SENSITIVITY: " + Mathf.RoundToInt(adsSensX));


        ads = gunHolder.GetComponentInChildren<AimDownSights>();

        if(ads != null)
        {
            if (ads.aiming)
            {
                currSensX = adsSensX;
                currSensY = adsSensY;
            }
            else
            {
                currSensX = sensX;
                currSensY = sensY;
            }
        }
        
        

        if (!ui.gunMenuActive && !ui.settingsActive)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * currSensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * currSensY;

            yRotation += mouseX;
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);


            camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            playerBody.rotation = Quaternion.Euler(0, yRotation, 0);


        }


        
        
    }

    public void DoFov(float endValue, float tranTime)
    {
        

        GetComponent<Camera>().DOFieldOfView(endValue, tranTime);
    }
    public void DoTilt(float zTilt, float tranTime)
    {
        

        transform.DOLocalRotate(new Vector3(0, 0, zTilt), tranTime);
    }
}
