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
    private StateManager _stateManager;
    private bool _initialized = false;

    private void OnEnable()
    {
        if (!_initialized)
        {
            Initialize();
        }
    }

    void Initialize()
    {
        _gunHead = GetComponentInChildren<Transform>();
        _stateManager = GameObject.Find("/StateManager").GetComponent<StateManager>();
        target = _stateManager.EnemyGetTarget();
        _initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_initialized)
        {
            Initialize();
        }
        
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
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
        var bullet = Instantiate(bulletPrefab, _gunHead.position, transform.rotation);
        Vector3 dir = target.position - bullet.transform.position;
        dir -= new Vector3(0, dir.y, 0);
        bullet.GetComponent<Rigidbody>().velocity = dir.normalized * bulletVelocity;
        target = _stateManager.EnemyGetTarget();
    }
}
