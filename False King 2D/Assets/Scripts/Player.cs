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

    Rigidbody2D myRigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Run();

        
    }

    private void Run()
    {
        // because its a 2D game we used Horizontal movement Axis from 'CrossPlatformInputManager'
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");

        // Vector2 is a 2D x/y vector where we made it into a new vector from our 'controlThrow' and our rigid body of 'y velocitiy'
        // which is horizontal movement
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody2D.velocity.y);
        myRigidBody2D.velocity = playerVelocity;

        flipSprite();
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
}