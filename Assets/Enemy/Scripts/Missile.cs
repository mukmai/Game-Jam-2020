using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Transform target;
    private bool goingDown = false;
    [SerializeField] private GameObject crossHairPrefab;
    private GameObject crossHair;
    public int damage = 30;

    // Update is called once per frame
    void Update()
    {
        if (!goingDown && transform.position.y > 20)
        {
            goingDown = true;
            var position = target.position;
            crossHair = Instantiate(crossHairPrefab, new Vector3(position.x, -0.5f, position.z) + Vector3.up * 0.1f, Quaternion.Euler(90,0,0));
            transform.position = new Vector3(position.x, transform.position.y, position.z);
            transform.rotation = Quaternion.Euler(180, 0, 0);
            GetComponent<Rigidbody>().velocity = Vector3.down * 17;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<Health>().TakeDamage(damage);
            // TODO: add explosion
            Destroy(crossHair);
            Destroy(gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            // TODO: add explosion
            Destroy(crossHair);
            Destroy(gameObject);
        }
    }
}
