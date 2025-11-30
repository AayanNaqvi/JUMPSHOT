using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Fire : MonoBehaviour
{
    public float dmgRate = 5f;
    private readonly HashSet<HealthAndDamage> playersInFire = new HashSet<HealthAndDamage>();






    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HealthAndDamage>(out var player))
        {
            playersInFire.Add(player);
            player.EnterMolotovFire();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HealthAndDamage>(out var player))
        {
            playersInFire.Remove(player);
            player.ExitMolotovFire();
        }
    }

    private void OnDestroy()
    {
        foreach (var player in playersInFire)
        {
            if (player != null)
            {
                player.ExitMolotovFire();
            }
        }
    }







}
