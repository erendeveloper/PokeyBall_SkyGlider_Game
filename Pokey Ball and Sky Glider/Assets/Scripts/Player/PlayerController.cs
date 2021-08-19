using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player Controller
//Added on Player object
public class PlayerController : MonoBehaviour
{
    //Access other script
    SwipePlayer swipePlayerScript;
    GameManager gameManagerScript;

    public Animator playerAnimator; //added on rocketman
    public Transform body; //for rotating, child of player,  solid part of player
    private Rigidbody playerRigidbody;

    private const float ThrowingSpeed = 15f;
    private const float HorizontalRotationSpeed = 50f;
    private const float ForwardRotationSpeed = 1000f;
    private const float ThrowingAngle = 45f;
    private const float BodyAngle = 90f;//looks downwarn while flying, initial player Vector3(180,180,180)
    private const float MaxWingAngle = 45f;

    private Vector3 forceVelocity; //throwing velocity depending on stick tension
    private Quaternion initialBodyQuaternion;

    private bool isFlying = false;
    private bool isFalling = false;
    private bool isWingAngleIdentity = true; //Quaternion.eular(0,0,0)

    //bounce factors on platforms
    private const float CubeBounceFactor = 1f;
    private const float CylinderBounceFactor = 2f;

    private const float GravityFactor=0.2f;//Factor of falling speed, applied on gravity

    

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        swipePlayerScript = GetComponent<SwipePlayer>();
        gameManagerScript = Camera.main.GetComponent<GameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        initialBodyQuaternion = body.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        RotateBody();
    }

    public void AssignVelocity(float rate) //for throwing
    {
        float radiantAngle = ThrowingAngle * Mathf.Deg2Rad;
        float z= Mathf.Cos(radiantAngle) * ThrowingSpeed*rate;
        float y= Mathf.Sin(radiantAngle) * ThrowingSpeed*rate;

        forceVelocity = new Vector3(0, y, z);

        Invoke("ThrowPlayer", 0.2f); //Fixing throwing at the top of release animation
    }
    private void ThrowPlayer()
    {
        this.transform.parent = null;
        transform.rotation = Quaternion.identity;
        playerRigidbody.velocity = forceVelocity;
        ToggleFallAndFly();
        playerAnimator.SetBool("Rotating", true);
        Camera.main.GetComponent<CameraFollow>().StartFollowing();
        swipePlayerScript.enabled = true;
    }
    public void ToggleFallAndFly() //Switch between falling and flying
    {
        if (!isFalling)
        {
            //fall
            playerRigidbody.useGravity = true;
            isFlying = false;
            isFalling = true;
            playerAnimator.SetBool("Flying", false);
            playerAnimator.SetBool("Rotating", true);
        }
        else
        {
            //fly
            playerRigidbody.useGravity = false;
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, Physics.gravity.y * GravityFactor, playerRigidbody.velocity.z);
            isFalling = false;
            isFlying = true;
            playerAnimator.SetBool("Rotating", false);
            playerAnimator.SetBool("Flying", true);
            isWingAngleIdentity = false; //wings have horizontal angle while turning
        }
    }


    public void TurnHorizontally(float rate)
    {
        transform.Rotate(Vector3.up*rate * Time.deltaTime * HorizontalRotationSpeed);

        //save forward speed on rotate way
        //z component is the first applied force, movement speed
        Vector3 normalizedVelocity = transform.forward;// 
        float x = normalizedVelocity.x * forceVelocity.z;
        float z = normalizedVelocity.z * forceVelocity.z;

        playerRigidbody.velocity= new Vector3(x, playerRigidbody.velocity.y, z);

    }

    private void OnTriggerEnter(Collider other)//Top of platform for bouncing
    {
        if (other.CompareTag("CubeTopSurface"))
        {
            RotateOnPlatform();
            playerRigidbody.velocity=new Vector3(playerRigidbody.velocity.x, forceVelocity.y * CubeBounceFactor, playerRigidbody.velocity.z);
        }
        else if (other.CompareTag("CylinderTopSurface"))
        {
            RotateOnPlatform();
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, forceVelocity.y * CylinderBounceFactor, playerRigidbody.velocity.z);
        }
    }
    private void OnCollisionEnter(Collision other)//Sides of platform for falling
    {
        playerAnimator.SetBool("Flying", false);
        playerRigidbody.useGravity = true;
        swipePlayerScript.enabled = false;
        isFalling = false;
        isFlying = false;

        if (other.gameObject.CompareTag("PlatformSideSurface"))
        {
            other.transform.parent.GetChild(0).gameObject.SetActive(false);     
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            gameManagerScript.GameOver();
            playerRigidbody.velocity = Vector3.zero;
        }
        
    }
    private void RotateOnPlatform() { //if player stands on a platform while flying, it rotates
        swipePlayerScript.IsSwiping = false;
        if (isFlying)
        {
            ToggleFallAndFly();
        }
    }

    private void RotateBody()//rotate forward and rotate wings
    {
        if (isFalling)
        {
            if (!isWingAngleIdentity)//make wings identity
            {
                body.localRotation = Quaternion.Lerp(body.localRotation, Quaternion.identity, ForwardRotationSpeed * Time.deltaTime);
                float angleThreshold = Quaternion.Angle(body.localRotation, Quaternion.identity);
                if (angleThreshold < 0.001f)
                {
                    isWingAngleIdentity = true;
                }
            }
            else //rotate forward
            {
                body.Rotate(ForwardRotationSpeed * Time.deltaTime, 0, 0, Space.Self);
            }
        }
        else if (isFlying) //rotate wings
        {
            float wingAngle = swipePlayerScript.GetCurrentPositionRate() * MaxWingAngle;
            Vector3 targetRotation = new Vector3(BodyAngle + wingAngle, BodyAngle, BodyAngle);
            body.localRotation = Quaternion.Lerp(body.localRotation, Quaternion.Euler(targetRotation), ForwardRotationSpeed * Time.deltaTime);
        }     
    }

}
