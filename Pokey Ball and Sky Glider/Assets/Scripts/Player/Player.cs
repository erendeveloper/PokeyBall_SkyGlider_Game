using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player Controller
//Added on Player object
public class Player : MonoBehaviour
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

    private Vector3 forceVelocity; //throwing velocity depending on stick tension
    private Quaternion initialBodyQuaternion;

    private bool isFlying = false;
    private bool isRotating = false;

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
        if (isRotating)
        {
            body.Rotate(ForwardRotationSpeed*Time.deltaTime, 0, 0, Space.Self);
        }
        else if (isFlying)
        {
            body.localRotation = Quaternion.Lerp(body.localRotation, initialBodyQuaternion, ForwardRotationSpeed * Time.deltaTime);
        }
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
        Camera.main.GetComponent<CameraFollow>().Follow();
        swipePlayerScript.enabled = true;
    }
    public void ToggleFallAndFly() //Switch between falling and flying
    {
        if (!isRotating)
        {
            //rotate
            playerRigidbody.useGravity = true;
            isFlying = false;
            isRotating = true;
            playerAnimator.SetBool("Flying", false);
            playerAnimator.SetBool("Rotating", true);
        }
        else
        {
            //fly
            playerRigidbody.useGravity = false;
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, Physics.gravity.y * GravityFactor, playerRigidbody.velocity.z);
            isRotating = false;
            isFlying = true;
            playerAnimator.SetBool("Rotating", false);
            playerAnimator.SetBool("Flying", true);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            playerRigidbody.velocity=new Vector3(playerRigidbody.velocity.x, forceVelocity.y * CubeBounceFactor, playerRigidbody.velocity.z);
        }
        else if (other.CompareTag("Cylinder"))
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, forceVelocity.y * CylinderBounceFactor, playerRigidbody.velocity.z);
        }
        else if (other.CompareTag("Ground"))
        {
            gameManagerScript.GameOver();
            swipePlayerScript.enabled = false;
            isRotating = false;
            isFlying = false;
            playerRigidbody.useGravity = false;
            playerRigidbody.velocity = Vector3.zero;
        }
    }

}
