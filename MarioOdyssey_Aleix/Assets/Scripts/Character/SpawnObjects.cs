using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToSpawn; // El objeto que se spawneará.
    [SerializeField]
    private float destroyDelay = 5f; // Tiempo en segundos antes de destruir el objeto.

    private bool buttonsPressed = false;

    private void Update()
    {
        // Comprueba si se presionan los botones R1 y L1 al mismo tiempo en el mando.
        if (Input_Manager._INPUT_MANAGER.GetSpawnPress())
        {
            if (!buttonsPressed)
            {
                buttonsPressed = true;
                SpawnObject();
            }
        }
        else
        {
            buttonsPressed = false;
        }
    }

    private void SpawnObject()
    {
        if (objectToSpawn != null)
        {
            // Instancia el objeto en la posición actual del personaje 
            GameObject spawnedObject = Instantiate(objectToSpawn, transform.position, transform.rotation);

            // Destruye el objeto después del tiempo especificado.
            Destroy(spawnedObject, destroyDelay);
        }
    }
}