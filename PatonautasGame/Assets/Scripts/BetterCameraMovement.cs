using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterCameraMovement : MonoBehaviour
{
    //NAO ESQUECA DE COLOCAR O TRANSFORM DO SEU PLAYER
    public Transform player;
    GrabHandler playerScript;
    private float verticalRotationLimitUp = 80f;
    private float verticalRotationLimitDown = 60f;
    private float horizontalRotationLimit = 110f;
    private float speed = 1f;

    //vou ter que alterar algumas coisas futuramente ainda

    // Update is called once per frame
    private void Start()
    { 
        //deixa o cursor travado no meio
        Cursor.lockState = CursorLockMode.Locked;
        playerScript = player.gameObject.GetComponent<GrabHandler>();
    }
    void Update()
    {
        if (!playerScript.flying && !HandlePause.isPaused)
        {
            //cria um vetor pra rotacao baseado no movimento do mouse
            Vector3 mouseRotation = new Vector3(-Input.GetAxisRaw("Mouse Y") * speed, Input.GetAxisRaw("Mouse X") * speed, 0);
            transform.localEulerAngles += mouseRotation;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

            //pega a diferenca entre a rotacao da camera e a do player
            float angleDiffenceX = Mathf.DeltaAngle(transform.localEulerAngles.y, player.localEulerAngles.y);
            float angleDiffenceY = Mathf.DeltaAngle(transform.localEulerAngles.x, player.localEulerAngles.x);

            transform.localEulerAngles = new Vector3(ClampAngle(transform.localEulerAngles.x, -verticalRotationLimitUp, verticalRotationLimitDown), ClampAngle(transform.localEulerAngles.y, -horizontalRotationLimit, horizontalRotationLimit), transform.localEulerAngles.z);
            /*if (transform.localEulerAngles.y > horizontalRotationLimit)
            {
                //se sim, nao deixa a diferenca passar do limite
                print(transform.localEulerAngles.y + " transform.localEulerAngles.y");
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, ClampAngle(transform.localEulerAngles.y, -horizontalRotationLimit, horizontalRotationLimit), transform.localEulerAngles.z);
            }
            if (transform.localEulerAngles.x > verticalRotationLimitDown)
            {
                print(transform.localEulerAngles.x + " transform.localEulerAngles.x");
                transform.localEulerAngles = new Vector3(ClampAngle(transform.localEulerAngles.x, -verticalRotationLimitUp, verticalRotationLimitDown), transform.localEulerAngles.y, transform.localEulerAngles.z);
            }*/
        }
    }

    //limita os angulos
    public static float ClampAngle(float angle, float min, float max)
    {
        float start = (min + max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
        return Mathf.Clamp(angle, min + floor, max + floor);
    }
}
