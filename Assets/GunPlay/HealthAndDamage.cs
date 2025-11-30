using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndDamage : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider hpSlider;
    public Slider shieldSlider;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI shieldText;

    [Header("Audio")]
    public AudioClip hurt;
    public float hurtVolume;
    public AudioClip healHP;
    public float healHPVolume;
    public AudioClip healShield;
    public float healShieldVolume;
    public AudioClip death;
    public float deathVolume;

    [Header("Gameplay Settings")]
    public float maxHP = 100f;
    public float maxShield = 50f;
    public float shieldRegenDelay = 10f;
    public float shieldRegenRate = 5f;
    public float sliderLerpSpeed = 5f;

    [Header("Effects")]
    public GameObject shieldEffect;
    public DamageIndicator damageIndicator;
    public CameraShake cameraShake;
    public float shakeTime;
    public float shakeMagnitude;

    [Header("Respawn")]
    public GameObject playerParent;
    public GameObject respawnAnchor;

    [SerializeField] private float currentHP;
    [SerializeField] private float currentShield;
    private float targetHP;
    private float targetShield;

    private float regenTimer;
    private bool tookDamage;
    
    private bool playedRegenSound;
    public float mollyMultiplier;

    private bool isInMol = false;
    private float mollyDamageTimer = 0f;
    public float mollyInterval = 1f; // every second

    public float currMollyDMG;



    void Start()
    {
        currentHP = targetHP = maxHP;
        currentShield = targetShield = maxShield;
        regenTimer = shieldRegenDelay;
        

        UpdateUIImmediate();
    }

    void Update()
    {
        hurtVolume = EditVolume.playerVol;
        healHPVolume = EditVolume.playerVol;
        healShieldVolume = EditVolume.playerVol;
        deathVolume = EditVolume.playerVol;

        SmoothUpdateSliders();

        if (targetHP <= 0.9f)
        {
            Die();
        }

        if (tookDamage)
        {
            regenTimer = shieldRegenDelay;
            tookDamage = false;
        }
        else
        {
            regenTimer -= Time.deltaTime;

            if (regenTimer <= 0f && targetShield < maxShield)
            {
                RegenerateShield();
            }
        }

        if (isInMol)
        {
            mollyDamageTimer -= Time.deltaTime;
            if (mollyDamageTimer <= 0f)
            {
                mollyDamageTimer = mollyInterval;

                // Deal damage and play sound
                TakeDamage(mollyInterval * mollyMultiplier, transform.position); // consistent DPS
                currMollyDMG = mollyInterval * mollyMultiplier;
                AudioManager.instance.Play2DSound(hurt, hurtVolume);
                StartCoroutine(cameraShake.Shake(shakeTime, shakeMagnitude / 3f));
            }
        }
    }

    public void EnterMolotovFire()
    {
        isInMol = true;
    }

    public void ExitMolotovFire()
    {
        isInMol = false;
        mollyDamageTimer = 0f;
    }
    public void TakeDamage(float damage, Vector3 source)
    {
        AudioManager.instance.Play2DSound(hurt, hurtVolume);
        DamageEffect.ShowVignetteOnHit();
        StartCoroutine(cameraShake.Shake(shakeTime, shakeMagnitude));
        ShowDamageIndicator(source);

        tookDamage = true;
        shieldEffect.SetActive(false);

        if (currentShield > 0)
        {
            float shieldDamage = Mathf.Min(damage, currentShield);
            targetShield -= shieldDamage;
            damage -= shieldDamage;
        }

        if (damage > 0)
        {
            targetHP -= damage;
        }

        targetHP = Mathf.Clamp(targetHP, 0f, maxHP);
        targetShield = Mathf.Clamp(targetShield, 0f, maxShield);
    }

    




    public void HealHP(float amount)
    {
        targetHP = Mathf.Min(targetHP + amount, maxHP);
        AudioManager.instance.Play2DSound(healHP, healHPVolume);
    }

    private void RegenerateShield()
    {
        targetShield += shieldRegenRate * Time.deltaTime;
        targetShield = Mathf.Clamp(targetShield, 0f, maxShield);

        shieldEffect.SetActive(true);

        if (!playedRegenSound)
        {
            AudioManager.instance.Play2DSound(healShield, healShieldVolume);
            playedRegenSound = true;
            Invoke("ResetRegenSound", 20f);
        }
    }

    private void SmoothUpdateSliders()
    {
        // Lerp only the slider visuals
        currentHP = Mathf.Lerp(currentHP, targetHP, Time.deltaTime * sliderLerpSpeed);
        currentShield = Mathf.Lerp(currentShield, targetShield, Time.deltaTime * sliderLerpSpeed);

        hpSlider.value = currentHP;
        shieldSlider.value = currentShield;

        // Update text immediately to reflect actual target values
        hpText.SetText(Mathf.RoundToInt(targetHP).ToString());
        shieldText.SetText(Mathf.RoundToInt(targetShield).ToString());


    }

    private void UpdateUIImmediate()
    {
        hpSlider.value = currentHP;
        shieldSlider.value = currentShield;
        hpText.SetText(Mathf.RoundToInt(currentHP).ToString());
        shieldText.SetText(Mathf.RoundToInt(currentShield).ToString());
    }

    private void ShowDamageIndicator(Vector3 source)
    {
        damageIndicator.damageLocation = source;
        GameObject ind = Instantiate(damageIndicator.gameObject, damageIndicator.transform.position, damageIndicator.transform.rotation, damageIndicator.transform.parent);
        ind.SetActive(true);
    }

    private void Die()
    {
        Respawn();
    }

    private void Respawn()
    {
        targetHP = currentHP = maxHP;
        targetShield = currentShield = maxShield;
        regenTimer = shieldRegenDelay;

        UpdateUIImmediate();
        

        PointCollector pc = GetComponent<PointCollector>();
        AudioManager.instance.Play2DSound(death, deathVolume);
        if(pc.totalPoints - 5000f <= 0.9f)
        {
            pc.totalPoints = 0;
        }
        else
        {
            pc.totalPoints -= 5000f;
        }
    }

    private void ResetMolSound()
    {
        isInMol = false;
    }

    private void ResetRegenSound()
    {
        playedRegenSound = false;
    }
}

