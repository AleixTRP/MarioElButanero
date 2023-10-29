using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Input_Manager : MonoBehaviour
{
    private PlayerInputActions playerInputs;
    public static Input_Manager _INPUT_MANAGER;

    private float timeSinceJumpPressed = 0f;


    private Vector2 leftAxisValue = Vector2.zero;

    private Vector2 cam = Vector2.zero;

    private float crouch = 0f;

    private float capy = 0f;
   
    private float spawn = 0f;

    private float dance = 0f;

    private float jumpB = 0f;

    private float jumpF = 0f;


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
            playerInputs.Character.Crouch.performed += CrouchMovement;
            playerInputs.Character.Capy.performed += CapyPress;
            playerInputs.Character.Spawn.performed += SpawnPress;
            playerInputs.Character.Dance.performed += DanceMove;
            playerInputs.Character.JumpB.performed += JumpBack;
            playerInputs.Character.JumpF.performed += JumpFront;

            _INPUT_MANAGER = this;
            DontDestroyOnLoad(this);
        }

    }
    private void Update()
    {
        timeSinceJumpPressed += Time.deltaTime;

        crouch += Time.deltaTime;

        capy += Time.deltaTime;

        spawn += Time.deltaTime;

        dance += Time.deltaTime;

        jumpB += Time.deltaTime;

        jumpF += Time.deltaTime;

        InputSystem.Update();

    }
    private void JumpButtonPressed(InputAction.CallbackContext context)
    {
        timeSinceJumpPressed = 0f;
    }

    public bool GetSouthButtonPressed()
    {
        return this.timeSinceJumpPressed == 0f;
    }


    private void LeftAxisUpdate(InputAction.CallbackContext context)
    {

        leftAxisValue = context.ReadValue<Vector2>();

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

    private void CrouchMovement(InputAction.CallbackContext context)
    {
        crouch = 0f;
    }

    public bool GetCrouchMovement()
    {
        return this.crouch == 0f;
    }

    private void CapyPress(InputAction.CallbackContext context)
    {
        capy = 0f;
    }

    public bool GetCapyPress()
    {
        return this.capy == 0f;

    }

    private void SpawnPress(InputAction.CallbackContext context)
    {
        spawn = 0f;
    }

    public bool GetSpawnPress()
    {
        return this.spawn == 0f;

    }

    private void DanceMove(InputAction.CallbackContext context)
    {
        dance = 0f;
    }

    public bool GetDanceMove()
    {
        return this.dance == 0f;
    }

    private void  JumpBack(InputAction.CallbackContext context)
    {
        jumpB = 0f;
    }

    public bool GetJumpBack()
    {
        return this.jumpB == 0f;
    }

    private void JumpFront(InputAction.CallbackContext context)
    {
        jumpF = 0f;
    }

    public bool GetJumpFront()
    {
        return this.jumpF == 0f;
    }

}
