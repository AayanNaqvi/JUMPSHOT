using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Vector3 damageLocation;
    public GameObject playerObj;

    private CanvasGroup dmgImgCanvas;
    public float fadeStartTime, fadeTime;
    private float maxFadeTime;


    private void Start()
    {
        maxFadeTime = fadeTime;
        dmgImgCanvas = GetComponent<CanvasGroup>();
        
    }

    private void Update()
    {
        
        
        
            if (fadeStartTime > 0)
            {
                fadeStartTime -= Time.deltaTime;
            }
            else
            {


                fadeTime -= Time.deltaTime;
                dmgImgCanvas.alpha = fadeTime / maxFadeTime;
                if (fadeTime <= 0)
                {
                    Destroy(this.gameObject);
                }



            }
        
        


        
        
            damageLocation.y = playerObj.transform.position.y;

            Vector3 direction = (damageLocation - playerObj.transform.position).normalized;
            float angle = Vector3.SignedAngle(direction, playerObj.transform.forward, Vector3.up);
            transform.localEulerAngles = new Vector3(0, 0, angle);
        
    }
        



}
