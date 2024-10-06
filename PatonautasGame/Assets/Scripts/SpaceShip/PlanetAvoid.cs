using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAvoid : MonoBehaviour
{
    public Transform planet;
    public float minDistance;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, planet.position);

        if (distance < minDistance)
        {
            Vector3 direction = (transform.position - planet.position).normalized;
            transform.position = planet.position + direction * minDistance;
        }
    }
}
