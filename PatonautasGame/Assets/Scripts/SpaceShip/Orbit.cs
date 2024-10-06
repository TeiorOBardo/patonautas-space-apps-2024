using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform planet;  
    public float orbitSpeed = 10f;  
    public float orbitRadius = 50f;  

    void Start()
    {
        Vector3 offset = (transform.position - planet.position).normalized * orbitRadius;
        transform.position = planet.position + offset;
    }

    void Update()
    {
        OrbitAroundPlanet();
    }

    void OrbitAroundPlanet()
    {
        transform.RotateAround(planet.position, Vector3.up, orbitSpeed * Time.deltaTime);
    }
}
