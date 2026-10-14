using System;
using UnityEngine;
using System.Collections;

public class PowerUpPrefab : MonoBehaviour
{
    public float bounceDistance;
    public float bounceTime;
    public float totalLifetime;
    private int runBounce;

    private void Start()
    {
        Destroy(gameObject, totalLifetime);
    }

    void Update()
    {
        if (runBounce == 0)
        {
            StartBounce();
        }
    }

    public void StartBounce()
    {
        runBounce = 1; // Mark runBounce as active to prevent redundant method calls.
        StartCoroutine(BounceCoroutine());
    }

    private IEnumerator BounceCoroutine()
    {
        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;

        // Move up for bounceTime
        while (elapsedTime < bounceTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / bounceTime);
            // Use sine for smooth motion
            float height = Mathf.Sin(progress * Mathf.PI * 0.5f) * bounceDistance;
            transform.position = originalPosition + Vector3.up * height;
            yield return null;
        }

        // Move down
        elapsedTime = 0f;
        while (elapsedTime < bounceTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / bounceTime);
            // Use sine
            float height = Mathf.Cos(progress * Mathf.PI * 0.5f) * bounceDistance;
            transform.position = originalPosition + Vector3.up * height;
            yield return null;
        }

        // Snap to starting pos
        transform.position = originalPosition;
        runBounce = 0; // Unmark runBounce to start again
    }
}