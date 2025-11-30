using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyIconPair
{
    public KeyCode key;
    public Sprite icon;
}

public class KeyIconManager : MonoBehaviour
{
    public List<KeyIconPair> keyIconPairs;
    private Dictionary<KeyCode, Sprite> keyIconDict;

    void Awake()
    {
        keyIconDict = new Dictionary<KeyCode, Sprite>();
        foreach (var pair in keyIconPairs)
        {
            keyIconDict[pair.key] = pair.icon;
        }
    }

    public Sprite GetIconForKey(KeyCode key)
    {
        if (keyIconDict.TryGetValue(key, out Sprite sprite))
            return sprite;

        return null; // Or a default icon
    }
}