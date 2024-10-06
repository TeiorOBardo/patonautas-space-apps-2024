using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ArmMovement : MonoBehaviour
{
    public Transform[] targets = new Transform[2];

    public Transform[] offSets = new Transform[2];

    private Vector3[] targetTargets = new Vector3[2];

    private Quaternion[] rotates = new Quaternion[2];

    private bool[] handsFree = new bool[2] {true, true};

    private Vector3[] hitPosition = new Vector3[2];

    private void Start()
    {
        targetTargets[0] = offSets[0].position;
        targetTargets[1] = offSets[1].position;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        Ray rayT1 = new Ray(transform.position, hitPosition[0] - transform.position);
        Ray rayT2 = new Ray(transform.position, hitPosition[1] - transform.position);


        RaycastHit hitInfo;

        Debug.DrawRay(transform.position, targetTargets[0], Color.red);
        //Movimento bra�o esquerdo
        if (handsFree[0])
        {
            targetTargets[0] = offSets[0].position;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (handsFree[0])
            {
                if(Physics.Raycast(ray, out hitInfo, 1.5f))
                {
                    hitPosition[0] = hitInfo.point;
                    targetTargets[0] = hitInfo.point + hitInfo.normal * 0.1f;
                    rotates[0] = Quaternion.FromToRotation(Vector3.forward, -hitInfo.normal);
                    handsFree[0] = false;
                }
            }
            else
            {
                handsFree[0] = true;
            }
        }
        //Movimento bra�o direiro
        if (handsFree[1])
        {
            targetTargets[1] = offSets[1].position;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (handsFree[1])
            {
                if(Physics.Raycast(ray, out hitInfo, 1.5f))
                {
                    hitPosition[1] = hitInfo.point;
                    targetTargets[1] = hitInfo.point + hitInfo.normal * 0.1f;
                    rotates[1] = Quaternion.FromToRotation(Vector3.forward, -hitInfo.normal);
                    handsFree[1] = false;
                } 
            }
            else
            {
                handsFree[1] = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float timePassed = 0;
            bool aguarde = true;
            while (aguarde)
            {
                if (timePassed >= 1.3f)
                {
                    handsFree[0] = true;
                    handsFree[1] = true;
                    aguarde = false;
                }
                timePassed += Time.deltaTime;
            }
        }

        //Movimento geral
        targets[0].position = Vector3.Lerp(targets[0].position, targetTargets[0], 0.5f * Time.deltaTime * 5);
        targets[1].position = Vector3.Lerp(targets[1].position, targetTargets[1], 0.5f * Time.deltaTime * 5);

        //Rota��o esquerda
        if (!handsFree[0])
        {
            if(Physics.Raycast(rayT1, out hitInfo, 1.5f))
            {
                targetTargets[0] = hitInfo.point + hitInfo.normal * 0.1f;
                
            }
            rotates[0] = Quaternion.FromToRotation(Vector3.forward, -hitInfo.normal);
            targets[0].rotation = Quaternion.Lerp(targets[0].rotation, rotates[0], 0.5f * Time.deltaTime * 5);
        }
        else
        {
            targets[0].rotation = Quaternion.Lerp(targets[0].rotation, offSets[0].rotation, 0.5f * Time.deltaTime * 5);
        }
        //Rota��o direita
        if (!handsFree[1])
        {
            if(Physics.Raycast(rayT2, out hitInfo, 1.5f))
            {
                targetTargets[1] = hitInfo.point + hitInfo.normal * 0.1f;
            }
            rotates[1] = Quaternion.FromToRotation(Vector3.forward, -hitInfo.normal);
            targets[1].rotation = Quaternion.Lerp(targets[1].rotation, rotates[1], 0.5f * Time.deltaTime * 5);
        }
        else
        {
            targets[1].rotation = Quaternion.Lerp(targets[1].rotation, offSets[1].rotation, 0.5f * Time.deltaTime * 5);
        }
    }
}
