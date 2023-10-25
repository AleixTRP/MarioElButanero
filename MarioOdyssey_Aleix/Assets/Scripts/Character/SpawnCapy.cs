using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnCapy : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToSpawn; // El objeto que se spawnear�.


    private GameObject spawnedObject; // Almacena la referencia al objeto spawnado.
   
    private float spawnTime; // Tiempo de creaci�n del objeto
   
    [SerializeField]
    private Vector3 vc = new Vector3(0, 0, 3);

    [SerializeField]
    private CharacterController characterController;

    private Vector3 bounceDirection = Vector3.up;
   
    [SerializeField]
    private float bounceForce = 20f;

    [SerializeField]
    private float destroyDelay = 5f; // Tiempo en segundos antes de destruir el objeto.

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

        // Verifica si el objeto ha estado presente por m�s de destroyDelay segundos.
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
            // Instancia el nuevo objeto en la posici�n actual del personaje.
            spawnedObject = Instantiate(objectToSpawn, transform.position + vc, transform.rotation);
            spawnTime = Time.time; // Registra el tiempo de creaci�n.
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Capy"))
        {
            characterController.Move(bounceDirection * bounceForce);
        }
    }
}