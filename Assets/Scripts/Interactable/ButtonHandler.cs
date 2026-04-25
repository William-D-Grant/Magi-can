/* Author:              William Grant
 * Filename:            ButtonHandler.cs
 * Version:             0.1.0
 * Previous Version:    0.0.1
 * Last Modified:       April 5th 2026
 * Course:              CPSC 323-10
 * Professor Name:      Professor Wolfe
 * Project:             Project 1
 * Purpose:             This file handles pressing a button to activate sliding walls.
 * Version Changes:     Created.
 */
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    // Boolean for one-time button press
    private bool isToggled = false;

    [SerializeField]
    private InputManager _inputManager;

    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    [Tooltip("The wall the button is connected to.")]
    private GameObject connectedWall;

    [SerializeField]
    [Tooltip("Sliding wall sound effect")]
    private GameObject slidingSoundEffectPrefab;

    // Name:        Awake
    // Description: Called when the script instance is being loaded.
    // Parameters:  None
    // Returns:     None
    private void Awake()
    {
        _inputManager.RegisterPulseAction("Interact", UseButton);
    }

    // Name:        UseButton
    // Description: Called when interact is pressed to check if the button is is in front of the player.
    //              If it is, then proceeds to toggle the button and move the associated wall.
    // Parameters:  None
    // Returns:     None
    public void UseButton()
    {
        if (!isToggled)
        {
            RaycastHit buttonTarget;
            Physics.Raycast(_playerController.playerCamera.transform.position, _playerController.transform.forward, out buttonTarget, 100);
            if (Object.ReferenceEquals(buttonTarget.transform, this.transform.GetChild(0))) //# yacth_Mon on Unity Forums https://discussions.unity.com/t/how-to-compare-if-two-gameobjects-are-the-same/111277 #//
            {
                isToggled = true;
                connectedWall.transform.position = new Vector3(connectedWall.transform.position.x, connectedWall.transform.position.y - 4, connectedWall.transform.position.z);
                Instantiate(slidingSoundEffectPrefab, connectedWall.transform.position, connectedWall.transform.rotation);
                //AudioSource.PlayClipAtPoint(slidingSoundEffect, connectedWall.transform.position);
            }
        }
        
    }

    // Name:        OnDestroy
    // Description: Called when the object is being destroyed.
    // Parameters:  None
    // Returns:     None
    private void OnDestroy()
    {
        _inputManager.DeregisterPulseAction("Interact", UseButton);
    }
}
