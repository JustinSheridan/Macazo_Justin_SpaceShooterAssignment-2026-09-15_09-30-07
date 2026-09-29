using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Other components
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;
    [Space(30)]
    // Player Listeners
    public InputActionReference bombKey; // Click B -> Instantiate Bomb
    public InputActionReference moveWASD; // WASD -> move in worldspace
    // Ints and Floats
    public float maxMoveSpeed;
    public float accelerationTime;
    
    private Vector3 velocity;

    void Start()
    {
        // 
        if (moveWASD.action != null)
        {
            moveWASD.action.Enable();
        }
        
        if (bombKey.action != null)
        {
            bombKey.action.Enable();
        }
    }
    void Update()
    {
        PlayerMovement();

       CheckBombKeyPressed();
    }

    #region Movement
    void PlayerMovement()
    {
        Vector2 moveInput = moveWASD.action.ReadValue<Vector2>(); // Take player input (e.g. WASD)
        
        if (moveInput == Vector2.zero) // If there is no input, decelerate
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime / accelerationTime); // Decelerate to a stop over accelerationTime
        }
        else
        {
            Vector3 targetVelocity = NormalizeVector2ToVector3(moveInput) * maxMoveSpeed; // Normalize the player inputs and mult by the max speed
            float accelerationPerSecond = maxMoveSpeed / accelerationTime; // a = delta v / delta t
            float accelerationDelta = accelerationPerSecond * Time.deltaTime; 
            
            Vector3 velocityDiff = targetVelocity - velocity;
            
            if (velocityDiff.magnitude <= accelerationDelta)
            {
                velocity = targetVelocity;
            }
            else
            {
                velocity += velocityDiff.normalized * accelerationDelta;
            }
        }

        transform.position += velocity * Time.deltaTime;
    }
    #endregion

    #region Bomb
    void CheckBombKeyPressed()
    {
        if (bombKey.action.WasPressedThisFrame())
        {
            Vector3 playerPosition = transform.position;
            Vector3 bombSpawnPosition = playerPosition + new Vector3(0, 1, 0);
            Instantiate(bombPrefab, bombSpawnPosition, Quaternion.identity);
        }
    }
    #endregion
    
    #region Misc
    public Vector3 NormalizeVector2ToVector3(Vector2 input) // Inputting a vector2 in this function spits out a normalized vector 3 with Z being zero
    {
        float x = input.x;
        float y = input.y;

        float magnitude = Mathf.Sqrt((x * x) + (y * y));

        if (magnitude == 0f)
        {
            return Vector3.zero;
        }

        float normalizedX = x / magnitude;
        float normalizedY = y / magnitude;

        return new Vector3(normalizedX, normalizedY, 0);
    }
    #endregion
}

