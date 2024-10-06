using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceCollision : MonoBehaviour
{
    public Transform station;  
    public Transform spaceship;
    public Transform spaceship2;
    private Vector3 relativePosition;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "StationEntrance")
        {
            relativePosition = station.InverseTransformPoint(spaceship.position);
            LastPositionRelativeToStation.lastRelativePosition = relativePosition;
            LastStation1.lastPosition1 = spaceship.position;
            LastPositionRelativeToStation.stationId = 1;
            SceneManager.LoadScene("SpaceStation");
        }
        if (collider.tag == "StationEntrance2")
        {
            relativePosition = station.InverseTransformPoint(spaceship2.position);
            LastPositionRelativeToStation.lastRelativePosition = relativePosition;
            LastStation2.lastPosition2 = spaceship2.position;
            LastPositionRelativeToStation.stationId = 2;
            SceneManager.LoadScene("SpaceStation2");
        }
    }
}
