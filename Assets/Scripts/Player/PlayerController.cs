/* Author:              William Grant
 * Filename:            PlayerController.cs
 * Version:             0.1.0
 * Previous Version:    0.0.1
 * Last Modified:       April 5th 2026
 * Course:              CPSC 323-10
 * Professor Name:      Professor Wolfe
 * Project:             Project 1
 * Purpose:             Controls almost anything related to the player, including movement, abilities, looking,
 *                      menu screens, control enabling and disabling, and death.
 * Version Changes:     Created.
 */

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    [Header("UI Stuff")]
    [SerializeField]
    [Tooltip("Collected fireball powerup")]
    public bool hasFireball = false;
    [SerializeField]
    [Tooltip("Collected ice beam powerup")]
    public bool hasIceBeam = false;
    [SerializeField]
    [Tooltip("Collected a key")]
    public bool hasKey = false;

    [Header("Movement Modifiers")]
    [SerializeField]
    [Tooltip("Speed modifier")]
    private float playerSpeed = 5f;
    [SerializeField]
    [Tooltip("Jump modifier")]
    private float jumpHeight = 20f;
    [SerializeField]
    [Tooltip("Gravity modifier")]
    private float gravityMod = -9.81f;

    [Header("Camera Modifiers")]
    [SerializeField]
    [Tooltip("Mouse sensitivity")]
    private float mouseSens = 2.0f;
    [SerializeField]
    [Tooltip("Look limits")]
    private float upDownRange = 80.0f;

    [Header("Ability Handlers")]
    [SerializeField]
    [Tooltip("Ice Beam")]
    private IceBeamHandler _iceBeamHandler;
    [SerializeField]
    [Tooltip("Fireball")]
    private float _fireballHandler;

    [SerializeField]
    [Tooltip("Death Menu Handler")]
    private DeathHandler _deathScreen;
    [SerializeField]
    [Tooltip("Continue Menu Handler")]
    private ContinueHandler _continueScreen;

    // Used for vertical looking and clamping
    private float verticalRotation;

    // Camera used by player
    public Camera playerCamera;


    [Header("Important Connections")]
    [SerializeField]
    [Tooltip("Input Manager")]
    public InputManager _inputManager;

    // Character controller for movement and collision
    public CharacterController playerController;
    // Vector relating to speed in x, y, z directions
    private Vector3 playerVelocity;
    // Is player grounded?
    private bool groundedPlayer;

    // Name:        Awake
    // Description: Called when the script instance is being loaded.
    // Parameters:  None
    // Returns:     None
    private void Awake()
    {
        // Just make sure the player can move when they are first put in a level
        _inputManager.ToggleGameplayControls(true);
        _inputManager.ToggleUIControls(true);
    }

    // Name:        Update
    // Description: Update is called once per frame.
    // Parameters:  None
    // Returns:     None
    void Update()
    {
        HandleMovement();
        HandleLooking();

        if (_inputManager.IceInput)
        {
            _iceBeamHandler.IceBeamCast(playerCamera);
            _iceBeamHandler.BeamRender(playerCamera);
        }
        else
        {
            _iceBeamHandler.BeamStop(playerCamera);
        }
    }

    // Name:        HandleMovement
    // Description: Handles all movement, vertical and horizontal, using input manager's inputs and a Player Controller.
    // Parameters:  None
    // Returns:     None
    //# Movement and Looking heavily inspired by SpeedTutor on Youtube https://www.youtube.com/watch?v=i_VFMOTfvmA taken on April 2nd, 2026 #//
    private void HandleMovement()
    {
        groundedPlayer = playerController.isGrounded;

        if (groundedPlayer)
        {
            // Keep the player stable on the ground
            if (playerVelocity.y < -2f)
                playerVelocity.y = -2f;
        }

        Vector3 move = new Vector3(_inputManager.WalkInput.x, 0, _inputManager.WalkInput.y);

        move = transform.rotation * move;

        if (groundedPlayer && _inputManager.JumpInput)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityMod);
        }

        // Gravity
        playerVelocity.y += gravityMod * Time.deltaTime;

        // Move
        Vector3 finalMove = move * playerSpeed + Vector3.up * playerVelocity.y;
        playerController.Move(finalMove * Time.deltaTime);
    }

    // Name:        HandleLooking
    // Description: Handles all Looking, vertical and horizontal, using input manager's inputs.
    // Parameters:  None
    // Returns:     None
    //# Movement and Looking heavily inspired by SpeedTutor on Youtube https://www.youtube.com/watch?v=i_VFMOTfvmA taken on April 2nd, 2026 #//
    private void HandleLooking()
    {
        float mouseXRotation = _inputManager.LookInput.x * mouseSens;
        transform.Rotate(0, mouseXRotation, 0);

        verticalRotation -= _inputManager.LookInput.y * mouseSens;
        
        // Make it so they can't look too far up or down
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    // Name:        KillPlayer()
    // Description: A function that can be called to bring up the Death screen, and toggles controls here.
    // Parameters:  None
    // Returns:     None
    public void KillPlayer()
    {
        _inputManager.ToggleGameplayControls(false);
        _inputManager.ToggleUIControls(false);
        _deathScreen.DeathMenu();
    }

    // Name:        OnTriggerEnter
    // Description: Called when a collider markes as isTrigger collides with this object.
    // Parameters:  Collider collision - The opposing collider that this object interacted with.
    // Returns:     None
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Level 2" || collision.gameObject.tag == "Main Menu")
        {
            _inputManager.ToggleGameplayControls(false);
            _inputManager.ToggleUIControls(false);
            _continueScreen.NextLevel();
        }
    }
}
