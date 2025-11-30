
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitchColorChange : ToggleSwitch
{
    [Header("Elements to Recolor")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image handleImage;

    [Space]
    [SerializeField] private bool recolorBackground;
    [SerializeField] private bool recolorHandle;

    [Header("Color")]
    [SerializeField] private Color backgroundColorOff = Color.white;
    [SerializeField] private Color backgroundColorOn = Color.white;
    [Space]
    [SerializeField] private Color handleColorOff = Color.white;
    [SerializeField] private Color handleColorOn = Color.white;

    private bool isBackgroundImageNotNull;
    private bool isHandleImageNotNull;

    private new void OnValidate()
    {
        base.OnValidate();

        CheckForNull();
        ChangeColors();
    }

    private void OnEnable()
    {
        transitionEffect += ChangeColors;
    }

    private void OnDisable()
    {
        transitionEffect -= ChangeColors;
    }

    protected override void Awake()
    {
        base.Awake();

        CheckForNull();
        ChangeColors();

    }

    private void CheckForNull()
    {
        isBackgroundImageNotNull = backgroundImage != null;
        isHandleImageNotNull = handleImage != null;
    }


    private void ChangeColors()
    {
        if(recolorBackground && isBackgroundImageNotNull)
            backgroundImage.color = Color.Lerp(backgroundColorOff, backgroundColorOn, sliderValue);

        if(recolorHandle && isHandleImageNotNull)
            handleImage.color = Color.Lerp(handleColorOff, handleColorOn, sliderValue);
    }


}
