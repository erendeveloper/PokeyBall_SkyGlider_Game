using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Camera follows player smoothly
//Added on Main Camera

public class CameraFollow : MonoBehaviour
{
   
    public Transform targetCameraPosition; //Gets child of target camera position and then gets zero local position
    public Transform targetLookAtPosition; //Camera looks at target look position

    private float followSpeed=0.5f;

    private bool isPositionFit = false; //position fits to target as child
    private bool isRotationFit = false; //rotation fits to target
    private bool isFollowing = false;

    // Start is called before the first frame update
    void Start()
    {        
        transform.LookAt(targetLookAtPosition); //to avoid instant lookAt, game starts with looking
    }


    void FixedUpdate()
    {
        if (isFollowing)
        {
            if (!isPositionFit)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, Time.deltaTime * followSpeed);
                if (Vector3.Distance(transform.localPosition, Vector3.zero) < 0.001f)
                {
                    isPositionFit = true;
                }
            }
            if (!isRotationFit)
            {
                transform.LookAt(targetLookAtPosition);
                float angleThreshold = Quaternion.Angle(transform.localRotation, Quaternion.identity);
                if (angleThreshold < 0.001f)
                {
                    isRotationFit = true;
                }
            }
            else if (isPositionFit)
            {
                this.enabled = false; //they fitted as child and no more needed to follow
            }
        }
        
    }

    public void Follow()
    {
        this.transform.parent = targetCameraPosition;
        isFollowing = true;
    }
    
}
