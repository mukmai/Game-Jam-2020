using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLauncher : MonoBehaviour
{
    public float shootMaxCooldown = 2.0f;
    private float _currCooldown = 1.0f;
    [SerializeField] private GameObject missilePrefab;
    public Transform _gunHead;
    [SerializeField] private Transform target;
    public float missileVelocity = 30.0f;
    private StateManager _stateManager;
    private bool _initialized = false;
    [SerializeField] private EnemyMovement body;

    private void OnEnable()
    {
        if (!_initialized)
        {
            Initialize();
        }
    }
    
    void Initialize()
    {
        _gunHead = GetComponentsInChildren<Transform>()[1];
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
            if (_currCooldown <= 0.0f)
            {
                if (Random.Range(0, 5) == 0)
                {
                    Shoot();
                    _currCooldown = shootMaxCooldown;
                }
            }

            _currCooldown -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        var missile = Instantiate(missilePrefab, _gunHead.position, Quaternion.identity);
        missile.GetComponent<Rigidbody>().velocity = Vector3.up * missileVelocity;
        missile.GetComponent<Missile>().target = target;
        target = _stateManager.EnemyGetTarget();
    }
    
    
}
