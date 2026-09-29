using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour
{
    
    public float maxFloatDistance;
    public float moveSpeed;
    public float arrivalDistance;
    private Vector3 targetPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomMovePosition();
    }

    // Update is called once per frame
    void Update()
    {
        MoveAsteroid();
    }
    
    #region Movement
    void MoveAsteroid()
    {
        Vector3 direction = targetPosition - transform.position; // Direction from current pos to target
        float distance = direction.magnitude;
        
        if (distance <= arrivalDistance) // If close enough to target, pick a new position
        {
            randomMovePosition();
        }
        else
        {
            transform.position += direction.normalized * moveSpeed * Time.deltaTime; // Move toward target
        }
    }
    
    void randomMovePosition()
    {
        float randomX = Random.Range(-maxFloatDistance, maxFloatDistance); // Random X within maxFloatDistance
        float randomY = Random.Range(-maxFloatDistance, maxFloatDistance); // Random Y within maxFloatDistance
        
        targetPosition = new Vector3(randomX, randomY, transform.position.z); // Set target to random position (keep current Z)
    }
    #endregion
}