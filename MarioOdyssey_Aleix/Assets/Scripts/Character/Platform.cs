using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Character_Controller characterController = GetComponent<Character_Controller>();

        if (hit.collider.CompareTag("Platform"))
        {
            Vector3 customDirection = new Vector3(0.0f, 80f, 0.0f);


            characterController.PlatformImpulse(customDirection);
        }
    }
}

