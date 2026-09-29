using UnityEngine;
using UnityEngine.InputSystem;

public class InClassLineDrawScript : MonoBehaviour
{
    private int currentAngleIndex = 0;
    public float[] angles;
    public InputActionReference spaceKey;

    public float cycleDuration = 1f;
    private float elapsedTime = 0f;

    void Start()
    {
        angles = new float[10];
        for (int i = 0; i < 10; i++)
        {
            angles[i] = Random.Range(0f, 360f);
        }

        if (spaceKey.action != null)
        {
            spaceKey.action.Enable();
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= cycleDuration)
        {
            elapsedTime %= cycleDuration;
            currentAngleIndex = (currentAngleIndex + 1) % angles.Length;
        }
    }

    void OnDrawGizmos()
    {
        if (angles == null || angles.Length == 0) return;

        float angle = angles[currentAngleIndex] * Mathf.Deg2Rad;
        Vector3 end = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 1f;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, end);
    }
}