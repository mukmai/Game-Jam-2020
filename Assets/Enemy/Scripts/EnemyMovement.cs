using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    public float nextMoveTimer = 1.5f;
    private float _currTimer = 0.0f;
    public RoomManager room;

    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_currTimer <= 0.0f)
        {
            int rand = Random.Range(0, 6);
            if (rand == 0)
            {
                agent.SetDestination(transform.position);
            } else
            {
                agent.SetDestination(transform.position + new Vector3(Random.Range(-6, 6), 0, Random.Range(-6, 6)));
            }
            
            _currTimer = nextMoveTimer * Random.Range(0.7f, 1.0f);
        }

        _currTimer -= Time.deltaTime;
    }
}
