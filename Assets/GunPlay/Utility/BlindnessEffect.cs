using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class BlindnessEffect : MonoBehaviour
{

    [SerializeField]private Image img;
    private Animator animator;

    private int width, height;  

    public static BlindnessEffect activeInstance;

    [Header("Sound")]
    public AudioClip blindsfx;
    public float blivol;


    // Start is called before the first frame update
    void Start()
    {
        activeInstance = this;

        width = Screen.width;
        height = Screen.height;
        animator = GetComponent<Animator>();
    }

    
    

    public void GoBlind()
    {
        StartCoroutine(Blinder());
    }
    public IEnumerator Blinder()
    {
        yield return new WaitForEndOfFrame();

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0,0, width, height), 0, 0);
        tex.Apply();


        img.sprite = Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 100);
        animator.SetTrigger("GoBlind");
        AudioManager.instance.Play2DSound(blindsfx, blivol);
    }
}
