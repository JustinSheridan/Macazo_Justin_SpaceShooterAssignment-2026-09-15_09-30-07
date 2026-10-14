using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    // Prefab and boundary references
    public GameObject powerUpPrefab;
    public Vector2 topLeftClamp; // Top-left boundary of spawn area
    public Vector2 botRightClamp; // Bottom-right boundary of spawn area

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnPowerUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPowerUp() // Instantiates a power-up at a random location within the defined boundaries
    {
        // Calculate random position using the clamped boundaries
        float randomX = Random.Range(topLeftClamp.x, botRightClamp.x);
        float randomY = Random.Range(topLeftClamp.y, botRightClamp.y);
        
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
    }
}