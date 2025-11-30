using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandMoveTarget : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float directionChangeInterval = 2f;
    public float randomIntervalRange = 1f; // +/- range around directionChangeInterval
    public float moveRadius = 30f; // Optional max roam radius

    private Vector3 moveDirection;
    private Vector3 spawnPosition;
    private float nextChangeTime;

    public float wallCheckDistance = 5f;            // How far ahead to check for walls
    public LayerMask wallLayer;

    void Start()
    {
        spawnPosition = transform.position;
        PickNewDirection();
    }

    void Update()
    {
        // Move
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Keep grounded
        KeepOnGround();

        // Direction change timing
        if (Time.time >= nextChangeTime)
        {
            PickNewDirection();
        }

        // Pull back to center if out of bounds
        if ((transform.position - spawnPosition).magnitude > moveRadius)
        {
            moveDirection = (spawnPosition - transform.position).normalized;
            nextChangeTime = Time.time + directionChangeInterval;
        }
    }

    void KeepOnGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up * wallCheckDistance, Vector3.down); // cast down from above
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 10f, wallLayer))
        {
            Vector3 groundedPos = transform.position;
            groundedPos.y = hitInfo.point.y;
            transform.position = groundedPos;
        }
    }

    void PickNewDirection()
    {
        int attempts = 0;
        int maxAttempts = 1000;

        while (attempts < maxAttempts)
        {
            Vector3 randomDir = Random.onUnitSphere;
            randomDir.y = 0;
            randomDir.Normalize();

            // Check if the path is clear
            if (!Physics.Raycast(transform.position, randomDir, wallCheckDistance, wallLayer))
            {
                Debug.DrawRay(transform.position, randomDir);
                moveDirection = randomDir;
                nextChangeTime = Time.time + directionChangeInterval + Random.Range(-randomIntervalRange, randomIntervalRange);
                return;
            }

            attempts++;
        }

        // Fallback: stop if no clear direction found
        moveDirection = Vector3.zero;
        Debug.LogWarning("RandomMover: Could not find a valid direction after multiple attempts.");
    }
}
