using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BBGun : MonoBehaviour
{
    public float shootMaxCooldown = 0.5f;
    private float _currCooldown = 0.5f;
    [SerializeField] private GameObject bulletPrefab;
    private Transform _gunHead;
    public float bulletVelocity = 12.0f;
    
    public bool foundEnemy = false;
    private Vector3 closestEnemy = new Vector3(1000, 1000, 1000);
    
    private void OnEnable()
    {
        _gunHead = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (foundEnemy)
        {
            transform.LookAt(new Vector3(closestEnemy.x, transform.position.y, closestEnemy.z));
            if (_currCooldown <= 0.0f)
            {
                if (Random.Range(0, 15) == 0)
                {
                    Shoot();
                    _currCooldown = shootMaxCooldown;
                }
            }

            foundEnemy = false;
        }
        else
        {
            //transform.rotation = Quaternion.identity;
        }
        _currCooldown -= Time.deltaTime;
    }
    
    void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, _gunHead.position, transform.rotation);
        Vector3 dir = closestEnemy - bullet.transform.position;
        dir -= new Vector3(0, dir.y, 0);
        bullet.GetComponent<Rigidbody>().velocity = dir.normalized * bulletVelocity;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        foundEnemy = true;
        Debug.Log("B");
        if (Vector3.Distance(closestEnemy, transform.position) >
            Vector3.Distance(other.transform.position, transform.position))
        {
            closestEnemy = other.transform.position;
        }
    }
}
