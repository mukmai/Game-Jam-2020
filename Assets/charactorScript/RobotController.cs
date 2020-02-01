using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RobotController : MonoBehaviour
{
    public NavMeshAgent agent;
    Animator _animator;

    public Transform player;
    
    public float lookRadius = 10f;

    private Vector3 targetPosition;

    public bool _robotMovingToTarget = false;
    [SerializeField] private crossHairPos _crossHair;
    private BBGun _gun;
    
    public int stopDistance = 2;

    public bool isBroken = false;
    public bool isHealing = false;

    public Text statusText;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _gun = GetComponentInChildren<BBGun>();
    }

    void Update()
    {
        
        if (!isBroken && !isHealing)
        {
            if (!_robotMovingToTarget)
            {
                statusText.text = "Follow";
                targetPosition = player.transform.position;
                agent.stoppingDistance = stopDistance;
            }
            else
            {
                statusText.text = "Stay";
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

        isHealing = false;
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

    public void Break()
    {
        statusText.text = "Dead";
        isBroken = true;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Collider>().enabled = false;
        _gun.enabled = false;
        _crossHair.cancelSetPos();
    }

    public void MoveOrder(Vector3 targetPos)
    {
        if (isBroken) return;
        _robotMovingToTarget = !_robotMovingToTarget;
        if (_robotMovingToTarget)
        {
            _crossHair.setPos(targetPos);
            targetPosition = targetPos;
        }
        else
        {
            _crossHair.cancelSetPos();
        }
    }
}
