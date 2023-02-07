using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using SUPERCharacter;
using BlackPearl;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public PlayerInputAction inputs;
    public InventoryManagerInput inventoryInputs;
    InventoryManagerInput.InventoryActionActions inventoryAction;
    PlayerInputAction.HotbarActionInputActions hotbarActions;
    public UIInput uiInputs;
    UIInput.ActionFromInventoryActions inventoryInputAction;
    public FlashLightInput flInputs;
    FlashLightInput.FlashLightInputControllerActions flashlightActions;
    public GunInput gunInput;
    GunInput.GunInputControllerActions GunActions;
    public AxeInput axeInput;
    AxeInput.AxeInputControllerActions axeActions;
    PlayerInputAction.PlayerMovementActions playerMovementActions;
    PlayerInput _controls;
    private PickUp pickup;
    private GameObject interactableObject = null;
    public static ControlDeviceType currentControlDevice;
    public enum ControlDeviceType
    {
        KeyboardAndMouse,
        Gamepad,
    }

    private SUPERCharacterAIO player;
    public string currentControlScheme { get; }
    Keyboard keyboard;
    Gamepad gamepad;

    Vector2 horizontalInput;
    Vector2 mouseInput;
    public InputSystemUIInputModule inputs_ui;
    float leftTrigger;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        player = GetComponent<SUPERCharacterAIO>();
        inputs = new PlayerInputAction();
        pickup = GetComponent<PickUp>();
        uiInputs = new UIInput();
        gunInput = new GunInput();
        flInputs = new FlashLightInput();
        axeInput = new AxeInput();
        playerMovementActions = inputs.PlayerMovement;
        inventoryInputs = new InventoryManagerInput();
        inventoryAction = inventoryInputs.InventoryAction;
        hotbarActions = inputs.HotbarActionInput;
        inventoryInputAction = uiInputs.ActionFromInventory;
        axeActions = axeInput.AxeInputController;
        GunActions = gunInput.GunInputController;
        flashlightActions = flInputs.FlashLightInputController;

        // horizontal movement
        if (player.enableMovementControl)
        {
            playerMovementActions.horizontal.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        }
        
        // mouse look 
        playerMovementActions.mousex.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        playerMovementActions.mousey.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
        // jump
        if (player.enableMovementControl)
        {
            playerMovementActions.Jump.performed += ctx => player.jumpInput = true;
            playerMovementActions.Jump.canceled += ctx => player.jumpInput = false;
        }

     
        // sprint
        playerMovementActions.sprint.performed += ctx => player.sprintInput = true;
        playerMovementActions.sprint.canceled += ctx => player.sprintInput = false;

        // crouch
        playerMovementActions.crouch.performed += ctx => player.crouchInput = true;
        playerMovementActions.crouch.canceled += ctx => player.crouchInput = false;

        // hotbar input
        hotbarActions.selectNext.performed += ctx => HotBar.instance.SelectNext();
        hotbarActions.selectPrev.performed += ctx => HotBar.instance.SelectPrev();
        // inventory show
        inventoryAction.ShowInventory.performed += OnShowInventoryPressed;
        // input pickup
        inventoryAction.pickup.performed += ctx => pickup.OnPickUpItem();
        // input equip
        inventoryAction.Equip.performed += ctx => pickup.OnEquipItem();
        // interract
        inventoryAction.Interract.performed += ctx => pickup.OnInterract();

        keyboard = Keyboard.current;
        gamepad = Gamepad.current;

    }
    
    private void Start()
    {
        _controls = GetComponent<PlayerInput>();
        _controls.onControlsChanged += OnControlsChanged;
        inputs_ui = GameManager.instance.GetComponentInChildren<InputSystemUIInputModule>();
        inputs_ui.enabled = false;

        // inventory actions inputs
        inventoryInputAction.Drop.performed += Inventory.instance.panelBackPack.OnDropItem;
        inventoryInputAction.Inspect.performed += Inventory.instance.panelBackPack.OnInspectItem;
        inventoryInputAction.Use.performed += Inventory.instance.panelBackPack.OnUseItem;
        inventoryInputAction.Equip.performed += Inventory.instance.panelBackPack.OnEquipItem;
        mouseInput.x = 0;
        mouseInput.y = 0;
        GameManager.instance.SetKeyBoard(true);
    }
    private void OnControlsChanged(PlayerInput obj)
    {
        if (obj.currentControlScheme == "gamepad")
        {
            if (currentControlDevice != ControlDeviceType.Gamepad)
            {
                currentControlDevice = ControlDeviceType.Gamepad;
                GameManager.instance.SetGamePad(true);

            }
        }
        else
        {
            if (currentControlDevice != ControlDeviceType.KeyboardAndMouse)
            {
                currentControlDevice = ControlDeviceType.KeyboardAndMouse;
                
                GameManager.instance.SetKeyBoard(true);

            }
        }
    }

    public void OnEnable()
    {
        inputs.Enable();
        inputs.HotbarActionInput.Enable();
        inventoryInputs.InventoryAction.Enable();
        uiInputs.Enable();
        flInputs.FlashLightInputController.Enable();
        gunInput.GunInputController.Enable();
        axeInput.AxeInputController.Enable();
        inventoryAction.pickup.Enable();
    }

    public void OnDisable()
    {
        inputs.Disable();
        inputs.HotbarActionInput.Disable();
        inventoryInputs.Disable();
        inventoryInputs.InventoryAction.Disable();
        uiInputs.Disable();
        flInputs.FlashLightInputController.Disable();
        gunInput.GunInputController.Disable();
        axeInput.AxeInputController.Disable();
        inventoryAction.pickup.Disable();
    }

    public void EnableInputInventory()
    {

        inventoryInputs.InventoryAction.ShowInventory.Enable();
    }
    public void DisableInventoryInput()
    {

        inventoryInputs.InventoryAction.ShowInventory.Disable();
    }


    public void EnableInputFL()
    {
       
        flInputs.FlashLightInputController.Enable();
        gunInput.GunInputController.Disable();
        axeInput.AxeInputController.Disable();
    }

    public void EnableInputGun()
    {
        
        gunInput.GunInputController.Enable();
        flInputs.FlashLightInputController.Disable();
        axeInput.AxeInputController.Disable();
    }

    public void EnableAxeInput()
    {
        gunInput.GunInputController.Disable();
        flInputs.FlashLightInputController.Disable();
        axeInput.AxeInputController.Enable();
    }

    public void DisableAllActions()
    {
        gunInput.GunInputController.Disable();
        flInputs.FlashLightInputController.Disable();
        axeInput.AxeInputController.Disable();
    }





    public void EnableInputsInventoryActions()
    {
      
        uiInputs.ActionFromInventory.Enable();
    }
    public void DisableInputsInventoryActions()
    {
        uiInputs.ActionFromInventory.Disable();
    }


    public void OnShowInventoryPressed(InputAction.CallbackContext context)
    {

        Inventory.instance.OpenCloseInventory();
        
        
    }


    public void EnableInputHotbar()
    {
        inventoryInputs.InventoryPanelNavigation.Disable();
        inputs.HotbarActionInput.Enable();
        DisableInputsInventoryActions();
    }

    public void DisbaleInputHotbar()
    {
        inventoryInputs.InventoryPanelNavigation.Enable();
        inputs.HotbarActionInput.Disable();
    }


    public void EnabledPickUpinput()
    {
        inventoryAction.pickup.Enable();
        
    }

    public void DisablePickupInput()
    {
        inventoryAction.pickup.Disable();
        
    }

    public void EnabledInterractpinput(bool activate)
    {
        if (activate)
        {
            inventoryAction.Interract.Enable();
        }
        else
        {
            inventoryAction.Interract.Disable();
        }
        

    }



    public void EnableEquipInput(bool activate)
    {
        if (activate)
        {
            inventoryAction.Equip.Enable();
        }
        else
        {
            inventoryAction.Equip.Disable();
        }
    }

    void Update()
    {



        //if (gamepad != null)
        //{
         
          
        //    GameManager.instance.SetGamePad(true);
        //    GameManager.instance.SetKeyBoard(false);
        //}
        //else
        //{
        //    GameManager.instance.SetKeyBoard(true);
        //    GameManager.instance.SetGamePad(false);
        //}

        player.receivedInput(horizontalInput);
        player.receivedmouseInput(mouseInput);
    }
}
