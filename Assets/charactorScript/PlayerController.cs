using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public int shieldPrice = 20;
    
    private bool _shiftClicked = false;
    public Image healthBarPrefab;
    
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _shiftClicked = true;
        } else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _shiftClicked = false;
        }

        if (_shiftClicked)
        {
            RobotFollowOrder();
        }
        
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
                    if (!robot.isBroken && robot.GetComponent<Health>().currHealth < robot.GetComponent<Health>().maxHealth)
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
                
                anim.SetBool("isRepair",true);

                if (_currHealTime <= 0)
                {
                    // heal
                    healTarget.Heal(healAmount);
                    _stateManager.boltCount--;
                    _currHealTime = healTime;
                }

                _currHealTime -= Time.deltaTime;
            }
            else if (foundHealTarget && healTarget.currHealth == healTarget.maxHealth)
            {
                StopHealing();
                anim.SetBool("isRepair",false);
            }
            
        }
        else
        {
            StopHealing();
            anim.SetBool("isRepair",false);
        }
        
        healthBarPrefab.fillAmount = GetComponent<Health>().SendHp();
        
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

            if (!_shiftClicked)
            {
                RobotMoveOrder(floorHit.point);
            }

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

    void RobotMoveOrder(Vector3 targetPosition)
    {
        for (int i = 0; i <= 2; i++)
        {
            if (Input.GetMouseButtonDown(i))
            {
                robots[i].MoveOrder(targetPosition);
            }
        }
    }
    
    void RobotFollowOrder()
    {
        for (int i = 0; i <= 2; i++)
        {
            if (Input.GetMouseButtonDown(i))
            {
                robots[i].FollowOrder();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Lava"))
        {
            GetComponent<Health>().TakeDamage(100);
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("treasure"))
        {
            if (other.gameObject.name == "Shield1" && _stateManager.boltCount >= shieldPrice)
            {
                _stateManager.boltCount -= 10;
                GameObject obj = GameObject.Find("/robot1_gun/robot1/B/robotShield");
                obj.SetActive(true);
                Destroy(other.gameObject);
            }
            if (other.gameObject.name == "Shield2" && _stateManager.boltCount >= shieldPrice)
            {
                _stateManager.boltCount -= 10;
                GameObject obj = GameObject.Find("/robot2_gun/robot2/B/robotShield");
                obj.SetActive(true);
                Destroy(other.gameObject);
            }
            if (other.gameObject.name == "Shield3" && _stateManager.boltCount >= shieldPrice)
            {
                _stateManager.boltCount -= 10;
                GameObject obj = GameObject.Find("/robot3_gun/robot3/B/robotShield");
                obj.SetActive(true);
                Destroy(other.gameObject);
            }
        }
    }
}
