using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    private bool isCamera1Active = true;

    void Start()
    {
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            isCamera1Active = !isCamera1Active;
            camera1.gameObject.SetActive(isCamera1Active);
            camera2.gameObject.SetActive(!isCamera1Active);
        }
    }
}
