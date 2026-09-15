using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public InputActionReference _bombKey; // Left mouse click -> Spawn Square
    public List<Transform> asteroidTransforms;
    
    // Update is called once per frame
    void Update()
    {
        if (_bombKey.action.WasPressedThisFrame())
        {   
            Debug.Log("Bomb key pressed!");
            Vector3 spawnPosition = transform.position + new Vector3(0, 1, 0);
            Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
