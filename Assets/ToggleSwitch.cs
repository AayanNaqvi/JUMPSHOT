using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Diagnostics.CodeAnalysis;
using System;

public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
{
    [Header("Slider Setup")]
    [SerializeField, Range(0, 1f)] protected float sliderValue;
    public bool currentValue { get; private set; }

    private Slider slider;

    [Header("Animation")]
    [SerializeField, Range(0, 1f)] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve slideEase = AnimationCurve.EaseInOut(0,0,1,1);

    private Coroutine animationSliderCoroutine;

    [Header("Events")]
    [SerializeField] private UnityEvent onToggleOn;
    [SerializeField] private UnityEvent onToggleOff;

    private ToggleSwitchGroupManager toggleSwitchGroupManager;

    protected Action transitionEffect;

    protected void OnValidate()
    {
        SetupToggleComponents();
        
        slider.value = sliderValue;
    }

    private void SetupToggleComponents()
    {
        if(slider != null)
        {
            return;
        }

        SetupSliderComponent();
    }

    private void SetupSliderComponent()
    {
        slider = GetComponent<Slider>();


        slider.interactable = false;
        var sliderColors = slider.colors;
        sliderColors.disabledColor = Color.white;
        slider.colors = sliderColors;

        slider.transition = Selectable.Transition.None;
    }

    public void SetupForManager(ToggleSwitchGroupManager tsgm)
    {
        toggleSwitchGroupManager = tsgm;
    }

    protected virtual void Awake()
    {
        SetupToggleComponents();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    public void Toggle()
    {
        if(toggleSwitchGroupManager != null)
        {
            toggleSwitchGroupManager.ToggleGroup(this);
        }
        else
        {
            SetStateAndStartAnimation(!currentValue);
        }

    }

    public void ToggleByGroupManager(bool valueToSetTo)
    {
        SetStateAndStartAnimation(valueToSetTo);
    }

    private void SetStateAndStartAnimation(bool state)
    {
        currentValue = state;

        if (currentValue)
            onToggleOn?.Invoke();
        else
            onToggleOff?.Invoke();

        if(animationSliderCoroutine != null)
            StopCoroutine(animationSliderCoroutine);

        animationSliderCoroutine = StartCoroutine(AnimateSlider());
    }

    private IEnumerator AnimateSlider()
    {
        float startValue = slider.value;
        float endValue = currentValue ? 1 : 0;

        float time = 0;
        if(animationDuration > 0 )
        {
            while(time < animationDuration)
            {
                time += Time.deltaTime;

                float lerpFactor = slideEase.Evaluate(time / animationDuration);
                slider.value = sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);

                transitionEffect?.Invoke();

                yield return null;
            }
        }

        slider.value = endValue;

    }

}
