using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;

public class MovePlayerCamera : MonoBehaviour
{
    public Transform player;
    float verticalRotationLimit = 30f;
    float horizontalRotationLimit = 110f;
    float speed = 0.2f;
    Vector3 lastMousePosition;
    Vector3 startMousePosition;
    void Start()
    {
        lastMousePosition = Input.mousePosition;
        startMousePosition = lastMousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        //Cursor.visible = false
        
        lastMousePosition = Input.mousePosition - lastMousePosition;
        Vector3 mouseRotation = new Vector3(-lastMousePosition.y * speed, lastMousePosition.x * speed, 0);
        transform.eulerAngles += mouseRotation;
        float angleDiffence = Mathf.DeltaAngle(transform.eulerAngles.y , player.eulerAngles.y);
        if(Mathf.Abs(angleDiffence) > horizontalRotationLimit)
        {
            print(angleDiffence);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -Mathf.Clamp(angleDiffence, -1, 1) * horizontalRotationLimit, transform.eulerAngles.z);
            print(Mathf.DeltaAngle(transform.eulerAngles.y , player.eulerAngles.y));
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        lastMousePosition = Input.mousePosition;
    }
}
