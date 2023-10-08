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
    
    //for climbing
    [SerializeField] float climbingSpeed = 8f;
    //for attack
    [SerializeField] float attackRadius = 3f;
    //for SFX
    [SerializeField] AudioClip jumpSFX, attackingSFX, runningSFX, gettingHitSFX,
        deathSFX;
    
    [SerializeField] Vector2 hitKick = new Vector2(10f, 10f);
    [SerializeField] Transform hurtBox;

    // references
    Rigidbody2D myRigidBody2D;
    Animator myAnimator;
    BoxCollider2D myBoxCollider2D;
    PolygonCollider2D myPlayersFeet;
    AudioSource myAudioSource;
    
   

    float startingGravityScale;
    bool isHurting;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myPlayersFeet = GetComponent<PolygonCollider2D>();
        myAudioSource = GetComponent<AudioSource>();

        startingGravityScale = myRigidBody2D.gravityScale;

        myAnimator.SetTrigger("Door Out");
    }

    // Update is called once per frame
    void Update()
    {
        //if players gets hurt he cant do anything
        if (!isHurting)
        {
            Run();
            Jump();
            Climb();
            Attack();
            

            if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
            {
                PlayerHit();
            }

            
            ExitLevel();
        }
        
    }

    public void doorIn()
    {
        myAnimator.SetTrigger("Door In");
    }


    private void ExitLevel()
    {
        if (!myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Interactable"))) { return; }

        if (CrossPlatformInputManager.GetButtonUp("Vertical"))
        {
            myAnimator.SetTrigger("Door In");
            FindObjectOfType<ExitDoor>().StartLoadingNextLevel();
        }
    }

    // function to turn of sprite renderer
    public void TurnOffRenderer()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Attack()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            myAnimator.SetTrigger("Attacking");
            myAudioSource.PlayOneShot(attackingSFX);
            Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Enemy"));

            foreach(Collider2D enemy in enemiesToHit)
            {
                enemy.GetComponent<Enemy>().Dying();
            }
        }
    }

    public void PlayerHit()
    {
        // with this the enemy pushes the character back
        myRigidBody2D.velocity = hitKick * new Vector2(-transform.localScale.x, 1f);

        // set trigger for hitting
        myAnimator.SetTrigger("Hitting");
        isHurting = true;

        //SFX
        myAudioSource.PlayOneShot(gettingHitSFX);

        ////adding for processing lives
        FindObjectOfType<GameSession>().ProcessPlayerDeath();

        StartCoroutine(StopHurting());

    }

    public void PlayDeath()
    {
        myAnimator.SetTrigger("isDead");

        //SFX
        myAudioSource.PlayOneShot(deathSFX);

        //make the body static
        myRigidBody2D.bodyType = RigidbodyType2D.Static;
        GetComponent<BoxCollider2D>().enabled = false;

    }

    // we made a Coroutine that waits for two seconds and turns the method 'isHurting' false    
    IEnumerator StopHurting()
    {
        yield return new WaitForSeconds(1f);

        isHurting = false;
    }

    private void Climb()
    {
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 climbingVelocity = new Vector2(myRigidBody2D.velocity.x, controlThrow * climbingSpeed);

            myRigidBody2D.velocity = climbingVelocity;


            bool climbingStart = Mathf.Abs(myRigidBody2D.velocity.y) > Mathf.Epsilon;
            myAnimator.SetBool("Climbing", climbingStart);

            myRigidBody2D.gravityScale = 0f;

        }
        else
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody2D.gravityScale = startingGravityScale;
        }
        /*
            myAnimator.SetBool("Climbing", true);
        }
        else
            myAnimator.SetBool("Climbing", false);
        */
    }

    private void Jump()
    {
        // the '!' negates whatever is infront of it. if the condition is false it will execute it
        if (!myPlayersFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            // this return takes us out of the jump method
            return;
        }
        else if (myPlayersFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myAnimator.SetBool("isJumping", false);
        }

            bool isJumping = CrossPlatformInputManager.GetButtonDown("Jump");


        // adding y velocity for jumping
        if (isJumping) 
        {

            Vector2 jumpVelocity = new Vector2(myRigidBody2D.velocity.x, jumpSpeed);
            myRigidBody2D.velocity = jumpVelocity;

            //jump sound
            myAudioSource.PlayOneShot(jumpSFX);

            //animation
            myAnimator.SetBool("isJumping", true);
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

    //creating SFX for moving player
    void stepsSFX()
    {
        bool playerMovingHorizontally = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;

        if (playerMovingHorizontally)
        {
            if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                myAudioSource.PlayOneShot(runningSFX);
            }
        }
        else
        {
            myAudioSource.Stop();
        }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hurtBox.position, attackRadius);
    }

    

}
