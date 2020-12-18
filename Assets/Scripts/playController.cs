using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playController : MonoBehaviour
{
    //player movement
    Rigidbody2D rb2d;
    [SerializeField]
    float moveSpeed = 0;
    [SerializeField]
    float jumpForce = 0;
    float moveInput;


    //Renderer
    [SerializeField]
    SpriteRenderer playerRender;

    //check if player is grounded
    private bool isGrounded;
    public Transform feetPosition;
    public Transform footPositionLeft;
    public Transform footPositionRight;
    public float checkRadius;
    public LayerMask whatIsGround;

    //jump higher mechanic
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    //Wall SlideDown
    bool isTouchingFrontRight;
    public Transform frontCheckRight;
    bool isTouchingFrontLeft;
    public Transform frontCheckLeft;
    bool wallSliding;
    public float wallSlideSpeed;

    //Wall Jump
    bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;


    //Hang time/Coyote hang time
    public float hangTime = .2f;
    private float hangCounter;

    //particle effects
    public ParticleSystem slideParticles;
    private ParticleSystem.EmissionModule slideEmission;

    public ParticleSystem impactParticles;
    private bool wasOnGround;





    // Start is called before the first frame update
    void Start()
    {
        //add the component to the script
        rb2d = GetComponent<Rigidbody2D>();
        playerRender = GetComponent<SpriteRenderer>();

        //particle setup to change emission amount in the inspector
        slideEmission = slideParticles.emission;
    }







    // Update is called once per frame
    void Update()
    {
        //jump (i need to put this in update (not fixedupdate) because of physics reasons)
        if(Physics2D.Linecast(transform.position, feetPosition.position, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Linecast(transform.position, footPositionLeft.position, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Linecast(transform.position, footPositionRight.position, 1 << LayerMask.NameToLayer("Ground")))
                {
                     isGrounded = true;
                }
        else
        {
            isGrounded = false;
        }

        if (hangCounter > 0f && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb2d.velocity = Vector2.up * jumpForce;
        }

        //Wall Slide mechanic
        isTouchingFrontRight = Physics2D.OverlapCircle(frontCheckRight.position, checkRadius, whatIsGround);
        isTouchingFrontLeft = Physics2D.OverlapCircle(frontCheckLeft.position, checkRadius, whatIsGround);

        if(isTouchingFrontLeft == true && isGrounded == false && moveInput != 0 || isTouchingFrontRight == true && isGrounded == false && moveInput != 0) 
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if(wallSliding == true)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -wallSlideSpeed, float.MaxValue));
        }

        //Wall Jump mechanic
        if(Input.GetKeyDown(KeyCode.Space) && wallSliding == true)
        {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if(wallJumping == true)
        {
            rb2d.velocity = new Vector2(xWallForce * -moveInput, yWallForce);
        }



        //Set hangTime to 0.2f if Grounded. If not in the air, then you have 0.2f to jump (because i'm nice)
        if(isGrounded)
        {
            hangCounter = hangTime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb2d.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        //partlcle effect effect/emissions
        if(isGrounded)
        {
            slideEmission.rateOverDistance = 15f;
        }
        else
        {
            slideEmission.rateOverDistance = 0f;
        }

        //show landing/impact effects

        if(!wasOnGround && isGrounded)
        {
            screenShakeController.instance.StartShake(.1f, .3f);

            impactParticles.gameObject.SetActive(true);
            impactParticles.Stop();
            impactParticles.transform.position = slideParticles.transform.position;
            impactParticles.Play();
        }
        wasOnGround = isGrounded;
    }


    //WallJump set to false
    void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }


        //use this for physical updates
        private void FixedUpdate()
        {
            //move left & right
            moveInput = Input.GetAxis("Horizontal");
            rb2d.velocity = new Vector2(moveInput * moveSpeed, rb2d.velocity.y);

            //flip spriterenderer when moving l or r
            if (Input.GetKey("a"))
            {
                playerRender.flipX = true;
            }
            if (Input.GetKey("d"))
            {
                playerRender.flipX = false;
            }
        }
    }
