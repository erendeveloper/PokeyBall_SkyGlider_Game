using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Swipe controller for the stick
//Sends ways to StickController.cs
//Added on Stick object

public class SwipeStick : MonoBehaviour
{
   //Access other script
    StickController stickcontrollerScript;

    private float firstPosition; 

    private float currenPositionRate;  //0 min, 1 max, swiping percentage

    private bool isSwipable = true;

    //Threshold
    private const float MinDistance = 0.2f; //if less, stick gets reverse back
    private const float MaxDistance = 0.4f;  //max swipe length


    private void Awake()
    {
        stickcontrollerScript = this.GetComponent<StickController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSwipable)
        {
            CheckSwipe();
        }
           
    }

    private void CheckSwipe()
    {
            if (Input.GetMouseButtonDown(0))
            {
                firstPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
                currenPositionRate = 0;
                stickcontrollerScript.Pull();
            }
            else if (Input.GetMouseButton(0))
            {
                SetCurrentPositionRate();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                SetCurrentPositionRate();
                VerifySwipe();
            }

    }

    private void SetCurrentPositionRate() //0 min, 1 max, swiping percentage
    {
        float distance = CheckSwipeDistance();
        if (distance >= MaxDistance)
            currenPositionRate = 1f;
        else if (distance <= 0)
            currenPositionRate = 0f;
        else
        {
            currenPositionRate = distance / MaxDistance;
        }
    }
    public float GetCurrentPositionRate()
    {
        return currenPositionRate;
    }

    public float CheckSwipeDistance() //Calculates swipe length
    {
        float lastPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
        return firstPosition - lastPosition;
    }
    private void VerifySwipe() //decides the stick to reverse or release depending on swipe disatance
    {
        if (CheckSwipeDistance() >= MinDistance)
        {
            isSwipable = false;
            stickcontrollerScript.Release();          
        }
        else
        {
            stickcontrollerScript.Reverse();
        }
    }

}
