using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterCameraMovement : MonoBehaviour
{
    //NAO ESQUECA DE COLOCAR O TRANSFORM DO SEU PLAYER
    public Transform player;
    private float verticalRotationLimitUp = 80f;
    private float verticalRotationLimitDown = 60f;
    private float horizontalRotationLimit = 110f;
    private float speed = 1f;

    //vou ter que alterar algumas coisas futuramente ainda

    // Update is called once per frame
    void Update()
    {
        //deixa o cursor travado no meio
        Cursor.lockState = CursorLockMode.Locked;
        //cria um vetor pra rotacao baseado no movimento do mouse
        Vector3 mouseRotation = new Vector3(-Input.GetAxisRaw("Mouse Y") * speed, Input.GetAxisRaw("Mouse X") * speed, 0);
        //rotaciona
        transform.localEulerAngles += mouseRotation;
        //nao deixa o z rotacionar
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
        //pega a diferenca entre a rotacao da camera e a do player no eixo x
        float angleDiffenceX = Mathf.DeltaAngle(transform.localEulerAngles.y, player.localEulerAngles.y);
        //pega a diferenca entre a rotacao da camera e a do player no eixo y
        float angleDiffenceY = Mathf.DeltaAngle(transform.localEulerAngles.x, player.localEulerAngles.x);
        //verifica se a diferenca e maior que o limite
        if (Mathf.Abs(angleDiffenceX) > horizontalRotationLimit)
        {
            //se sim, nao deixa a diferenca passar do limite
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, ClampAngle(transform.localEulerAngles.y, -horizontalRotationLimit, horizontalRotationLimit), transform.localEulerAngles.z);
        }
        if (Mathf.Abs(angleDiffenceY) > verticalRotationLimitDown)
        {
            //se sim, nao deixa a diferenca passar do limite
            transform.localEulerAngles = new Vector3(ClampAngle(transform.localEulerAngles.x, -verticalRotationLimitUp, verticalRotationLimitDown), transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        //armazena a nova posicao do mouse
    }

    //limita os angulos
    public static float ClampAngle(float angle, float min, float max)
    {
        float start = (min + max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
        return Mathf.Clamp(angle, min + floor, max + floor);
    }
}
