using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public GameObject[] topBoxes;

    public float skillCooldown = 2.0f;
    private float _currCooldown = 1.0f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform target;
    public float bulletVelocity = 12.0f;
    private StateManager _stateManager;
    private bool _initialized = false;
    [SerializeField] private EnemyMovement body;
    private int usingSkill = 0;
    private float startTime;
    
    [SerializeField] private GameObject missilePrefab;
    public float missileVelocity = 17.0f;

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
        target = _stateManager.EnemyGetTarget();
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
                    usingSkill = Random.Range(1, 3);
                    body.isUsingSkill = true;
                    startTime = Time.time;
                    _currCooldown = skillCooldown;
                }

                _currCooldown -= Time.deltaTime;
            }
            if (usingSkill == 1)
            {
                Spin();
            }
            else if (usingSkill == 2)
            {
                Rain();
            }
        }
    }

    public float nextSpinCooldown = 0.8f;
    private float _currNextSpinCooldown = 1.0f;
    private int dirCount = 16;
    
    void Spin()
    {
        transform.Rotate (0, 600 * Time.deltaTime,0);
        if (Time.time - startTime >= 5.0f)
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

    public float nextRainCooldown = 0.2f;
    private float _currNextRainCooldown = 0.0f;
    public int curBox = 0;
    
    void Rain()
    {
        if (Time.time - startTime >= 2.3f)
        {
            usingSkill = 0;
            body.isUsingSkill = false;
            _currNextRainCooldown = 0.0f;
            curBox = 0;
            return;
        }
        
        if (_currNextRainCooldown <= 0.0f)
        {
            var missile = Instantiate(missilePrefab, topBoxes[curBox].transform.position, Quaternion.identity);
            missile.GetComponent<Rigidbody>().velocity = Vector3.up * missileVelocity;
            missile.GetComponent<Missile>().target = target;
            target = _stateManager.EnemyGetTarget();
            _currNextRainCooldown = nextRainCooldown;
            curBox = (curBox + 1) % 4;
        }

        _currNextRainCooldown -= Time.deltaTime;
    }
}
