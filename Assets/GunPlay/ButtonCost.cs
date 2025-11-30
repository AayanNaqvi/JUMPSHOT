using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCost : MonoBehaviour
{
    public float cost;
    public bool unlocked;
    public GameObject gun;
    public TextMeshProUGUI namer;
    public string gunName;

    private void Update()
    {
        unlocked = gun.GetComponent<GunSystem>().unlocked;
        cost = gun.GetComponent<GunSystem>().cost;
        if(unlocked)
        {
            namer.SetText(gunName);
        }
        else
        {
            namer.SetText(gunName + " (" + cost + ")");
        }
    }
}
