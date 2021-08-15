using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player Controller
//Added on Player object
public class Player : MonoBehaviour
{
    private const float ThrowingSpeed = 100f;
    private const float HorizontalRotationSpeed = 10f;
    private const float ForwardRotationSpeed = 3f;
    private const float ThrowingAngle = 45f;
    private Vector3 forceVelocity;

    private Quaternion initialBodyQuaternion;

    public Transform body; //for rotating, child of player,  solid part of player

    public Animator playerAnimator; //added on rocketman

    private bool isFlying = false;
    private bool isRotating = false;

    private Rigidbody playerRigidbody;

    //Access otrher script
    SwipePlayer swipePlayerScript;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        swipePlayerScript = GetComponent<SwipePlayer>();
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
            body.Rotate(ForwardRotationSpeed, 0, 0, Space.Self);
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
        playerRigidbody.AddForce(forceVelocity);
        Fall();
        Camera.main.GetComponent<CameraFollow>().Follow();
        swipePlayerScript.enabled = true;
    }
    public void Fall(){
        toggleGravity();
        isFlying = false;
        isRotating = true;
        toggleAnimatorParameters();
    }
    public void Fly()
    {
        toggleGravity();
        isRotating = false;
        isFlying = true;
        toggleAnimatorParameters();
    }
    private void toggleGravity()//enabling gravity depending on rotating or flying
    {
        if (!playerRigidbody.useGravity) //notr fall
        {
            playerRigidbody.useGravity = true;
        }
        else //not fly
        {
            playerRigidbody.useGravity = false;
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
        }
    }
    private void toggleAnimatorParameters() //setting state paarameters depending on rotating or flying
    {
        if (isRotating)
        {
            playerAnimator.SetBool("Flying", false);
            playerAnimator.SetBool("Rotating", true);
        }
        else //flying
        {
            playerAnimator.SetBool("Rotating", false);
            playerAnimator.SetBool("Flying", true);
        }
    }

    public void TurnHorizontally(float rate)
    {
        transform.Rotate(Vector3.up*rate * Time.deltaTime * HorizontalRotationSpeed);
    }
    
}
