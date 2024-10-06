using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPosition : MonoBehaviour
{
    public Transform spaceship;  
    public Transform station1;  
    public Transform station2;  

    void Start()
    {
        Vector3 globalPosition;

        // Verifica de qual estação a nave veio
        if (LastPositionRelativeToStation.stationId == 1)
        {
            
            globalPosition = station1.TransformPoint(LastPositionRelativeToStation.lastRelativePosition);
        }
        else if (LastPositionRelativeToStation.stationId == 2)
        {
            
            globalPosition = station2.TransformPoint(LastPositionRelativeToStation.lastRelativePosition);
        }
        else
        {
            globalPosition = spaceship.position;
        }

        spaceship.position = globalPosition;
    }
}
