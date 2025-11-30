using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarReveal : MonoBehaviour
{
  
    

    [SerializeField] private float revealDuration;
    private Outline outlineComponent;
    private Coroutine revealRoutine;

    void Start()
    {
        outlineComponent = GetComponent<Outline>();
        if (outlineComponent != null)
            outlineComponent.enabled = false;
    }

    public void Reveal()
    {
        if (outlineComponent == null)
            return;

        if (revealRoutine != null)
            StopCoroutine(revealRoutine);

        revealRoutine = StartCoroutine(RevealRoutine());
    }

    private IEnumerator RevealRoutine()
    {
        outlineComponent.enabled = true;
        yield return new WaitForSeconds(revealDuration);
        outlineComponent.enabled = false;
    }

    
}
