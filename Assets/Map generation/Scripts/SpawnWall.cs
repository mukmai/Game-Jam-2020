using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWall : MonoBehaviour
{
    public GameObject[] objects;
    
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, objects.Length);
        //Instantiate(objects[rand], transform.position, Quaternion.identity);
        
        Instantiate(objects[rand], transform.position + Vector3.down, Quaternion.Euler(-90,0,0));
    }
}
