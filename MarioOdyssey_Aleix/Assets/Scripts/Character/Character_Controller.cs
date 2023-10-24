using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character_Controller : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 finalVelocity = Vector3.zero;
    private float gravity = 20f;
    private float moveSpeed = 5f;
    private float velocityXZ = 5f;

    [SerializeField]
    private float initialJumpForce = 8f; // Fuerza inicial del primer salto

    [SerializeField]
    private float jumpForceIncrement = 2f; // Incremento de fuerza para los saltos posteriores

    [SerializeField]
    private int currentJump = 1; // Número de salto actual

    [SerializeField]
    private int maxJumps = 3; // Número máximo de saltos grandes

    private float coyoteTime = 0.1f;

    private Vector3 movementInput = Vector3.zero;

    [SerializeField]
    private GameObject cam;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {

        Vector3 direction = Quaternion.Euler(0f, cam.transform.eulerAngles.y, 0f) * new Vector3(movementInput.x, 0f, movementInput.z);
        direction.Normalize();



        Vector2 inputVector = Input_Manager._INPUT_MANAGER.GetLeftAxisValue();
        movementInput = new Vector3(inputVector.x, 0f, inputVector.y);
        movementInput.Normalize();

        //Calcular velocidad XZ
        finalVelocity.x = direction.x * velocityXZ;
        finalVelocity.z = direction.z * velocityXZ;

        //Asignar dirección Y
        direction.y = -1f;
        
        Vector3 move = movementInput * moveSpeed;

        if (controller.isGrounded)
        {
            coyoteTime = 0.1f;

            if (Input_Manager._INPUT_MANAGER.GetSouthButtonPressed())
            {
                finalVelocity.y = initialJumpForce;
                initialJumpForce += jumpForceIncrement;
                currentJump++;

                if (currentJump > maxJumps)
                {
                    initialJumpForce = 8f; // Restablecer la fuerza del primer salto
                    currentJump = 1;
                }
            }
        }
        else
        {
            finalVelocity.y -= gravity * Time.deltaTime;
            coyoteTime -= Time.deltaTime;
        }

        controller.Move((move + Vector3.up * finalVelocity.y) * Time.deltaTime);
    }

}