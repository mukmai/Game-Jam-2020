using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class Bolt : MonoBehaviour
{
    public Transform target;
    private bool beginSuck = false;
    public StateManager stateManager;
    
    Vector3 _velocity = Vector3.zero;

    private void OnEnable()
    {
        Physics.IgnoreLayerCollision(12, 8);
        Physics.IgnoreLayerCollision(12, 9);
        stateManager = GameObject.Find("/StateManager").GetComponent<StateManager>();
        target = stateManager.player.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "player")
        {
            stateManager.boltCount++;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (beginSuck)
        {
            transform.position =
                Vector3.SmoothDamp(transform.position, target.position, ref _velocity, Time.deltaTime * 5);
        }
        else
        {
            if (Vector3.Distance(transform.position, target.position) < 4)
            {
                beginSuck = true;
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Collider>().isTrigger = true;
                Physics.IgnoreLayerCollision(12, 8, false);
            }
        }
    }
}
