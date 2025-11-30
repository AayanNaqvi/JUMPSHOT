using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFireHitbox : MonoBehaviour
{
    public GameObject hitBox;
    

    // Update is called once per frame
    void Update()
    {
        Destroy(hitBox, 12f);
    }
}
