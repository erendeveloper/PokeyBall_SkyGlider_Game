using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    private Animator stickAnimator;
    private SwipeStick swipeStickScript;

    private bool isStickPulling = false;
    private bool isStickReleasing = false;

    private bool isStickActive = true; //Swiping stick components are  active

    // Start is called before the first frame update
    private void Awake()
    {
        stickAnimator = this.GetComponent<Animator>();
        
        swipeStickScript = this.GetComponent<SwipeStick>();

    }
    void Start()
    {
        setStickSpeed(0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStickActive)
        {
            if (isStickPulling)
            {
                stickAnimator.Play("Base Layer.Armature|Bend_Stick", 0, swipeStickScript.getCurrentPositionRate());
            }
            else if (isStickReleasing)
            {
                if (stickAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    isStickActive = false;
                    Invoke("DisactivateStick", 1.0f);
                    //Call Throw ball here
                }
            }
        }
        

    }

    public void Pull()
    {
        setStickSpeed(0f);
        isStickPulling = true;
    }
    public void Reverse() //Rereversing stick on unenough swipe
    {
        isStickPulling = false;
        setStickSpeed(-1f);
    }
    public void Release()
    {
        isStickPulling = false;
        isStickReleasing = true;
        stickAnimator.SetTrigger("Release");
        setStickSpeed(1f);
    }
    public void setStickSpeed(float value) //Changing animator speed to stop,reverse and forward
    {
        stickAnimator.SetFloat("SpeedMultiplier", value);
    }

    private void DisactivateStick() //Disable swiping  stick components
    {
        swipeStickScript.enabled = false;
        stickAnimator.enabled = false;
        this.enabled = false;
    }
}
