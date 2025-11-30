using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetValueFromDropDown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int pickedInd;
    public void GetDropDownValue()
    {
        pickedInd = dropdown.value;
        
    }




}
