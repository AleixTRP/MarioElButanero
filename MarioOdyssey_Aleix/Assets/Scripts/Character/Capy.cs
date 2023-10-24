using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capy : MonoBehaviour
{

    [SerializeField]
    private Vector3 imapct = new Vector3(0, 50, 0);

    [SerializeField]
    private CharacterController characterController;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Butanero")
        {
            characterController.Move(imapct * Time.deltaTime);
            Debug.Log("Imapcta");
        }
    }
}
