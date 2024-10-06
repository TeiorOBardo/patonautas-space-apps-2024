using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceCollision : MonoBehaviour
{
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "StationEntrance")
        {
            SceneManager.LoadScene("SpaceStation");
        }
        if (collider.tag == "StationEntrance2")
        {
            SceneManager.LoadScene("SpaceStation2");
        }
    }
}
