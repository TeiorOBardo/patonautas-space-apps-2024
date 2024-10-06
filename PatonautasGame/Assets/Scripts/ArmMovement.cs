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

    private bool[] item = new bool[2];

    private Vector3[] hitPosition = new Vector3[2];

    public LayerMask unmovableObjectLayer;

    public LayerMask itemLayer;

    public LayerMask Interact;

    GameObject gameOBJ;

    string itemName;

    public static int value = 0;

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

        //Movimento braço esquerdo
        if (handsFree[0])
        {
            targetTargets[0] = offSets[0].position;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (handsFree[0])
            {
                if(Physics.Raycast(ray, out hitInfo, 1.5f, unmovableObjectLayer))
                {
                    hitPosition[0] = hitInfo.point;
                    targetTargets[0] = hitInfo.point + hitInfo.normal * 0.1f;
                    rotates[0] = Quaternion.FromToRotation(Vector3.forward, -hitInfo.normal);
                    handsFree[0] = false;
                }

                if(Physics.Raycast(ray, out hitInfo, 1.5f, itemLayer))
                {
                    targetTargets[0] = hitInfo.point + hitInfo.normal * 0.1f;
                    handsFree[0] = false;
                    hitPosition[0] = hitInfo.point;
                    gameOBJ = hitInfo.collider.gameObject;
                    itemName = gameOBJ.name;
                }
            }
            else
            {
                if (!item[0])
                {
                    handsFree[0] = true;
                }
            }
        }
        //Movimento braço direiro
        if (handsFree[1])
        {
            targetTargets[1] = offSets[1].position;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (handsFree[1])
            {
                if(Physics.Raycast(ray, out hitInfo, 1.5f, unmovableObjectLayer))
                {
                    hitPosition[1] = hitInfo.point;
                    targetTargets[1] = hitInfo.point + hitInfo.normal * 0.1f;
                    rotates[1] = Quaternion.FromToRotation(Vector3.forward, -hitInfo.normal);
                    handsFree[1] = false;
                } 

                if(Physics.Raycast(ray, out hitInfo, 1.5f, itemLayer))
                {
                    targetTargets[1] = hitInfo.point + hitInfo.normal * 0.1f;
                    handsFree[1] = false;
                    hitPosition[1] = hitInfo.point;
                    gameOBJ = hitInfo.collider.gameObject;
                }
            }
            else
            {
                if (!item[1])
                {
                    handsFree[1] = true;
                }

            }
        }

        if(Physics.Raycast(ray, out hitInfo, 2f, Interact))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                value++;
                Soltar(gameOBJ);
            }
        }

        //pulo
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

        //Rotação esquerda
        if (!handsFree[0]) //Rotação agarrada
        {
            if (item[0])
            {
                targetTargets[0] = offSets[0].position;
            }
            if(Physics.Raycast(rayT1, out hitInfo, 1.5f, unmovableObjectLayer))
            {
                targetTargets[0] = hitInfo.point + hitInfo.normal * 0.1f;
            }
            if(Physics.Raycast(rayT1, out hitInfo,1.5f, itemLayer))
            {
                if ((hitInfo.point - targets[0].position).magnitude <= 0.2)
                {
                    hitInfo.transform.SetParent(targets[0]);
                    item[0] = true;
                }
            }
            rotates[0] = Quaternion.FromToRotation(Vector3.forward, -hitInfo.normal);
            targets[0].rotation = Quaternion.Lerp(targets[0].rotation, rotates[0], 0.5f * Time.deltaTime * 5);
        }
        else //Rotação presa
        {
            targets[0].rotation = Quaternion.Lerp(targets[0].rotation, offSets[0].rotation, 0.5f * Time.deltaTime * 5);
        }
        //Rotação direita
        if (!handsFree[1]) //Rotação agarrada
        {
            if (item[1])
            {
                targetTargets[1] = offSets[1].position;
            }

            if(Physics.Raycast(rayT2, out hitInfo, 1.5f, unmovableObjectLayer))
            {
                targetTargets[1] = hitInfo.point + hitInfo.normal * 0.1f;
            }
            if(Physics.Raycast(rayT2, out hitInfo, 1.5f, itemLayer))
            {
                if ((hitInfo.point - targets[1].position).magnitude <= 0.2)
                {
                    hitInfo.transform.SetParent(targets[1]);
                    item[1] = true;
                }
            }
            rotates[1] = Quaternion.FromToRotation(Vector3.forward, -hitInfo.normal);
            targets[1].rotation = Quaternion.Lerp(targets[1].rotation, rotates[1], 0.5f * Time.deltaTime * 5);
        }
        else //Rotação presa
        {
            targets[1].rotation = Quaternion.Lerp(targets[1].rotation, offSets[1].rotation, 0.5f * Time.deltaTime * 5);
        }
    }
    public void Soltar(GameObject itemObj)
    {
        Destroy(itemObj);
        item[0] = false;
        item[1] = false;
        handsFree[1] = true;
        handsFree[0] = true;
    }
}
