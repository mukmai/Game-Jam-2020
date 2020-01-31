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

    void Awake ()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask ("Floor");
        
        anim = GetComponentInChildren <Animator> ();
        playerRigidbody = GetComponent <Rigidbody> ();
    }

    void Update()
    {
        crossHairFollow();
    }


    void FixedUpdate ()
    {
        float h = Input.GetAxisRaw ("Horizontal");
        float v = Input.GetAxisRaw ("Vertical");

        Move (h, v);

        Turning ();

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
            //crossHair.SetActive(true);
            //crossHair.transform.position = floorHit.point + Vector3.up * 0.1f;
            
            Vector3 playerToMouse = floorHit.point - transform.position;

            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation (playerToMouse);

            playerRigidbody.MoveRotation (newRotation);
        }
        //else
        //{
            //crossHair.SetActive(false);
        //}
    }

    void Animating (float h, float v)
    {
        bool walking = h != 0f || v != 0f;

        anim.SetBool ("isWalk", walking);
    }

    void crossHairFollow()
    {
        Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            crossHair.SetActive(true);
            crossHair.transform.position = floorHit.point + Vector3.up * 0.1f;
            crossHair.transform.rotation = Quaternion.Euler(90,0,0);
        }
        else
        {
            crossHair.SetActive(false);
        }
    }
}
