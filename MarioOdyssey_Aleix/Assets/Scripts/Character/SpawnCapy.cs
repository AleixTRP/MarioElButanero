using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class SpawnCapy : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToSpawn; // El objeto que se spawneará.


    private GameObject spawnedObject; // Almacena la referencia al objeto spawnado.

    private float spawnTime; // Tiempo de creación del objeto

    [SerializeField]
    private Vector3 vc = new Vector3(0, 0, 3);

    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private float destroyDelay = 5f; // Tiempo en segundos antes de destruir el objeto.

    [SerializeField]
    private float spawnDistance = 3f;




    


    private void Start()
    {
       
        characterController.detectCollisions = false;
    }

    private void Update()
    {
        if (spawnedObject == null)
        {
            // Si no hay un objeto spawnado, verifica si puedes spawnear uno.
            if (Input_Manager._INPUT_MANAGER.GetCapyPress())
            {
                SpawnCapyObject();
            }
        }

        // Verifica si el objeto ha estado presente por más de destroyDelay segundos.
        if (spawnedObject != null && Time.time - spawnTime >= destroyDelay)
        {
            Destroy(spawnedObject);
            spawnedObject = null; // Marcar el objeto como destruido
        }
    }

    private void SpawnCapyObject()
    {
        if (objectToSpawn != null)
        {
            Vector3 spawnPosition = transform.position + spawnDistance * transform.forward;
            spawnedObject = Instantiate(objectToSpawn, spawnPosition, transform.rotation);
            spawnTime = Time.time; // Registra el tiempo de creación.
        }
    }




    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Character_Controller characterController = GetComponent<Character_Controller>();
       
        if (hit.collider.CompareTag("Capy"))
        {
            Vector3 customDirection = new Vector3(0.0f, 30f, 0.0f);

            
            characterController.CappyImpulse(customDirection);
        }
    }
}