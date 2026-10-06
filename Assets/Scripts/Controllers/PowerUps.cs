using UnityEngine;

public class PowerUps : MonoBehaviour
{
    // Player Listeners
    public Transform playerTransform; // Reference to the player's transform
    // Ints and Floats
    [Space(30)]
    // Public
    public float detectionRadius; // Radius in which the enemy will disappear
    // Private

    void Update()
    {
        CheckPlayerDistance();
    }

    void CheckPlayerDistance()
    {
        if (playerTransform != null)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= detectionRadius)
            {
                Destroy(gameObject);
            }
        }
    }
}