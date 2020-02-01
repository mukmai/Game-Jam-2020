using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    public GameObject robot1;
    public GameObject robot2;
    public GameObject robot3;
    
    public GameObject crossHair1;
    public GameObject crossHair2;
    public GameObject crossHair3;
    
    private bool robot1Stay;
    private bool robot2Stay;
    private bool robot3Stay;
    
    void Awake ()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask ("Floor");
        
        anim = GetComponentInChildren <Animator> ();
        playerRigidbody = GetComponent <Rigidbody> ();
    }

    private void Start()
    {
        robot1Stay = false;
        robot2Stay = false;
        robot3Stay = false;
    }

    void Update()
    {
        Turning();
    }


    void FixedUpdate ()
    {
        float h = Input.GetAxisRaw ("Horizontal");
        float v = Input.GetAxisRaw ("Vertical");

        Move (h, v);

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
            robot1Stay = !robot1Stay;
            if (robot1Stay)
            {
                crossHair1.GetComponent<crossHairPos>().setPos(targetPosition);
                robot1.GetComponent<RobotController>().stayAtPosition(targetPosition);
            }
            else
            {
                crossHair1.GetComponent<crossHairPos>().cancelSetPos();
                robot1.GetComponent<RobotController>().freePosition();
            }
        }

        if (Input.GetKeyDown("2"))
        {
            robot2Stay = !robot2Stay;
            if (robot2Stay)
            {
                crossHair2.GetComponent<crossHairPos>().setPos(targetPosition);
                robot2.GetComponent<RobotController>().stayAtPosition(targetPosition);
            }
            else
            {
                crossHair2.GetComponent<crossHairPos>().cancelSetPos();
                robot2.GetComponent<RobotController>().freePosition();
            }
        }
        
        if (Input.GetKeyDown("3"))
        {
            robot3Stay = !robot3Stay;
            if (robot3Stay)
            {
                crossHair3.GetComponent<crossHairPos>().setPos(targetPosition);
                robot3.GetComponent<RobotController>().stayAtPosition(targetPosition);
            }
            else
            {
                crossHair3.GetComponent<crossHairPos>().cancelSetPos();
                robot3.GetComponent<RobotController>().freePosition();
            }
        }
    }
    
}
