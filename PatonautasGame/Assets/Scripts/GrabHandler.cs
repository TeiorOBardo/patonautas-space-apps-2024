using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : MonoBehaviour
{
    public Transform playerCamera;
    bool leftArmFree = true;
    bool rightArmFree = true;
    public RaycastHit hitInfo;
    public float grabRange = 4f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if(Physics.Raycast(ray, out hitInfo, grabRange, 10));
            {
                print("hit");
            }
        }
    }
}
