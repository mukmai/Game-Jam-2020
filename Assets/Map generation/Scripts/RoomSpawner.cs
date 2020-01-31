using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 --> need bottom door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door

    private RoomTemplates _templates;
    private int rand;
    private bool spawned = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    // Update is called once per frame
    void Spawn()
    {
        if (!spawned)
        {
            if (openingDirection == 1)
            {
                rand = Random.Range(0, _templates.bottomRooms.Length);
                Instantiate(_templates.bottomRooms[rand], transform.position, Quaternion.identity);
            } else if (openingDirection == 2)
            {
                rand = Random.Range(0, _templates.topRooms.Length);
                Instantiate(_templates.topRooms[rand], transform.position, Quaternion.identity);
            } else if (openingDirection == 3)
            {
                rand = Random.Range(0, _templates.leftRooms.Length);
                Instantiate(_templates.leftRooms[rand], transform.position, Quaternion.identity);
            } else if (openingDirection == 4)
            {
                rand = Random.Range(0, _templates.rightRooms.Length);
                Instantiate(_templates.rightRooms[rand], transform.position, Quaternion.identity);
            }

            spawned = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            Debug.Log("A");
            if (!other.GetComponent<RoomSpawner>().spawned && !spawned)
            {
                
            }
            Destroy(gameObject);
        }
    }
}
