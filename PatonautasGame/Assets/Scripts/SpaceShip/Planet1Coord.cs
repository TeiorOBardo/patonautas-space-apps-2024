using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet1Coord : MonoBehaviour
{
    public Vector3 initialPosition = new Vector3(15f, 9f, 559f);

    void Start()
    {
        transform.position = initialPosition; 
    }
}
