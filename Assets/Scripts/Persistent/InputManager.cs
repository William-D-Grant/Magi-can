/* Author:              Professor Wolfe
 * Editor:              William Grant
 * Filename:            InputManager.cs
 * Version:             0.1.0
 * Previous Version:    0.0.1
 * Last Modified:       April 5th 2026
 * Course:              CPSC 323-10
 * Professor Name:      Professor Wolfe
 * Project:             Project 1
 * Purpose:             The manager script to handlke access to Unity's input system
 * Version Changes:     Redesigned for use in Magi-can, mainly renaming inputs and adding a few.
 */

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    // Boilerplate
    private static InputManager _singletonInstance;
    public static InputManager SingletonInstance
    {
        get
        {
            return _singletonInstance;
        }
    }

    [SerializeField]
    [Tooltip("The Action Map")]
    private InputActionAsset _actionMap;

    [SerializeField]
    [Tooltip("Name of the Action Map")]
    private string _actionMapName = "Player";

    private InputAction _walkAction;
    private InputAction _lookAction;
    private InputAction _jumpAction;
    private InputAction _iceAction;
    private InputAction _fireAction;
    private InputAction _interactAction;
    private InputAction _pauseMenuAction;

    public Vector2 WalkInput { get; private set; }

    public Vector3 LookInput { get; private set; }

    public bool JumpInput { get; private set; }

    public bool IceInput { get; private set; }

    public bool FireInput { get; private set; } 
    
    public bool InteractInput { get; private set; }

    public bool PauseInput { get; private set; }

    [SerializeField]
    [Tooltip("Whether or not gameplay-related controls should be enabled")]
    private bool _shouldEnableGameplay = true;

    [SerializeField]
    [Tooltip("Whether or not UI-related controls should be enabled")]
    private bool _shouldEnableUI = true;

    // Name:        Awake
    // Description: Called immediately upon the object loading in
    // Parameters:  None
    // Returns:     None
    void Awake()
    {
        // Boilerplate
        if (_singletonInstance == null)
        {
            _singletonInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _walkAction = _actionMap.FindActionMap(_actionMapName).FindAction("Walk");
        _lookAction = _actionMap.FindActionMap(_actionMapName).FindAction("Look");
        _jumpAction = _actionMap.FindActionMap(_actionMapName).FindAction("Jump");
        _iceAction = _actionMap.FindActionMap(_actionMapName).FindAction("Ice");
        _fireAction = _actionMap.FindActionMap(_actionMapName).FindAction("Fire");
        _interactAction = _actionMap.FindActionMap(_actionMapName).FindAction("Interact");
        _pauseMenuAction = _actionMap.FindActionMap(_actionMapName).FindAction("Pause");

        RegisterHeldActions();
    }

    // Name:        RegisterHeldActions
    // Description: Registers all input actions which work on-hold to associated variables
    // Parameters:  None
    // Returns:     None
    private void RegisterHeldActions()
    {
        _walkAction.performed += context => WalkInput = context.ReadValue<Vector2>();
        _walkAction.canceled += context => WalkInput = Vector2.zero;

        _lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        _lookAction.canceled += context => LookInput = Vector2.zero;

        _iceAction.performed += context => IceInput = true;
        _iceAction.canceled += context => IceInput = false;

        _jumpAction.performed += context => JumpInput = true;
        _jumpAction.canceled += context => JumpInput = false;
    }

    // Name:        ToggleGameplayControls
    // Description: Enables/Disables any gameplay-related input actions
    // Parameters:  bool shouldEnable - Whether the Gameplay controls should be enabled or not
    // Returns:     None
    public void ToggleGameplayControls(bool shouldEnable)
    {
        // Keeps track of the desired state of the gameplay action's activation
        _shouldEnableGameplay = shouldEnable;
        if (_shouldEnableGameplay)
        {
            _walkAction.Enable();
            _lookAction.Enable();
            _iceAction.Enable();
            _fireAction.Enable();
            _interactAction.Enable();
            _jumpAction.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            _walkAction.Disable();
            _lookAction.Disable();
            _iceAction.Disable();
            _fireAction.Disable();
            _interactAction.Disable();
            _jumpAction.Disable();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    // Name:        ToggleUIControls
    // Description: Enables/Disables UI input actions
    // Parameters:  bool shouldEnable - Whether the UI controils should be enabled or not
    // Returns:     None
    public void ToggleUIControls(bool shouldEnable)
    {
        // Keeps track of the desired state of the UI action's activation
        _shouldEnableUI = shouldEnable;
        if (_shouldEnableUI)
        {
            _pauseMenuAction.Enable();
        }
        else
        {
            _pauseMenuAction.Disable();
        }
    }

    // Name:        RegisterPulseAction
    // Description: Makes a signal caused by a given action activate a given method
    // Parameters:  string actionName - The name of the action associated with the desired signal
    //              Action desiredEvent - The method to be activated by the signal
    // Returns:     None
    public void RegisterPulseAction(string actionName, Action desiredEvent)
    {
        Debug.Log("Registering " + actionName + " to " + desiredEvent.Method.Name);
        _actionMap.FindActionMap(_actionMapName).FindAction(actionName).started += context => desiredEvent();
    }

    // Name:        DeregisterPulseAction
    // Description: The counterpart to RegisterPulseAction, called to prevent dangling threads
    // Parameters:  string actionName - The name of the action associated with the desired signal
    //              Action desiredEvent - The method to no longer be activated by the signal
    // Returns:     None
    public void DeregisterPulseAction(string actionName, Action desiredEvent)
    {
        Debug.Log("Deregistering " + actionName + " to " + desiredEvent.Method.Name);
        _actionMap.FindActionMap(_actionMapName).FindAction(actionName).started -= context => desiredEvent();
    }

    // Name:        OnEnable
    // Description: Runs whenever the GameObject associated with the script is enabled
    // Parameters:  None
    // Returns:     None
    private void OnEnable()
    {
        // Only re-enables UI actions if they are meant to be enabled
        if (_shouldEnableUI)
        {
            _pauseMenuAction.Enable();
        }
        // Only re-enables gameplay actions if they are meant to be enabled
        if (_shouldEnableGameplay)
        {
            _walkAction.Enable();
            _lookAction.Enable();
            _iceAction.Enable();
            _fireAction.Enable();
            _interactAction.Enable();
            _jumpAction.Enable();
        }
    }
}
