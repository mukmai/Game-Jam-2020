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
    
    private bool stayingAtPosition;

    private Vector3 targetPosition;
    
    public int stopDistance = 2;
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        stayingAtPosition = false;
    }

    void Update()
    {
        if (!stayingAtPosition)
        {
            targetPosition = player.transform.position;
            agent.stoppingDistance = stopDistance;
        }
        else
        {
            agent.stoppingDistance = 0;
        }
        
        float distanceBtw = Vector3.Distance(transform.position, targetPosition);

        if (agent.stoppingDistance+0.6 >= distanceBtw)
        {
            agent.SetDestination(transform.position);
            _animator.SetBool("isWalk", false);
        }
        else
        {
            FaceTarget(targetPosition);
            agent.SetDestination(targetPosition);
            _animator.SetBool("isWalk", true);
        }
        
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    
    void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void stayAtPosition(Vector3 orderPosition)
    {
        targetPosition = orderPosition;
        
        stayingAtPosition = true;
    }

    public void freePosition()
    {
        stayingAtPosition = false;
    }
}
