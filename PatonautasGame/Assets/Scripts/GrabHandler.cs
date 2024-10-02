using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GrabHandler : MonoBehaviour
{
    public Transform playerCamera;
    Rigidbody rigidBody;
    Vector3 anchor;
    Vector3 sideAnchor;
    int anchorArm;
    int sideAnchorArm;
    float anchorDistance = 0;
    Quaternion anchorRotation = Quaternion.identity;
    bool leftArmFree = true;
    bool rightArmFree = true;
    bool grabbing = false;
    bool needToSetup = false;
    bool stopSetup = false;
    RaycastHit hitInfo;
    LayerMask unmovableObjectLayer;
    float grabRange = 1.5f;
    float distanceFromRange = 0.5f;
    float verticalAngularSpeed = 20f;
    float angleFormed = 0f;
    Coroutine setupCoroutine;
    void Start()
    {
        unmovableObjectLayer = LayerMask.GetMask("Not Movable Grabable");
        rigidBody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CanTryToGrab())
        {
            if (LookingAtGrabbable())
            {
                print("hit the collider " + hitInfo.collider.name);
                GrabOnObject();
            }
        }
        if(grabbing)
        {
            HandleMovement();
        }
    }
    
    bool CanTryToGrab()
    {
        return (Input.GetMouseButtonDown(0) && leftArmFree) || (Input.GetMouseButtonDown(1) && rightArmFree);
    }
    
    bool LookingAtGrabbable()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        return Physics.Raycast(ray, out hitInfo, grabRange, unmovableObjectLayer);
    }

    void GrabOnObject()
    {
        anchor = hitInfo.point;
        anchorDistance = (rigidBody.position - anchor).magnitude;
        anchorRotation = rigidBody.rotation;
        if (Input.GetMouseButtonDown(0) && leftArmFree)
        {
            //adjust for if this is second grab
            leftArmFree = false;
        }
        else if (rightArmFree)
        {
            rightArmFree = false;
        }
        grabbing = true;
        needToSetup = true;
    }

    void HandleMovement()
    {
        float verticalDirection = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            verticalDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalDirection = -1f;
        }
        Vector3 positionDifference = rigidBody.position - anchor;
        Quaternion rotation = Quaternion.AngleAxis(verticalAngularSpeed * Time.deltaTime * verticalDirection, transform.right);
        Vector3 rotatedDifference = rotation * positionDifference;
        if (needToSetup)
        {
            setupCoroutine = StartCoroutine(SetupPositionAndRotation(Vector3.Angle(transform.forward, -1 * positionDifference), anchor - rigidBody.position));
            needToSetup = false;
            transform.LookAt(anchor);
            print("Started Setup");
        }else if(stopSetup)
        {
            StopCoroutine(setupCoroutine);
            print("Finished Setup");
            stopSetup = false;
        }
        rigidBody.MovePosition((rotatedDifference.normalized * anchorDistance) + anchor);
        angleFormed += verticalDirection * Vector3.Angle(positionDifference, rigidBody.position - anchor);
        rigidBody.MoveRotation(Quaternion.AngleAxis(angleFormed, transform.right) * anchorRotation);
        /*if(Mathf.Abs(anchorDistance - grabRange - distanceFromRange) > 0.2)
        {
            anchorDistance = Mathf.Lerp(anchorDistance, grabRange - distanceFromRange, 0.5f * Time.deltaTime);
        }*/
        
    }

    IEnumerator SetupPositionAndRotation(float angleToMove, Vector3 oldPositionDifference)
    {
        while (true)
        {
            anchorDistance = Mathf.Lerp(anchorDistance, grabRange - distanceFromRange, Time.deltaTime * 0.5f);
            //Quaternion targetRotation = Quaternion.AngleAxis(angleToMove, -1 * oldPositionDifference);
            //Quaternion.Lerp(anchorRotation, targetRotation, 0.5f * Time.deltaTime);
            //anchorRotation = Quaternion.Euler(Vector3.Lerp(anchorRotation.eulerAngles, oldPositionDifference.normalized, 0.5f * Time.deltaTime));
            print(oldPositionDifference.normalized + "OldPosDif");
            print(anchorRotation + " Rotation");
            print(Vector3.Angle(anchorRotation.eulerAngles, oldPositionDifference) + " Delta");
            if ((Mathf.Abs(anchorDistance - grabRange - distanceFromRange) < 0.05f) && (Mathf.Abs(Vector3.Angle(anchorRotation.eulerAngles, oldPositionDifference.normalized)) < 1f))
            {
                print("Ready to stop setup");
                stopSetup = true;
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(anchor != null)
        {
            Gizmos.DrawSphere(anchor, 0.05f);
        }
    }
}
