using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SwipeStick.cs gives ways to this script to control the stick
//Added on Stick_long_Animated

public class StickController : MonoBehaviour
{
    private Animator stickAnimator;
    
    //Other scripts
    private SwipeStick swipeStickScript;
    private PlayerController playerControllerScript;

    private bool isStickPulling = false;

    private int bendStateHash;//calling state for performance 

    // Start is called before the first frame update
    private void Awake()
    {
        stickAnimator = this.GetComponent<Animator>();
        
        swipeStickScript = this.GetComponent<SwipeStick>();
        playerControllerScript = GameObject.FindGameObjectWithTag("Rocketman").GetComponent<PlayerController>();

    }
    void Start()
    {
        SetStickSpeed(0f);
        bendStateHash = Animator.StringToHash("Base Layer.Armature|Bend_Stick");
    }

    // Update is called once per frame
    void Update()
    {
         if (isStickPulling)
         {
             stickAnimator.Play(bendStateHash, 0, swipeStickScript.GetCurrentPositionRate());
         }

    }

    #region Stick states
    public void Pull()
    {
        SetStickSpeed(0f);
        isStickPulling = true;
    }
    public void Reverse() //Rereversing stick on unenough swipe
    {
        isStickPulling = false;
        SetStickSpeed(-1f);
    }
    public void Release()
    {
        isStickPulling = false;
        stickAnimator.SetTrigger("Release");
        SetStickSpeed(1f);
        playerControllerScript.AssignPlayerVelocity(swipeStickScript.GetCurrentPositionRate());
    }
    public void ReleaseFinished()
    {
        DisableStickComponents(); 
    }
    public void SetStickSpeed(float value) //Changing animator speed to stop,reverse and forward
    {
        stickAnimator.SetFloat("SpeedMultiplier", value);
    }
    #endregion
    public void DisableStickComponents()//components that won't be used again
    {
        swipeStickScript.enabled = false;
        this.GetComponent<Animator>().enabled = false;
        this.enabled = false;
    }
}
