using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station2Pos : MonoBehaviour
{
    void Start()
    {
        transform.position = LastStation2.lastPosition2;
    }
}
