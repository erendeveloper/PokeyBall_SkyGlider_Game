using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//swipe controller for the player
//Sends ways to Player.cs
//Added on Player object
//Enabled after throwing
public class SwipePlayer : MonoBehaviour
{
    //Access other script
    PlayerController playerControllerScript;

    private float firstPosition;

    private float currenPositionRate;  //0 min, 1 max, swiping percentage


    private const float MinDistance = 0.01f;  //max swipe length
    private const float MaxDistance = 0.4f;  //max swipe length


    private void Awake()
    {
        playerControllerScript = this.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
            CheckSwipe();
    }

    private void CheckSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
             firstPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
             playerControllerScript.ToggleFallAndFly(); //Fly
        }
        if (Input.GetMouseButton(0))
        {
             SetCurrentPositionRate();
             playerControllerScript.TurnHorizontally(GetCurrentPositionRate());
        }
        if (Input.GetMouseButtonUp(0))
        {
             playerControllerScript.ToggleFallAndFly(); //Fall
        }

    }

    private void SetCurrentPositionRate() //0 min, 1 max, swiping percentage
    {
        float distance = CheckSwipeDistance();

        if (Mathf.Abs(distance) >= MaxDistance)
        {
            currenPositionRate = 1f;
            currenPositionRate *= Mathf.Sign(distance);
        }
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
        return Camera.main.ScreenToViewportPoint(Input.mousePosition).x -0.5f; //0.5 is the miidle of the screen
    }

}
