using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public float shootMaxCooldown = 2.0f;
    private float _currCooldown = 1.0f;
    [SerializeField] private GameObject bulletPrefab;
    private Transform _gunHead;
    [SerializeField] private Transform target;
    public float bulletVelocity = 12.0f;

    private void OnEnable()
    {
        _gunHead = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        if (_currCooldown <= 0.0f)
        {
            if (Random.Range(0, 15) == 0)
            {
                Shoot();
                _currCooldown = shootMaxCooldown;
            }
        }

        _currCooldown -= Time.deltaTime;
    }

    void Shoot()
    {
        // TODO: choose target
        var bullet = Instantiate(bulletPrefab, _gunHead.position, transform.rotation);
        Vector3 dir = target.position - bullet.transform.position;
        dir -= new Vector3(0, dir.y, 0);
        bullet.GetComponent<Rigidbody>().velocity = dir.normalized * bulletVelocity;
    }
}
