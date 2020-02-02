using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate (0, 60 * Time.deltaTime,0);
        transform.position = startPos + new Vector3(0, Mathf.Sin(Time.time) * 0.3f, 0);
    }
}
