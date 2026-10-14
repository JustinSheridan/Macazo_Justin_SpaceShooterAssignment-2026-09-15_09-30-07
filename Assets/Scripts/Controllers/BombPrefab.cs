using UnityEngine;

public class BombPrefab : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 moveDirection; 
    public float totalLifetime;

    void Start()
    {
        moveDirection = NormalizeVector3(moveDirection);
        Destroy(gameObject, totalLifetime);

    }
    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    
    public Vector3 NormalizeVector3(Vector3 input) // Normalizes a vector3
    {
        float x = input.x;
        float y = input.y;
        // Z is not recorded because it will always be 0 in this program

        float magnitude = Mathf.Sqrt((x * x) + (y * y));

        if (magnitude == 0f)
        {
            return Vector3.zero;
        }

        float normalizedX = x / magnitude;
        float normalizedY = y / magnitude;

        return new Vector3(normalizedX, normalizedY, 0); 
    }
}
