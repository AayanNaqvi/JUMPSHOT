using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public GameObject firstSelectedButton;
    public AudioClip menuNav;
    public float menuVol;


    [Header("Background Music")]
    public AudioSource menuMusic; // Assign your main menu AudioSource here
    public float fadeDuration = 1f; // Time to fade out

    IEnumerator Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        // Wait one frame to let scene load settle
        yield return null;

        // Clear current selection
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;

        // Optionally reselect the button
        // EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    public void PlayGame()
    {
        AudioManager.instance.Play2DSound(menuNav, menuVol);

        if (menuMusic != null)
        {
            menuMusic.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                menuMusic.Stop(); // Optional, fully stop after fade
            });
        }



        FindObjectOfType<BarTransition>().CloseBars(() =>
        {
            StartCoroutine(Player());
        });
    }

    private IEnumerator Player()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

   

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}