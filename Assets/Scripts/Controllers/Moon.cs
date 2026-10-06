using UnityEngine;

public class Moon : MonoBehaviour
{
    // Other Components
    public Transform playerTransform;
    [Space(30)]
    // Publics
    public float orbitRadius;
    public float orbitSpeed;
    // Privates
    private float currentAngle = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoonOrbit();
    }

    #region Orbit
    void MoonOrbit()
    {
        if (playerTransform == null) // If no player assigned, don't orbit
        {
            return;
        }

        currentAngle += orbitSpeed * Time.deltaTime; // Rotate around the player by orbitSpeed per second
        float xPos = Mathf.Cos(currentAngle) * orbitRadius;
        float yPos = Mathf.Sin(currentAngle) * orbitRadius;

        Vector3 moonPosition = playerTransform.position + new Vector3(xPos, yPos, 0);
        transform.position = moonPosition;
    }
    #endregion
}