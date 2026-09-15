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
    public InputActionReference _moveWASD; // WASD -> move in worldspace
    
    // Ints and Floats
    public int moveSpeed;

    
    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput =  NormalizeVector2(_moveWASD.action.ReadValue<Vector2>()); // Read the WASD keys vector2 value, and Normalize the output in the declaration
        Vector3 moveDirection = new Vector2(moveInput.x, moveInput.y); // Take vector2 input (WASD) and convert it to a vector3 
        transform.position += moveDirection * moveSpeed * Time.deltaTime; // Transform the player position in worldspace based on moveDirection * movespeed over time
        
        if (_bombKey.action.WasPressedThisFrame())
        {   
            Debug.Log("Bomb key pressed!");
            Vector3 playerPosition = transform.position;
            Vector3 bombSpawnPosition = playerPosition + new Vector3(0, 1, 0);
            Instantiate(bombPrefab, bombSpawnPosition, Quaternion.identity); // Quaternion.Identity keeps prefab rotation, consider using transform.position again
        }
        
    }
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
}

