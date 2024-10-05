using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class QuaternionRotationTest : MonoBehaviour
{
    // Start is called before the first frame update
    RaycastHit hitInfo;
    LayerMask unmovableObjectLayer;
    float grabRange = 4f;
    public Transform playerCamera;
    Vector3 anchor;
    int anchorArm;
    Vector3 sideAnchor;
    int sideAnchorArm;
    float verticalAngularSpeed = 20f;
    void Start()
    {
        unmovableObjectLayer = LayerMask.GetMask("Not Movable Grabable");
    }

    // Update is called once per frame
    void Update()
    {
        float verticalDirection = 0f;
        if(LookingAtGrabbable())
        {
            anchor = hitInfo.point;
        }
        if(Input.GetKey(KeyCode.W))
        {
            verticalDirection = 1f;
        }else if(Input.GetKey(KeyCode.S))
        {
            verticalDirection = -1f;
        }
        Vector3 positionDifference = transform.position - anchor;
        Quaternion rotation = Quaternion.AngleAxis(verticalAngularSpeed * Time.deltaTime * verticalDirection, transform.right);
        Vector3 rotatedDifference = rotation * positionDifference;
        transform.position = rotatedDifference + anchor;
        float angleFormed = Vector3.Angle(positionDifference, transform.position - anchor);
        transform.rotation *= Quaternion.AngleAxis(angleFormed, transform.right);
    }

    bool LookingAtGrabbable()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        return Physics.Raycast(ray, out hitInfo, grabRange, unmovableObjectLayer);
    }
}
