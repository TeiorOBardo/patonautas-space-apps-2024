using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GrabHandler : MonoBehaviour
{
    public Transform playerCamera;
    bool leftArmFree = true;
    bool rightArmFree = true;
    public RaycastHit hitInfo;
    public float grabRange = 4f;
    private LayerMask unmovableObjectLayer;
    private HingeJoint leftJoint;
    private HingeJoint rightJoint;
    void Start()
    {
        unmovableObjectLayer = LayerMask.GetMask("Not Movable Grabable");
        leftJoint = new HingeJoint();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CanTryToGrab())
        {
            if (LookingAtGrabbable())
            {
                GrabOnObject();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = Quaternion.Euler(10,0,0) * transform.position;
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
        HingeJoint newJoint = gameObject.AddComponent<HingeJoint>();
        newJoint.anchor = hitInfo.point;
        if (Input.GetMouseButtonDown(0) && leftArmFree == true)
        {
            leftJoint = newJoint;
            leftArmFree = false;
        }
        else if (rightArmFree)
        {
            rightJoint = newJoint;
            rightArmFree = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(leftJoint != null)
        {
            Gizmos.DrawSphere(leftJoint.anchor, 0.05f);
        }
        if (rightJoint != null)
        {
            Gizmos.DrawSphere(rightJoint.anchor, 0.05f);
        }

    }
}
