using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Input_Manager : MonoBehaviour
{
    private PlayerInputActions playerInputs;
    public static Input_Manager _INPUT_MANAGER;

    private float timeSinceJumpPressed = 0f;

    private Vector2 leftAxisValue = Vector2.zero;
   
    private Vector2 cam = Vector2.zero;


    private void Awake()
    {
        if (_INPUT_MANAGER != null && _INPUT_MANAGER != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            playerInputs = new PlayerInputActions();
            playerInputs.Character.Enable();
            playerInputs.Character.Jump.performed += JumpButtonPressed;
            playerInputs.Character.Move.performed += LeftAxisUpdate;
            playerInputs.Character.Camera.performed += CameraMovement;
            _INPUT_MANAGER = this;
            DontDestroyOnLoad(this);
        }

    }
    private void Update()
    {
        timeSinceJumpPressed += Time.deltaTime;
        InputSystem.Update();

    }
    private void JumpButtonPressed(InputAction.CallbackContext context)
    {
        timeSinceJumpPressed = 0f;
    }

    private void LeftAxisUpdate(InputAction.CallbackContext context)
    {
        
       leftAxisValue = context.ReadValue<Vector2>();
     
    }   

    public bool GetSouthButtonPressed()
    {
        return this.timeSinceJumpPressed == 0f;
    }

    public Vector2 GetLeftAxisValue()
    {
         return this.leftAxisValue; 
    }
    
  private void CameraMovement(InputAction.CallbackContext context)
    {

        cam = context.ReadValue<Vector2>();

    }

    public Vector2 GetCameraValue()
    {
        return this.cam;
    }


}
