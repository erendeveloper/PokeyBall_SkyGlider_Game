using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Camera follows player smoothly
//Added on Main Camera

public class CameraFollow : MonoBehaviour
{
   
    public Transform targetCameraPosition; //camera gets child of this and then gets zero local position

    public Transform lookAtPosition; //camera looks at this position
    public Transform targetLookAtPosition; //lookAtPosition gets child of this and then gets zero local position

    private float followSpeed=0.5f;

    private bool isFollowing = false;

    private bool isCameraPositionFit = false; //camera fits to target as child
    private bool isLookAtPositionFit = false; //lookAtPosition fits to target as child

    // Start is called before the first frame update
    void Start()
    {        
        transform.LookAt(targetLookAtPosition); //to avoid instant lookAt, game starts with looking
    }


    void FixedUpdate()
    {
        if (isFollowing)
        {
            Follow();
        }

    }

    public void StartFollowing()
    {
        this.transform.parent = targetCameraPosition;
        targetLookAtPosition.parent = targetLookAtPosition;
        isFollowing = true;
    }
    private void Follow()
    {
        transform.LookAt(targetLookAtPosition);

        if (!isCameraPositionFit)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, Time.deltaTime * followSpeed);
            if (Vector3.Distance(transform.localPosition, Vector3.zero) < 0.001f)
            {
                transform.localPosition = Vector3.zero;
                isCameraPositionFit = true;
            }
        }
        if (!isLookAtPositionFit)
        {
            lookAtPosition.localPosition = Vector3.MoveTowards(lookAtPosition.localPosition, Vector3.zero, Time.deltaTime * followSpeed);
            if (Vector3.Distance(lookAtPosition.localPosition, Vector3.zero) < 0.001f)
            {
                lookAtPosition.localPosition = Vector3.zero;
                isLookAtPositionFit = true;
            }
        }
        if (isCameraPositionFit && isLookAtPositionFit)
        {
            this.enabled = false; //they fitted the local target positions and no more need to follow
        }
    }

}
