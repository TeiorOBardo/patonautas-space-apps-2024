using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrabHandler : MonoBehaviour
{
    public Transform playerCamera;
    Rigidbody rigidBody;
    struct Anchor {
        public int arm;
        public Vector3 position;
        public float distance;
        public Quaternion rotation;
        public bool nulled;
        public Vector3 right;
    }
    Anchor[] anchors = new Anchor[2];
    Vector3 contactNormal;
    Collider collisionCollider;
    bool leftArmFree = true;
    bool rightArmFree = true;
    bool grabbing = false;
    bool needToSetup = false;
    bool stopSetup = false;
    bool stopFly = false;
    bool flying = false;
    RaycastHit hitInfo;
    LayerMask unmovableObjectLayer;
    float grabRange = 1.5f;
    float distanceFromRange = 0.5f;
    float verticalAngularSpeed = 20f;
    float angleFormed = 0f;
    Coroutine setupCoroutine;
    Coroutine flyCoroutine;
    void Start()
    {
        unmovableObjectLayer = LayerMask.GetMask("Not Movable Grabable");
        rigidBody = GetComponent<Rigidbody>();
        ResetAnchor(ref anchors[0]);
        ResetAnchor(ref anchors[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if (!flying)
        {
            if (CanTryToGrab())
            {
                if (LookingAtGrabbable())
                {
                    print("hit the collider " + hitInfo.collider.name);
                    GrabOnObject();
                }
            }
            else if (CanUngrab())
            {
                print("Ungrabbing " + leftArmFree + "  = left arm " + rightArmFree + " = right arm");
                Ungrab();
            }
        }
        if(CanFly())
        {
            if (!anchors[0].nulled)
            {
                Ungrab();
            }
            if (!anchors[1].nulled)
            {
                Ungrab();
            }
            flyCoroutine = StartCoroutine(SetupFly());
        }
        if (stopFly)
        {
            StopCoroutine(flyCoroutine);
            flying = false;
        }
    }

    private void FixedUpdate()
    {
        if(grabbing)
        {
            HandleMovement();
        }
    }

    void ResetAnchor(ref Anchor anchor)
    {
        anchor.arm = 0;
        anchor.position = Vector3.zero;
        anchor.distance = 0f;
        anchor.rotation = Quaternion.identity;
        anchor.nulled = true;
        anchor.right = Vector3.zero;
    }

    bool CanTryToGrab()
    {
        return (Input.GetMouseButtonDown(0) && leftArmFree) || (Input.GetMouseButtonDown(1) && rightArmFree);
    }

    bool CanUngrab()
    {
        return(Input.GetMouseButtonDown(0) && !leftArmFree) || (Input.GetMouseButtonDown(1) && !rightArmFree);
    }

    bool CanFly()
    {
        return(Input.GetKeyDown(KeyCode.Space) && !flying);
    }
    
    bool LookingAtGrabbable()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        return Physics.Raycast(ray, out hitInfo, grabRange, unmovableObjectLayer);
    }

    void GrabOnObject()
    {
        //save anchor settings
        int index = 0;
        if (grabbing)
        {
            if (!anchors[1].nulled)
            {
                Debug.LogError("Tentou criar nova anchor, mas os dois bracos estavam indisponiveis, settando para braco 2");
            }
            index = 1;
        }
        SetUpAnchor(ref anchors[index]);

        if (Input.GetMouseButtonDown(0) && leftArmFree)
        {
            anchors[index].arm = 1;
            leftArmFree = false;
        }
        else if (rightArmFree)
        {
            anchors[index].arm = 2;
            rightArmFree = false;
        }
        if (index == 0)
        {
            needToSetup = true;
        }
        grabbing = true;
    }

    void SetUpAnchor(ref Anchor anchor)
    {
        anchor.position = hitInfo.point;
        anchor.distance = (rigidBody.position - anchor.position).magnitude;
        anchor.rotation = rigidBody.rotation;
        anchor.nulled = false;
        anchor.right = transform.right;
    }

    void HandleMovement()
    {
        float verticalDirection = 0f;
        if ((Input.GetKey(KeyCode.W)) && anchors[1].nulled)
        {
            verticalDirection = 1f;
        }
        else if ((Input.GetKey(KeyCode.S)) && anchors[1].nulled)
        {
            verticalDirection = -1f;
        }
        /*float horizontalDirection = 0f;
        if ((Input.GetKey(KeyCode.A)) && anchors[1].nulled)
        {
            horizontalDirection = 1f;
        }
        else if ((Input.GetKey(KeyCode.D)) && anchors[1].nulled)
        {
            horizontalDirection = -1f;
        }*/
        //anchors[0].rotation *= Quaternion.AngleAxis(5f * Time.fixedDeltaTime * horizontalDirection, Vector3.forward);
        Vector3 positionDifference = rigidBody.position - anchors[0].position;
        Quaternion rotation = Quaternion.AngleAxis(verticalAngularSpeed * Time.fixedDeltaTime * verticalDirection, anchors[0].right);
        Vector3 rotatedDifference = rotation * positionDifference;
        if (needToSetup)
        {
            setupCoroutine = StartCoroutine(SetupPositionAndRotation());
            needToSetup = false;
            print("Started Setup");
        }else if(stopSetup)
        {
            StopCoroutine(setupCoroutine);
            print("Finished Setup");
            stopSetup = false;
        }
        //print(transform.rotation + " transform.rotation");
        //print(rigidBody.rotation + " rigidBody.rotation");
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        if ((contactNormal == Vector3.zero) || (Vector3.Angle((rotatedDifference - positionDifference), contactNormal) <= 95f))
        {
            rigidBody.MovePosition((rotatedDifference.normalized * anchors[0].distance) + anchors[0].position);
            angleFormed += verticalDirection * Vector3.Angle(positionDifference, rigidBody.position - anchors[0].position);
            rigidBody.MoveRotation(Quaternion.AngleAxis(angleFormed, anchors[0].right) * anchors[0].rotation);
        }
        // check if body got too far from anchors[1]
    }

    IEnumerator SetupPositionAndRotation()
    {
        Quaternion originalRotation = anchors[0].rotation; // talvez tenha um problema pq quaternion e um reference value
        float originalAnchorDistance = anchors[0].distance;
        Vector3 positionDifference = anchors[0].position - rigidBody.position;
        float time = 0f;
        while (true)
        {
            time += Time.deltaTime / 2f;
            anchors[0].distance = Mathf.Lerp(originalAnchorDistance, grabRange - distanceFromRange, Mathf.Clamp01(time));
            anchors[0].rotation = Quaternion.Slerp(originalRotation, Quaternion.LookRotation(positionDifference, Vector3.up), Mathf.Clamp01(time*2));
            /*   para debugar essa funcao
            print(anchorDistance + " AnchorDistance");
            print(anchorRotation + " Rotation");
            print(Vector3.Angle(anchorRotation.eulerAngles, oldPositionDifference) + " Delta"); */
            anchors[0].right = transform.right;
            if (time >= 1f)
            {
                print("Ready to stop setup");
                stopSetup = true;
            }
            yield return null;
        }
    }

    void Ungrab()
    {
        // se o braco que precisa soltar e o principal e tem outro braco segurando
        if (Input.GetMouseButtonDown(anchors[0].arm-1) && (!anchors[1].nulled))
        {
            print("Making the other arm the main arm");
            ResetAnchor(ref anchors[0]);
            anchors[0] = anchors[1];
            anchors[0].rotation = rigidBody.rotation;
            anchors[0].right = transform.right;
            if(setupCoroutine != null)
            {
                StopCoroutine(setupCoroutine);
            }
            needToSetup = true;
            stopSetup = false;
            angleFormed = 0f;
            ResetAnchor(ref anchors[1]);
            switch(anchors[0].arm)
            {
                case 1:
                    rightArmFree = true;
                    break;
                case 2:
                    leftArmFree = true;
                    break;
            }
        }else if (Input.GetMouseButtonDown(anchors[0].arm - 1))
        {
            //se esta soltando do braco principal
            print("Letting go of main arm");
            switch (anchors[0].arm)
            {
                case 1:
                    leftArmFree = true;
                    break;
                case 2:
                    rightArmFree = true;
                    break;
            }
            ResetAnchor(ref anchors[0]);
            if (setupCoroutine != null)
            {
                StopCoroutine(setupCoroutine);
            }
            needToSetup = false;
            stopSetup = false;
            grabbing = false;
            angleFormed = 0f;
        }else
        {
            //se esta soltando do braco side
            print("Letting go of side arm");
            switch (anchors[1].arm)
            {
                case 1:
                    leftArmFree = true;
                    break;
                case 2:
                    rightArmFree = true;
                    break;
            }
            ResetAnchor(ref anchors[1]);
        }
    }

    IEnumerator SetupFly()
    {
        
        float timePassed = -3f;
        flying = true;
        stopFly = false;
        Vector3 startPosition = rigidBody.position;
        Quaternion startRotation = rigidBody.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(playerCamera.forward, Vector3.up);
        Quaternion startCameraLocalRotation = playerCamera.localRotation;
        Vector3 cameraUp = playerCamera.up;
        Vector3 targetForward = playerCamera.forward;
        playerCamera.SetParent(null);
        playerCamera.position = rigidBody.position + new Vector3(0, 0.5f, 0);
        playerCamera.rotation = rigidBody.rotation * startCameraLocalRotation;
        //playerCamera.rotation = Quaternion.LookRotation(targetForward, transform.up);
        while (true)
        {
            timePassed += Time.deltaTime * 5;
            Vector3 lastCameraRotation = playerCamera.localEulerAngles   ;
            print(playerCamera.rotation + " camera.rotation");
            //playerCamera.SetParent(null);
            rigidBody.MoveRotation(Quaternion.Lerp(startRotation, targetRotation, Mathf.Clamp01((timePassed + 3) / 5)));
            rigidBody.MovePosition(startPosition + targetForward * (timePassed * timePassed / 10f - 0.9f));
            //playerCamera.SetParent(transform);
            //playerCamera.position = startPosition + targetForward * (timePassed * timePassed / 10f - 0.9f);
            //playerCamera.localEulerAngles = lastCameraRotation;
            //playerCamera.localRotation = Quaternion.Lerp(startCameraLocalRotation, Quaternion.identity, Mathf.Clamp01((timePassed + 3) / 5));
            if (timePassed >= 3)
            {
                playerCamera.SetParent(transform);
                stopFly = true;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint point = collision.GetContact(0);
        collisionCollider = collision.collider;
        contactNormal = point.normal;
    }
    private void OnCollisionStay(Collision collision)
    {
        ContactPoint point = collision.GetContact(0);
        collisionCollider = collision.collider;
        contactNormal = point.normal;
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider == collisionCollider)
        {
            collisionCollider = null;
            contactNormal = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Color[] colors = new Color[2];
        colors[0] = Color.yellow;
        colors[1] = Color.blue;
        if(!anchors[0].nulled)
        {
            Gizmos.color = colors[(int)Mathf.Clamp01(anchors[0].arm - 1)];
            Gizmos.DrawSphere(anchors[0].position, 0.05f);
        }
        if (!anchors[1].nulled)
        {
            Gizmos.color = colors[(int)Mathf.Clamp01(anchors[1].arm - 1)];
            Gizmos.DrawSphere(anchors[1].position, 0.05f);
        }
    }
}
