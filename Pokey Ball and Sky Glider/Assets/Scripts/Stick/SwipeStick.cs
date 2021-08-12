using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeStick : MonoBehaviour
{
   //swipe controller for the stick


    StickController stickcontrollerScript;

    private float firstPosition;

    private float currenPositionRate;  //0 min, 1 max, swiping percentage

    //Threshold
    private const float minDistance = 0.25f; //if less, stick gets reverse back
    private const float maxDistance = 0.5f;  //max swipe length


    private void Awake()
    {
        stickcontrollerScript = this.GetComponent<StickController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkSwipe();
    }

    private void checkSwipe()
    {
            if (Input.GetMouseButtonDown(0))
            {
                firstPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
                setCurrentPositionRate();
                stickcontrollerScript.Pull();
            }
            else if (Input.GetMouseButton(0))
            {
                setCurrentPositionRate();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                setCurrentPositionRate();
                verifySwipe();
            }

    }

    private void setCurrentPositionRate() //0 min, 1 max, swiping percentage
    {
        float distance = CheckSwipeDistance();
        if (distance >= maxDistance)
            currenPositionRate = 1f;
        else if (distance <= 0)
            currenPositionRate = 0f;
        else
        {
            currenPositionRate = distance / maxDistance;
        }
    }
    public float getCurrentPositionRate()
    {
        return currenPositionRate;
    }

    public float CheckSwipeDistance() //Calculates swipe length
    {
        float lastPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
        return firstPosition - lastPosition;
    }
    private void verifySwipe() //decides the stick to reverse or release depending on swipe disatance
    {
        if (CheckSwipeDistance() >= minDistance)
        {
            stickcontrollerScript.Release();
        }
        else
        {
            stickcontrollerScript.Reverse();
        }
    }

}
