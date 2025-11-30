using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawning : MonoBehaviour
{

    [Header("Spawn Area Bounds")]
    public float minX;
    public float maxX;
    public float minY, maxY;
    public float minZ, maxZ;

    [Header("Spawning Settings")]
    public GameObject[] targetPrefabs;
    public int maxTargets = 3;
    public float respawnDelay = 2f;
    public LayerMask collisionCheckMask;
    public float collisionRadius = 1f;

    private List<GameObject> activeTargets = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < maxTargets; i++)
        {
            TrySpawnTarget();
        }
    }

    private void TrySpawnTarget()
    {
        Vector3 spawnPos = GetValidSpawnPosition();
        if (spawnPos != Vector3.zero)
        {
            GameObject prefab = targetPrefabs[UnityEngine.Random.Range(0, targetPrefabs.Length)];
            GameObject newTarget = Instantiate(prefab, spawnPos, Quaternion.identity);

            var targetScript = newTarget.GetComponent<EnemHealth>();
            if (targetScript != null)
            {
                targetScript.spawner = this; // Pass reference to this spawner
            }

            activeTargets.Add(newTarget);
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        const int maxAttempts = 20;
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 pos = new Vector3(
                UnityEngine.Random.Range(minX, maxX),
                UnityEngine.Random.Range(minY, maxY),
                UnityEngine.Random.Range(minZ, maxZ)
            );

            bool blocked = Physics.CheckSphere(pos, collisionRadius, collisionCheckMask);
            if (!blocked)
                return pos;
        }

        return Vector3.zero; // Failed to find valid spawn
    }

    public void NotifyTargetDeath(GameObject deadTarget)
    {
        activeTargets.Remove(deadTarget);
        Invoke(nameof(RespawnTarget), respawnDelay);
    }

    private void RespawnTarget()
    {
        Debug.Log("Respawning targets");
        if (activeTargets.Count < maxTargets)
        {
            TrySpawnTarget();
        }
    }
}
