using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class DanceCharacter : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private AudioClip soundClip;
   
 

    [SerializeField]
    private AudioSource audioSource;



    private bool soundPlaying = false;

    private CharacterController characterController;

    private float timeSinceLastChange = 0f; // Tiempo transcurrido desde el �ltimo cambio de animaci�n
    private bool isDancing = false; // Indica si est� bailando

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
       
        characterController = GetComponent<CharacterController>();
    }

    
    void Update()
        {
       
        if (Input_Manager._INPUT_MANAGER.GetDanceMove() && !soundPlaying)
            {

                isDancing = true;
                animator.SetBool("isDance", true);
                soundPlaying = true;
                audioSource.PlayOneShot(soundClip);
            }

            if (!audioSource.isPlaying)
            {
                soundPlaying = false;
            }

            if (characterController.velocity.magnitude > 0)
            {
                isDancing = false;
                animator.SetBool("isDance", false);
                audioSource.Stop();
            }

            // Si est� bailando, actualiza el tiempo transcurrido
            if (isDancing)
            {
                timeSinceLastChange += Time.deltaTime;

                // Verifica si han pasado 10 segundos y cambia a la siguiente animaci�n
                if (timeSinceLastChange >= 10f)
                {
                    animator.SetInteger("newDance", 1);
                }

                // Verifica si han pasado 20 segundos y reinicia el tiempo y cambia a la primera animaci�n
                if (timeSinceLastChange >= 20f)
                {
                    timeSinceLastChange = 0f;
                    animator.SetInteger("newDance", 0);
                }
            }
        }
    }

