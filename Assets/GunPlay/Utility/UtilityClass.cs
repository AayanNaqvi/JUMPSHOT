using UnityEngine;

  [System.Serializable]   
public class UtilityClass : MonoBehaviour
{
    public float throwForce;
    public float throwUpwardForce;
    public float throwCooldown;

    [Header("Sound Effects")]
    public AudioClip collide;
    public float colVol;

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.instance.Play3DSound(collide, transform.position, colVol);
    }

    private void Update()
    {
        colVol = EditVolume.utilVol;
    }
}



