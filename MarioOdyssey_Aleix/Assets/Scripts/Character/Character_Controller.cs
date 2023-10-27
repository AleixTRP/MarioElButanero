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

    [SerializeField]
    private float velocityXZ = 5f;

    [SerializeField]
    private float initialJumpForce = 8f; // Fuerza inicial del primer salto

    [SerializeField]
    private float jumpForceIncrement = 2f; // Incremento de fuerza para los saltos posteriores

    [SerializeField]
    private int currentJump = 1; // N�mero de salto actual

    [SerializeField]
    private int maxJumps = 3; // N�mero m�ximo de saltos grandes

    private float coyoteTime;
    private Vector3 movementInput = Vector3.zero;

    [SerializeField]
    private GameObject cam;

    private bool isCrouching = false;

    private Animator animator;

    



    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controller.height = 2.0f; // Altura normal del personaje
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 direction = Quaternion.Euler(0f, cam.transform.eulerAngles.y, 0f) * new Vector3(movementInput.x, 0f, movementInput.z);
        direction.Normalize();

        Vector2 inputVector = Input_Manager._INPUT_MANAGER.GetLeftAxisValue();
        movementInput = new Vector3(inputVector.x, 0f, inputVector.y);
        movementInput.Normalize();

        // Calcular velocidad XZ
        finalVelocity.x = direction.x * velocityXZ;
        finalVelocity.z = direction.z * velocityXZ;

        float speed = finalVelocity.z;

        animator.SetFloat("velocity", speed);

        // Asignar direcci�n Y
        direction.y = -1f;

        if (Input_Manager._INPUT_MANAGER.GetCrouchMovement())
        {
            isCrouching = !isCrouching;
            animator.SetBool("crouch", false);

            if (isCrouching)
            {
                animator.SetBool("crouch", true);
                controller.height = 1.0f;
                velocityXZ = 2.5f; // Ajusta la velocidad al agacharse (puedes usar cualquier valor que prefieras)
            }
            else
            {
                animator.SetBool("crouch", false);
                controller.height = 2.0f;
                velocityXZ = 5.0f; // Restablece la velocidad al levantarse
            }
        }

        if (controller.isGrounded)
        {
            coyoteTime = 0.1f;

            if (Input_Manager._INPUT_MANAGER.GetSouthButtonPressed())
            {
                animator.SetBool("isGround", false);

                finalVelocity.y = initialJumpForce;
                initialJumpForce += jumpForceIncrement;
                animator.SetInteger("jump", currentJump + 1);
                currentJump++;

                if (currentJump > maxJumps)
                {
                    initialJumpForce = 8f;
                    currentJump = 1;
                }
            }
            else
            {

                animator.SetBool("isGround", true);
            }
        }
        else
        {
            finalVelocity.y -= gravity * Time.deltaTime;
            coyoteTime -= Time.deltaTime;
        }
        if (finalVelocity.y < -0.1f)
        {
            animator.SetBool("fall", true);
        }
        else
        {
            animator.SetBool("fall", false);
        }

        controller.Move(finalVelocity * Time.deltaTime);
    }
}



