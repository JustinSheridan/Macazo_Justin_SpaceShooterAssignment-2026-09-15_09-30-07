using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;
    [Space(30)]
    public float maxMoveSpeed;
    public float accelerationRate; // How quickly the enemy accelerates toward max speed
    private Vector3 velocity;
    
    private void Update()
    {
        moveToPlayerEnemy();
    }

    #region AI Movement
    void moveToPlayerEnemy()
    {
        if (playerTransform == null)
            return;

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        
        // Don't move if already on top of the player
        if (directionToPlayer.sqrMagnitude < 0.01f)
            return;

        directionToPlayer.Normalize(); // Aim towards player

        // Accelerate towards max speed in the direction of the player
        float accelerationDelta = accelerationRate * Time.deltaTime;
        velocity += directionToPlayer * accelerationDelta;

        // Cap velocity at maxMoveSpeed
        if (velocity.magnitude > maxMoveSpeed)
        {
            velocity = velocity.normalized * maxMoveSpeed;
        }

        transform.position += velocity * Time.deltaTime;
    }
    #endregion

}