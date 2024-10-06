using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet2Coord : MonoBehaviour
{
    public Vector3 initialPosition = new Vector3(3415f, 9f, 2301f);

    void Start()
    {
        transform.position = initialPosition;
    }
}
