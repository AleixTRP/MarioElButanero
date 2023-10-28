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
    private int currentJump = 1; // Número de salto actual

    [SerializeField]
    private int maxJumps = 3; // Número máximo de saltos grandes

    private float coyoteTime;
    private Vector3 movementInput = Vector3.zero;

    [SerializeField]
    private GameObject cam;

    private bool isCrouching = false;

    private Animator animator;

    private float acceleration = 5f;

    private float speedCameraX = 0.2f;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controller.height = 2.0f; // Altura normal del personaje
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        Vector2 inputVector = Input_Manager._INPUT_MANAGER.GetLeftAxisValue();
        movementInput = new Vector3(inputVector.x, 0f, inputVector.y);
        movementInput.Normalize();

        Vector3 direction = Quaternion.Euler(0f, cam.transform.eulerAngles.y, 0f) * new Vector3(movementInput.x, 0f, movementInput.z);
        direction.Normalize();

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // Calcular velocidad XZ
        finalVelocity.x = Mathf.MoveTowards(finalVelocity.x, direction.x * velocityXZ, acceleration * Time.deltaTime);
        finalVelocity.z = Mathf.MoveTowards(finalVelocity.z, direction.z * velocityXZ, acceleration * Time.deltaTime);

        float speed = Mathf.Abs(finalVelocity.z);
        float speedX = Mathf.Abs(finalVelocity.x);

        if (speed < speedCameraX && speedX >= speedCameraX)
        {
            // Si la velocidad en Z es baja pero la velocidad en X es suficiente, consideramos que estamos en movimiento en la dirección X.
            speed = Mathf.Abs(finalVelocity.x); // Usar la velocidad de X original
            animator.SetBool("isRun", false); // Desactivar la animación de correr
        }
        else if (speed >= speedCameraX)
        {
            // La velocidad en Z es suficiente para considerar que estamos en movimiento hacia adelante.
            speed = Mathf.Abs(finalVelocity.z); // Usar la velocidad de Z original
            animator.SetBool("isRun", true); // Activar la animación de correr
        }
        else
        {
            // Ambas velocidades son bajas, consideramos que estamos prácticamente detenidos.
            speed = 0.0f;
            animator.SetBool("isRun", false); // Desactivar la animación de correr
        }

        animator.SetFloat("velocity", speed);
        animator.SetFloat("velocityX", speedX);

        // Asignar dirección Y
        direction.y = -1f;

        if (Input_Manager._INPUT_MANAGER.GetCrouchMovement())
        {
            isCrouching = !isCrouching;
            animator.SetBool("crouch", false);

            if (isCrouching)
            {
                animator.SetBool("crouch", true);
                controller.height = 1.0f;
                velocityXZ = 2.5f; // Ajusta la velocidad al agacharse 
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
                animator.SetInteger("jump", currentJump);
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



