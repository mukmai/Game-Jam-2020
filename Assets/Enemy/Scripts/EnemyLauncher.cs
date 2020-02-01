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

    private void OnEnable()
    {
        _gunHead = GetComponentsInChildren<Transform>()[1];
    }

    // Update is called once per frame
    void Update()
    {
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

    void Shoot()
    {
        // TODO: choose target
        var missile = Instantiate(missilePrefab, _gunHead.position, Quaternion.identity);
        //missile.transform.rotation = Quaternion.Euler(0,0,90.0f);
        missile.GetComponent<Rigidbody>().velocity = Vector3.up * missileVelocity;
        // TODO: set missile target
        missile.GetComponent<Missile>().target = target;
    }
}
