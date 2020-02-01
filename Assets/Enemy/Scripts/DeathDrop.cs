using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeathDrop : MonoBehaviour
{
    [SerializeField] private GameObject boltPrefab;
    public int minDrop = 3;
    public int maxDrop = 5;
    
    private void OnDestroy()
    {
        // TODO: explode
        for (int i = 0; i < Random.Range(minDrop, maxDrop + 1); i++)
        {
            Rigidbody currBolt = Instantiate(boltPrefab, transform.position, transform.rotation * Quaternion.Euler(30, Random.Range(0, 360), 0)).GetComponent<Rigidbody>();
            currBolt.velocity = Random.Range(0.5f, 1.0f) * currBolt.transform.forward;
        }
    }
}
