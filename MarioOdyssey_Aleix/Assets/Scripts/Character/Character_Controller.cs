using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    private float acceleration = 5f;

    private float speedCameraX = 0.2f;

    private float customImpulse = 20f;

    private float platformImpulse = 30f;

    private float flipForce = 10.0f;


    private float impulseX = 8f;

    private float ducks;



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

            speed = Mathf.Abs(finalVelocity.x); // Usar la velocidad de X original
            animator.SetBool("isRun", false); // Desactivar la animaci�n de correr
        }
        else if (speed >= speedCameraX)
        {

            speed = Mathf.Abs(finalVelocity.z); // Usar la velocidad de Z original
            animator.SetBool("isRun", true); // Activar la animaci�n de correr
        }
        else
        {

            speed = 0.0f;
            animator.SetBool("isRun", false); // Desactivar la animaci�n de correr
        }

        animator.SetFloat("velocity", speed);
        animator.SetFloat("velocityX", speedX);

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
                velocityXZ = 2.5f;

            }

            else
            {
                animator.SetBool("crouch", false);
                controller.height = 2.0f;
                velocityXZ = 5.0f;
            }

        }

        if (controller.isGrounded && isCrouching)
        {
            if (Input_Manager._INPUT_MANAGER.GetJumpBack())
            {
                LargeJump();
            }

            if (Input_Manager._INPUT_MANAGER.GetJumpFront())
            {
                BackFlip();
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
    public void CappyImpulse(Vector3 direction)
    {
        finalVelocity.y = customImpulse;
    }

    public void PlatformImpulse(Vector3 direction)
    {
        finalVelocity.y = platformImpulse;
    }

    private void LargeJump()
    {
        finalVelocity += -transform.forward * flipForce;
        finalVelocity.y = flipForce;

    }
    private void BackFlip()
    {
        finalVelocity += transform.forward * flipForce;
        finalVelocity.y = flipForce;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Wall"))
        {
              if (!controller.isGrounded)
                {
                    if (Input_Manager._INPUT_MANAGER.GetSouthButtonPressed())
                    {
                        finalVelocity += -transform.forward * impulseX;
                    finalVelocity.y = impulseX;

                }
            }
        }

        if (hit.collider.CompareTag("Water"))
        {
            SceneManager.LoadScene("Odysse");
        }
        if (hit.collider.CompareTag("Duck"))
        {
            ducks++;
            Destroy(hit.gameObject);

        }
        if (ducks == 13)
        {
            SceneManager.LoadScene("Win");
        }
    }
}



 




