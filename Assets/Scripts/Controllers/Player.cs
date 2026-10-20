using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;
    
    // Player Listeners
    public InputActionReference input_bombKey; // Click B -> Instantiate Bomb
    public InputActionReference input_moveWASD; // WASD -> move in worldspace
    
    // Ints and Floats
    public float moveSpeed;
    public float accelRate; // Controls acceleration/deceleration rate
    
    // Movement variables
    private Vector2 dirMoving = Vector2.zero; // Current input direction
    private Vector2 velocityMoving = Vector2.zero; // Current velocity
    private float currentMovementMultiplier = 1f; // Stores the multiplier for the HandleMovement function
    private float boostEndTime = 0f; // Time when the boost effect ends
    public float boostDuration = 5f; // Duration of the boost in seconds

    // Update is called once per frame
    void Update()
    {
        // Reset multiplier if boost has expired
        if (currentMovementMultiplier != 1f && Time.time >= boostEndTime)
        {
            currentMovementMultiplier = 1f;
        }

        // Pass the currentMultiplier (starts at 1, changes when power-up is picked)
        HandleMovement(currentMovementMultiplier);
        HandleRotation();
        
        if (input_bombKey.action.WasPressedThisFrame())
        {   
            Debug.Log("Bomb key pressed!");
            SpawnBomb();
        }
    }

    #region Movement
    public void BoostMovement(float newMultiplier) // Public method called by PowerUpManager to set speed
    {
        currentMovementMultiplier = newMultiplier;
        boostEndTime = Time.time + boostDuration;
    }


    public void HandleMovement(float movementMultiplier) // Handles player movement with acceleration/deceleration
    {
        // Read and normalize input
        dirMoving = NormalizeVector2(input_moveWASD.action.ReadValue<Vector2>());
        
        // Check if input is not 0
        if (dirMoving != Vector2.zero)
        {
            // Accelerate in dirMoving if dirMoving > 0, 0
            velocityMoving += dirMoving * accelRate * 2f * Time.deltaTime;
        }
        else
        {
            // De-accelerate if dirMoving == 0
            velocityMoving *= (1f - accelRate * Time.deltaTime);
            
            // Clamp small values to zero to prevent jitter
            if (velocityMoving.magnitude < 0.01f)
            {
                velocityMoving = Vector2.zero;
            }
        }
        
        // Apply velocity to transform using the provided multiplier
        Vector3 moveDirection = new Vector3(velocityMoving.x, velocityMoving.y, 0f);
        transform.position += moveDirection * movementMultiplier * moveSpeed * Time.deltaTime;
    }
    
    public void HandleRotation() // Rotates the player sprite in the direction of travel using sine and cosine
    {
        if (velocityMoving.magnitude > 0.01f)
        {
            float cosAngle = velocityMoving.x / velocityMoving.magnitude;
            float sinAngle = velocityMoving.y / velocityMoving.magnitude;
            float angle = Mathf.Atan2(sinAngle, cosAngle) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // Subtract 90° to point "up"
        }
    }
    #endregion
    
    #region Misc Methods
    public void SpawnBomb() // Instantiates a bomb and sets its properties based on the player's current state
    {
        Vector3 playerPosition = transform.position;
        
        // Get the direction the player is facing (in the 2D plane)
        // transform.up gives the "up" direction of the local space, which corresponds to the rotation applied in HandleRotation
        Vector3 forwardDirection = transform.up; 
        
        Vector3 bombSpawnPosition = playerPosition + (forwardDirection * 1f); // Spawns 1 unit in front
        
        GameObject bombInstance = Instantiate(bombPrefab, bombSpawnPosition, Quaternion.identity);
        
        BombPrefab bombScript = bombInstance.GetComponent<BombPrefab>();
        bombScript.moveDirection = new Vector3(velocityMoving.x, velocityMoving.y, 0f); // Set direction to current velocity direction
        bombScript.moveSpeed = moveSpeed * velocityMoving.magnitude; // Set speed to current speed
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
    #endregion
}