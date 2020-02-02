using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpin : MonoBehaviour
{
    public float shootMaxCooldown = 2.0f;
    private float _currCooldown = 1.0f;
    [SerializeField] private GameObject bulletPrefab;
    public float bulletVelocity = 12.0f;
    private StateManager _stateManager;
    private bool _initialized = false;
    [SerializeField] private EnemyMovement body;
    private int usingSkill = 0;
    private float startTime;

    private void OnEnable()
    {
        if (!_initialized)
        {
            Initialize();
        }
    }

    void Initialize()
    {
        _stateManager = GameObject.Find("/StateManager").GetComponent<StateManager>();
        _initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!body.isSleeping)
        {
            if (!_initialized)
            {
                Initialize();
            }
            
            if (usingSkill == 0)
            {
                if (_currCooldown <= 0.0f)
                {
                    usingSkill = 1;
                    body.isUsingSkill = true;
                    startTime = Time.time;
                    _currCooldown = shootMaxCooldown;
                    Spin();
                }

                _currCooldown -= Time.deltaTime;
            }
            if (usingSkill == 1)
            {
                Spin();
            }
        }
    }

    public float nextSpinCooldown = 0.8f;
    private float _currNextSpinCooldown = 1.0f;
    private int dirCount = 6;
    
    void Spin()
    {
        transform.Rotate (0, 600 * Time.deltaTime,0);
        if (Time.time - startTime >= 3.0f)
        {
            usingSkill = 0;
            body.isUsingSkill = false;
            _currNextSpinCooldown = 1.0f;
            return;
        }
        
        if (_currNextSpinCooldown <= 0.0f)
        {
            for (int i = 0; i < dirCount; i++)
            {
                var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                Vector3 dir = Quaternion.Euler(0, 360.0f / dirCount * i, 0) * Vector3.forward;
                bullet.GetComponent<Rigidbody>().velocity = dir.normalized * bulletVelocity;
            }

            _currNextSpinCooldown = nextSpinCooldown;
        }

        _currNextSpinCooldown -= Time.deltaTime;
    }
}
