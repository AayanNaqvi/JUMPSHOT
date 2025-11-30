using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using TMPro;


public class KnifeAttack : MonoBehaviour
{

    public float attackDistance = 3f;
    public float attackDelay = 0.3f;
    public float attackSpeed = 0f;
    public int attackDamage = 50;
    public LayerMask whatIsEnemy;

    public bool attacking = false;
    public bool readyToAttack = true;
    int attackCount;

    public TextMeshProUGUI ammoUI;
    public KeyCode attackKey;

    public Camera cam;
    public GameObject hitEffect;

    public Animator animator;
    public Sprite knifeIcon;
    public Sprite knifeIconShadow;
    public Vector3 imageSize;

    private UIElements ui;
    public GameObject canv;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip attack;
    public float volume;
    public AudioClip hitSound;
    public float hitVol;
    public AudioClip kill;
    public float killVol;

    // Start is called before the first frame update
    void Start()
    {
        ui = canv.GetComponent<UIElements>();
    }

    // Update is called once per frame
    void Update()
    {
        volume = EditVolume.weaponVol;
        hitVol = EditVolume.weaponVol;
        killVol = EditVolume.weaponVol;
        
        if(Input.GetKeyDown(attackKey) && !ui.gunMenuActive && !ui.settingsActive)
        {
            Attack();
        }

        ammoUI.SetText("");
    }

    public void Attack()
    {
        if (!readyToAttack || attacking)
        {
            return;
        }


        AudioManager.instance.Play2DSound(attack, volume);
        readyToAttack = false;
        attacking = true;
        animator.SetBool("KnifeAttack", true);

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
        animator.SetBool("KnifeAttack", false);
    }

    void AttackRaycast()
    {
        Vector3 center = cam.transform.position + cam.transform.forward * (attackDistance / 2);
        float radius = 2f; // Wider = more forgiving

        Collider[] hits = Physics.OverlapSphere(center, radius, whatIsEnemy);
        foreach (Collider hit in hits)
        {
            Instantiate(hitEffect, hit.transform.position, Quaternion.identity);

            EnemHealth enemy = hit.GetComponent<EnemHealth>();
            if (enemy != null)
            {
                if(enemy.health - attackDamage <= 0f)
                {
                    HitMarker.instance.PlayHitmarker(HitMarker.HitType.Kill);
                    AudioManager.instance.Play2DSound(kill, killVol);
                }
                else
                {
                    HitMarker.instance.PlayHitmarker(HitMarker.HitType.Normal);
                    AudioManager.instance.Play2DSound(hitSound, hitVol);
                }

                enemy.TakeDamage(attackDamage);
                
            }
        }
    }

    

}
