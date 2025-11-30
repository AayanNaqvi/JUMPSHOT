using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource globalAudioSourcePrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        
    }

    public void Play2DSound(AudioClip clip, float volume)
    {
        AudioSource src = Instantiate(globalAudioSourcePrefab, transform);
        src.clip = clip;
        src.volume = volume;
        src.spatialBlend = 0f; // 2D
        src.Play();
        Destroy(src.gameObject, clip.length);
    }

    public void Play3DSound(AudioClip clip, Vector3 position, float volume)
    {
        GameObject go = new GameObject("3D Sound");
        go.transform.position = position;

        AudioSource src = go.AddComponent<AudioSource>();
        src.clip = clip;
        src.volume = volume;
        src.spatialBlend = 1f; // 3D
        src.minDistance = 15f;
        src.maxDistance = 50f;
        src.Play();

        Destroy(go, clip.length);
    }
}