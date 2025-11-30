using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyBinds : MonoBehaviour
{
    public bool selecting = false;
    public KeyCode selectedKey;
    public KeyIconManager iconManager; // Drag in from Inspector
    public Image keyIconImage;
    public Sprite defaultUnknownKeySprite;
    public void StartSelectKeyBind()
    {
        selecting = true;
    }

    private void Start()
    {
        UpdateIcon();
    }

    private void Update()
    {
        LockInputs.inputLocked = selecting;
        
        if (selecting)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                selecting = false;
                LockInputs.inputLocked = false;
                Debug.Log("Keybind selection canceled.");
                return;
            }



            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    selectedKey = key;
                    selecting = false;
                    UpdateIcon();
                    EventSystem.current.SetSelectedGameObject(null);
                    Debug.Log("Captured key: " + selectedKey);
                    break;
                }
                
            }
        }
       
    }

    void UpdateIcon()
    {
        Sprite icon = iconManager.GetIconForKey(selectedKey);
        if (icon != null)
        {
            keyIconImage.sprite = icon;
        }
        else
        {
            Debug.LogWarning("Icon not found for key: " + selectedKey);
            keyIconImage.sprite = defaultUnknownKeySprite;
            // Or leave blank, or show fallback UI
        }
    }
}
