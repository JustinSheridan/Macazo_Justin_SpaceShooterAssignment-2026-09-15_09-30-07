using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour
{
    // Prefab and boundary references
    public GameObject powerUpPrefab;
    public Vector2 topLeftClamp; // Top-left boundary of spawn area
    public Vector2 botRightClamp; // Bottom-right boundary of spawn area
    
    // Player and collision settings
    public Transform playerTransform; // Reference to the player's Transform
    public float triggerRadius = 1f; // Radius around the power-up for detection
    public int maxPowerUps = 20;     // Maximum number of power-ups allowed
    public float speedMultiplier = 2f; // The speed multiplier applied to the player upon pickup

    // Array to track active power-up instances
    private List<GameObject> prefabArray = new List<GameObject>();
    private float spawnInterval = 10f; // Time between spawns
    private float lastSpawnTime;

    void Start()
    {
        lastSpawnTime = Time.time; 
    }

    void Update()
    {
        CheckPlayerCollisions(); // Check if the player is near any power-up
        // Spawn a new power-up every 10 seconds if below the maximum limit
        if (Time.time - lastSpawnTime >= spawnInterval && prefabArray.Count < maxPowerUps)
        {
            SpawnPowerUp();
            lastSpawnTime = Time.time; // Reset the spawn timer
        }
    }

    public void SpawnPowerUp() // Instantiates a power-up at a random location within the defined boundaries
    {
        // Spawn based on our public clamps
        float randomX = Random.Range(topLeftClamp.x, botRightClamp.x);
        float randomY = Random.Range(topLeftClamp.y, botRightClamp.y);
        
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
        GameObject newPowerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
        prefabArray.Add(newPowerUp); // add new prefab to array
    }

    public void CheckPlayerCollisions() // Checks if the player transform is within the trigger radius of any power-up
    {
        // Retrieve the player using the Tag as requested
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        
        // Safety check to ensure the player exists and is not null
        if (playerObj == null) return; 

        Transform playerTrans = playerObj.transform;

        for (int i = prefabArray.Count - 1; i >= 0; i--)
        {
            if (prefabArray[i] != null)
            {
                float distance = Vector3.Distance(playerTrans.position, prefabArray[i].transform.position);
                
                if (distance < triggerRadius)
                {
                    Player playerScript = playerObj.GetComponent<Player>();
                    if (playerScript != null)
                    {
                        // Remove the (int) cast
                        playerScript.BoostMovement(speedMultiplier); 
                    }

                    Destroy(prefabArray[i]);
                    prefabArray.RemoveAt(i);
                }
            }

            else
            {
                // Clean up any null references in the list
                prefabArray.RemoveAt(i); 
            }
        }
    }

}