using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnCapy : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToSpawn; // El objeto que se spawneará.
    [SerializeField]
    private float destroyDelay = 5f; // Tiempo en segundos antes de destruir el objeto.

    private bool buttonsPressed = false;

    [SerializeField]
    private Vector3 vc = new Vector3 (0, 0, 3);


    [SerializeField]
    private Vector3 imapct = new Vector3(0, 1, 0);

    [SerializeField]
    private CharacterController characterController;


    private void Start()
    {
        characterController.detectCollisions = false;
    }




    private void Update()
    {
        // Comprueba si se presionan los botones R1 y L1 al mismo tiempo en el mando.
        if (Gamepad.current != null &&
            Gamepad.current.buttonNorth.isPressed || Keyboard.current.fKey.isPressed)
        {
            if (!buttonsPressed)
            {
                buttonsPressed = true;
                Capy();
            }
        }
        else
        {
            buttonsPressed = false;
        }


    }

    private void Capy()
    {
        if (objectToSpawn != null)
        {
            // Instancia el objeto en la posición actual del personaje 
            GameObject spawnedObject = Instantiate(objectToSpawn, transform.position + vc, transform.rotation);
            
            // Destruye el objeto después del tiempo especificado.
            Destroy(spawnedObject, destroyDelay);
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Capy")
        {
            Debug.Log("Entra");
            characterController.SimpleMove(Vector3.up * 2000f * Time.deltaTime);
        }
    }






}