using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;            

    Vector3 movement;           
    Animator anim;                
    Rigidbody playerRigidbody;          
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.

    public GameObject crossHair;
    
    public RobotController[] robots;

    private bool healing = false;
    public float healRange = 2;
    Vector3 _healMoveVelocity = Vector3.zero;
    private bool foundHealTarget = false;
    private Health healTarget;
    public int healAmount = 10;
    public float healTime = 0.5f;
    private float _currHealTime = 0.5f;
    
    private StateManager _stateManager;
    
    void Awake ()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask ("Floor");
        
        anim = GetComponentInChildren <Animator> ();
        playerRigidbody = GetComponent <Rigidbody> ();
        _stateManager = GameObject.Find("/StateManager").GetComponent<StateManager>();
    }

    void Update()
    {
        Turning();
        if (Input.GetKeyDown("space"))
        {
            foundHealTarget = false;
        }
        if (Input.GetKey("space") && _stateManager.boltCount > 0)
        {
            // find robot to heal first
            if (!foundHealTarget)
            {
                RobotController closestRobot = robots[0];
                float closestDist = 20000;
                foreach (var robot in robots)
                {
                    if (robot.GetComponent<Health>().currHealth > 0)
                    {
                        float dist = Vector3.Distance(robot.transform.position, transform.position);
                        if (closestDist > dist)
                        {
                            closestDist = dist;
                            closestRobot = robot;
                        }
                    }
                }
                if (closestDist <= healRange)
                {
                    foundHealTarget = true;
                    healTarget = closestRobot.GetComponent<Health>();
                    // tell robot to stop
                    closestRobot.isHealing = true;
                }
            }
            
            if (foundHealTarget && healTarget.currHealth < healTarget.maxHealth)
            {
                // Start heal
                healing = true;
                Vector3 robotPos = healTarget.transform.position;
                Vector3 healPos = robotPos + (transform.position - robotPos).normalized * 0.7f;
                if (Vector3.Distance(healPos, transform.position) > 0.05f)
                {
                    transform.position =
                        Vector3.SmoothDamp(transform.position, healPos, ref _healMoveVelocity, Time.deltaTime * 10);
                }
                
                transform.LookAt(new Vector3(robotPos.x, transform.position.y, robotPos.z));

                // TODO: add repairing animation

                if (_currHealTime <= 0)
                {
                    // heal
                    healTarget.Heal(healAmount);
                    _stateManager.boltCount--;
                    _currHealTime = healTime;
                }

                _currHealTime -= Time.deltaTime;
            }
        }
        else
        {
            StopHealing();
        }
    }

    void StopHealing()
    {
        foundHealTarget = false;
        healing = false;
        foreach (var robot in robots)
        {
            robot.isHealing = false;
        }
        _currHealTime = healTime;
    }

    void FixedUpdate ()
    {
        float h = 0;
        float v = 0;
        if (!healing)
        {
            h = Input.GetAxisRaw ("Horizontal");
            v = Input.GetAxisRaw ("Vertical");

            Move (h, v);
        }
        
        Animating (h, v);
    }

    void Move (float h, float v)
    {
        movement.Set (h, 0f, v);

        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition (transform.position + movement);
    }

    void Turning ()
    {
        Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

        RaycastHit floorHit;

        if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
        {
            crossHair.SetActive(true);
            crossHair.transform.position = floorHit.point + Vector3.up * 0.1f;
            crossHair.transform.rotation = Quaternion.Euler(90,0,0);

            robotOrder(floorHit.point);
            
            Vector3 playerToMouse = floorHit.point - transform.position;

            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation (playerToMouse);

            playerRigidbody.MoveRotation (newRotation);
        }
        else
        {
            crossHair.SetActive(false);
        }
    }

    void Animating (float h, float v)
    {
        bool walking = h != 0f || v != 0f;

        anim.SetBool ("isWalk", walking);
    }

    void robotOrder(Vector3 targetPosition)
    {
        if (Input.GetKeyDown("1"))
        {
            robots[0].MoveOrder(targetPosition);
        }

        if (Input.GetKeyDown("2"))
        {
            robots[1].MoveOrder(targetPosition);
        }
        
        if (Input.GetKeyDown("3"))
        {
            robots[2].MoveOrder(targetPosition);
        }
    }
    
}
