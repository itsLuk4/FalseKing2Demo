using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
// added standard assets addon for different inputs (i.e. controller, keyboard etc.)

public class Player : MonoBehaviour
{

    // its just like public where we change our inspector, but we manipulate the variable with a different script
    // in other words to change our variables i.e. you want to change players speed depending on the platform they are standing
    [SerializeField] float runSpeed = 10f;
    // now for jumping
    [SerializeField] float jumpSpeed = 15f;
    [SerializeField] float climbingSpeed = 8f;

    // references
    Rigidbody2D myRigidBody2D;
    Animator myAnimator;
    BoxCollider2D myBoxCollider2D;
    PolygonCollider2D myPlayersFeet;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myPlayersFeet = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Run();
        Jump();
        Climb();
        
    }

    private void Climb()
    {
        if (myPlayersFeet.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 climbingVelocity = new Vector2(myRigidBody2D.velocity.x, controlThrow * climbingSpeed);

            myRigidBody2D.velocity = climbingVelocity;

            bool climbingStart = Mathf.Abs(myRigidBody2D.velocity.y) > Mathf.Epsilon;
            myAnimator.SetBool("Climbing", climbingStart);
        }
    }

    private void Jump()
    {
        // the '!' negates whatever is infront of it. if the condition is false it will execute it
        if (!myPlayersFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            // this return takes us out of the jump method
            return;
        }

        bool isJumping = CrossPlatformInputManager.GetButtonDown("Jump");
        
        // adding y velocity for jumping
        if(isJumping) 
        {
            Vector2 jumpVelocity = new Vector2(myRigidBody2D.velocity.x, jumpSpeed);
            myRigidBody2D.velocity = jumpVelocity;
        }
    }



    private void Run()
    {
        // because its a 2D game we used Horizontal movement Axis from 'CrossPlatformInputManager'
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");

        // Vector2 is a 2D x/y vector where we made it into a new vector from our 'controlThrow' and our rigid body of 'y velocitiy'
        // which is horizontal movement
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody2D.velocity.y);
        myRigidBody2D.velocity = playerVelocity;

        // added it here, when our character runs in the left direction he flips
        flipSprite();

        // a method that changes the animation state from idling to running and vice versa
        runningState(); 
        
    }

    // I used mathf.Abs/mathf.Sign the different is what it returns
    private void flipSprite()
    {
        bool runningHorizontally = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;

        if(runningHorizontally) 
        { 
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody2D.velocity.x), 1f);
        }
    }

    private void runningState()
    {
        bool runningStart = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", runningStart);

    }
}
