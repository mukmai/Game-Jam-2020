using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public NavMeshAgent agent;
    Animator _animator;

    public Transform player;
    
    public float lookRadius = 10f;
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceBtw = Vector3.Distance(transform.position, player.transform.position);

        if (agent.stoppingDistance >= distanceBtw)
        {
            agent.SetDestination(transform.position);
            _animator.SetBool("isWalk",false);
        }
        else
        {
            FaceTarget();
            agent.SetDestination(player.transform.position);
            _animator.SetBool("isWalk",true);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    
    void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
