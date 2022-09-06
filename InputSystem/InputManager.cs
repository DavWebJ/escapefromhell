using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using BlackPearl;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public PlayerInputAction inputs;
    public UIInput uiInputs;
    public FlashLightInput flInputs;
    public GunInput gunInput;
    PlayerInputAction.PlayerMovementActions playerMovement;
    PlayerInputAction.UIActions uIActions;
    

    private FirstPersonAIO player;

    Keyboard keyboard;
    Gamepad gamepad;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    float leftTrigger;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        player = GetComponent<FirstPersonAIO>();
        inputs = new PlayerInputAction();
        uiInputs = new UIInput();
        gunInput = new GunInput();
        flInputs = new FlashLightInput();
        playerMovement = inputs.PlayerMovement;
        uIActions = inputs.UI;

        // horizontal movement
        if (player.playerCanMove)
        {
            playerMovement.horizontal.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        }
        
        // mouse look 
        playerMovement.mousex.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        playerMovement.mousey.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
        // jump

        playerMovement.Jump.performed += ctx =>player.Jumpinput();
        // sprint
        playerMovement.sprint.performed += ctx => player.sprintInput = true;
        playerMovement.sprint.canceled += ctx => player.sprintInput = false;

        // hotbar input
        uIActions.selectNext.performed += ctx => HotBar.instance.SelectNext();
        uIActions.selectPrev.performed += ctx => HotBar.instance.SelectPrev();

        // interract pick up
        uiInputs.InterractAction.interract.performed += ctx =>player.fpscam.PickUpInput();

        // gun input

        keyboard = Keyboard.current;
        gamepad = Gamepad.current;

    }
    
    private void Start()
    {
        
    }

    public void OnEnable()
    {
        inputs.Enable();
        inputs.UI.Enable();
        uiInputs.Enable();
        flInputs.Enable();
        gunInput.Enable();
    }

    public void OnDisable()
    {
        inputs.Disable();
        uiInputs.Disable();
        flInputs.Disable();
        gunInput.Disable();
    }

    void Update()
    {

        if(keyboard != null)
        {
            GameManager.instance.enabledKeyboard = true;
        }

        if(gamepad != null)
        {
            GameManager.instance.enabledGamepad = true;
        }
        player.receivedInput(horizontalInput);
        player.receivedmouseInput(mouseInput);
    }
}
