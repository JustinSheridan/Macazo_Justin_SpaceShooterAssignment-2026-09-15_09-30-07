using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;
    
    // Player Listeners
    public InputActionReference _bombKey; // Click B -> Instantiate Bomb
    public InputActionReference _cornerBombKey; // Click B -> Instantiate Bomb
    public InputActionReference _moveWASD; // WASD -> move in worldspace
    public InputActionReference _warpKey;
    
    [Space(30)]
    // Ints and Floats
    public float moveSpeed;
    public int numberOfBombs;
    public float bombSpacing;
    public float inDistance;
    public float warpAmount;
    public float inMaxRange;

    
    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput =  NormalizeVector2(_moveWASD.action.ReadValue<Vector2>()); // Read the WASD keys vector2 value, and Normalize the output in the declaration
        Vector3 moveDirection = new Vector2(moveInput.x, moveInput.y); // Take vector2 input (WASD) and convert it to a vector3 
        transform.position += moveDirection * moveSpeed * Time.deltaTime; // Transform the player position in worldspace based on moveDirection * movespeed over time
        
        if (_bombKey.action.WasPressedThisFrame())
        {   
           spawnBomb();
        }
        if (_cornerBombKey.action.WasPressedThisFrame())
        {   
            spawnCornerBomb();
        }
        if (_warpKey.action.WasPressedThisFrame())
        {
            WarpToEnemy();
        }
        DrawLinesToAsteroids();

    }

    #region normalize
    public Vector2 NormalizeVector2(Vector2 input) // Normalizes a vector2
    {
        float x = input.x;
        float y = input.y;

        float magnitude = Mathf.Sqrt((x * x) + (y * y));

        if (magnitude == 0f)
        {
            return Vector2.zero;
        }

        float normalizedX = x / magnitude;
        float normalizedY = y / magnitude;

        return new Vector2(normalizedX, normalizedY);
    }
    #endregion

    void spawnBomb()
    {
        // Debug.Log("Bomb key pressed!");
        Vector3 playerPosition = transform.position;
        for (int i = 0; i < numberOfBombs; i++) 
        {
            Vector3 bombSpawnPosition = playerPosition + new Vector3(0, -1-bombSpacing*i, 0);
            Instantiate(bombPrefab, bombSpawnPosition, Quaternion.identity); // Quaternion.Identity keeps prefab rotation, consider using transform.position again
        }

    }

    void spawnCornerBomb()
    {
        
        Vector3 playerPosition = transform.position;
        if (_cornerBombKey.action.WasPressedThisFrame())
        {
            int corner = Random.Range(0, 4);
            Debug.Log("Bomb spawned in: " + corner);
            if (corner == 0)
            {
                Vector3 bombSpawnPosition = playerPosition + new Vector3(-inDistance, -inDistance, 0);
                Instantiate(bombPrefab, bombSpawnPosition,
                    Quaternion
                        .identity); // Quaternion.Identity keeps prefab rotation, consider using transform.position again
            }
            else if (corner == 1)
            {
                Vector3 bombSpawnPosition = playerPosition + new Vector3(-inDistance, inDistance, 0);
                Instantiate(bombPrefab, bombSpawnPosition,
                    Quaternion
                        .identity); // Quaternion.Identity keeps prefab rotation, consider using transform.position again
            }
            else if (corner == 2)
            {
                Vector3 bombSpawnPosition = playerPosition + new Vector3(inDistance, -inDistance, 0);
                Instantiate(bombPrefab, bombSpawnPosition,
                    Quaternion
                        .identity); // Quaternion.Identity keeps prefab rotation, consider using transform.position again
            }
            else
            {
                Vector3 bombSpawnPosition = playerPosition + new Vector3(inDistance, inDistance, 0);
                Instantiate(bombPrefab, bombSpawnPosition,
                    Quaternion
                        .identity); // Quaternion.Identity keeps prefab rotation, consider using transform.position again
            }
        }
 
    }
    void WarpToEnemy() 
    {
        if (enemyTransform == null) return;

        Vector3 playerPosition = transform.position;
        Vector3 targetPosition = enemyTransform.position;

        float t = Mathf.Clamp01(warpAmount); // The player is unable to go beyond 1 in the ratio

        Vector3 newPosition = Vector3.Lerp(playerPosition, targetPosition, t);
        transform.position = newPosition;
    }
    void DrawLinesToAsteroids()
    {
        Vector3 playerPosition = transform.position;
        
        foreach (Transform asteroid in asteroidTransforms) // Check all asteroids
        {
            if (asteroid == null) continue;
            
            float distance = Vector3.Distance(playerPosition, asteroid.position);
            
            if (distance <= inMaxRange) // If in range -> draw line in the direction of asteroid
            {
                Vector3 direction = (asteroid.position - playerPosition).normalized;
                Vector3 lineEnd = playerPosition + direction * 2.5f;
                
                Debug.DrawLine(playerPosition, lineEnd);
            }
        }
    }
}

