using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Slider hpSlider;

    public int points;

    [SerializeField] private float respawnRad;
    [SerializeField] private float checkRad;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    public LayerMask obstacleLayers;

    //Cam
    [SerializeField] private Transform playerCamera;
    public GameObject playerBody;
    public TargetSpawning spawner;
    public GameObject deathEffect;

    


    [Header("Sounds")]
    public AudioClip destroy;
    public float destVol;

    


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        playerBody = GameObject.FindWithTag("BodyShot");

        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }

        
        

    }

    // Update is called once per frame
    void Update()
    {
        destVol = EditVolume.enemVol;
        
        hpSlider.value = health;


        if (playerCamera != null)
        {
            hpSlider.transform.LookAt(playerCamera);
            hpSlider.transform.Rotate(0, 180f, 0);
        }

        
    }

    public void TakeDamage(float dmg)
    {
        if(health - dmg <= 0f)
        {
            Die();
        }
        else
        {
            
            health -= dmg;
        }
        
    }

    public void Die()
    {
        health = maxHealth;
        

        if(GetComponent<HealTarget>() != null)
        {
            playerBody.GetComponent<HealthAndDamage>().HealHP(GetComponent<HealTarget>().healAmmount);
        }

        ShootingTarget st = GetComponent<ShootingTarget>();
        if(st != null)
        {
            Destroy(st.currentFlashEffect);
        }
        AudioManager.instance.Play3DSound(destroy, transform.position, destVol);

        playerBody.GetComponent<PointCollector>().addPoints(points);
        GameObject death = Instantiate(deathEffect, transform.position, Quaternion.identity);
        ParticleSystem ps = death.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startSizeMultiplier *= transform.localScale.x / 5f;
        

        var shape = ps.shape;
        shape.radius *= transform.localScale.x / 5f;


        spawner?.NotifyTargetDeath(gameObject);

        Destroy(death,1f);
        Destroy(gameObject);
        



        //Respawn();
        
    }
    

    public void Respawn()
    {
        Vector3 randomPosition;
        int attempts = 0;

        Collider selfCollider = GetComponent<Collider>();
        if (selfCollider != null)
            selfCollider.enabled = false;

        while (true)
        {
            attempts++;
            randomPosition = GetRandomPosition();

            bool isClear = Physics.OverlapSphere(randomPosition, checkRad, obstacleLayers).Length == 0;

            if (isClear)
            {
                Debug.Log($"Found valid position after {attempts} attempts");
                break;
            }

            if (attempts > 1000)
            {
                Debug.LogWarning("Too many attempts — something is wrong.");
                break;
            }

            
        }

        transform.position = randomPosition;
        if (selfCollider != null)
            selfCollider.enabled = true;
    }

    private Vector3 GetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * respawnRad;
        Vector3 origin = transform.position; // Respawns near current position
        float randomHeight = Random.Range(minHeight, maxHeight);
        return new Vector3(origin.x + randomCircle.x, randomHeight, origin.z + randomCircle.y);
    }

    public float getHealth()
    {
        return health;
    }


    public void setHealth(float hp)
    {
        health = hp;
    }
}
