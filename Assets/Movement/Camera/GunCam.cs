using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GunCam : MonoBehaviour
{
    


    public void DoFov(float endValue, float tranTime)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, tranTime);
    }
    public void DoTilt(float zTilt, float tranTime)
    {


        transform.DOLocalRotate(new Vector3(0, 0, zTilt), tranTime);
    }
}
