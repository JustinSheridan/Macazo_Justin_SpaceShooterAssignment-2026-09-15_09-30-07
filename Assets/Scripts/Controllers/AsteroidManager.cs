using UnityEngine;
using System.Collections.Generic;

public class AsteroidManager : MonoBehaviour
{
    // Prefab and boundary references
    public GameObject asteroidPrefab;
    public Vector2 topLeftClamp; // Top-left boundary of spawn area
    public Vector2 botRightClamp; // Bottom-right boundary of spawn area

    // Player and collision settings
    public Transform playerTransform; // Reference to the player's Transform
    public float triggerRadius = 1f; // Radius around the asteroid for detection
    public int maxAsteroids = 20;    // Maximum number of asteroids allowed

    // Array to track active asteroid instances
    private List<GameObject> asteroidArray = new List<GameObject>();

    void Start()
    {
        // Spawn all asteroids on start
        for (int i = 0; i < maxAsteroids; i++)
        {
            SpawnAsteroid();
        }
    }

    void Update()
    {
        CheckPlayerCollisions();
    }

    public void SpawnAsteroid() // Instantiates an asteroid at a random location within the defined boundaries
    {
        // Spawn based on our public clamps
        float randomX = Random.Range(topLeftClamp.x, botRightClamp.x);
        float randomY = Random.Range(topLeftClamp.y, botRightClamp.y);

        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
        GameObject newAsteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        asteroidArray.Add(newAsteroid); // add new prefab to array
    }

    public void CheckPlayerCollisions() // Checks if the player transform is within the trigger radius of any asteroid
    {
        // Retrieve the player using the Tag a
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        // Safety check
        if (playerObj == null) return;

        Transform playerTrans = playerObj.transform;

        for (int i = asteroidArray.Count - 1; i >= 0; i--)
        {
            // Check if the object still exists (in case it was destroyed by another means)
            if (asteroidArray[i] != null)
            {
                float distance = Vector3.Distance(playerTrans.position, asteroidArray[i].transform.position);

                if (distance < triggerRadius)
                {
                    // Call the new method on the Player to set the speed multiplier to -1
                    Player playerScript = playerObj.GetComponent<Player>();
                    if (playerScript != null)
                    {
                        playerScript.BoostMovement(-1);
                    }

                    Destroy(asteroidArray[i]); // Destroy the asteroid
                    asteroidArray.RemoveAt(i); // Remove it from tracking list
                }
            }
            else
            {
                // Clean up any null references in the list
                asteroidArray.RemoveAt(i);
            }
        }
    }
}