using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MoveArms : MonoBehaviour
{

    public TwoBoneIKConstraint leftArm;
    public GameObject leftArmObj;
    public GameObject inventory;
    private GunSystem gs;
    public GameObject pistolGrip;
    public GameObject aRGrip;

    public GameObject gunHolder;
    private WeaponSwitching ws;

    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        ws = gunHolder.GetComponent<WeaponSwitching>();

        leftArm = leftArmObj.GetComponent<TwoBoneIKConstraint>();
        
        if(ws.selectedWeapon != 2)
        {
            gs = inventory.GetComponentInChildren<GunSystem>();
        }
        
        

       
        


        


    }

    // Update is called once per frame
    void Update()
    {
        if (ws.selectedWeapon != 2)
        {
            gs = inventory.GetComponentInChildren<GunSystem>();
        }


        if (ws.selectedWeapon == 2)
        {
            leftArm.weight = 0.7f;
            animator.SetBool("Secondary", false);
        }
        else
        {
            if(gs.secondary)
            {
                leftArm.weight = 1f;
                animator.SetBool("Secondary", true);

            }
            else if(gs.primary)
            {
                leftArm.weight = 1f;
                animator.SetBool("Secondary", false);
            }
            
        }
    }
}
