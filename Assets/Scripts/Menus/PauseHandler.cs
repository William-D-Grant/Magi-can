/* Author:              William Grant
 * Filename:            PauseHandler.cs
 * Version:             0.1.0
 * Previous Version:    0.0.1
 * Last Modified:       April 5th 2026
 * Course:              CPSC 323-10
 * Professor Name:      Professor Wolfe
 * Project:             Project 1
 * Purpose:             This files handles opening and closing the pause menu, restarting the level, and exiting to the main menu.
 * Version Changes:     Created.
 * Notes:               I cannot for the life of me figure out why RegisterPulseAction breaks between scenes, but only sometimes.
 */
//# All menus based off BMo's tutorial on Youtube, 6 Minute PAUSE MENU Unity Tutorial https://www.youtube.com/watch?v=9dYDBomQpBQ taken on April 3rd, 2026 #//
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour
{
    // A check if the pause menu is up.
    public bool isPaused{ get; private set; }

    [SerializeField]
    private InputManager _inputManager;

    // Name:        Awake
    // Description: Called when the script instance is being loaded.
    // Parameters:  None
    // Returns:     None
    private void Awake()
    {
        _inputManager = InputManager.SingletonInstance;
        _inputManager.RegisterPulseAction("Pause", ToggleMenu);
        gameObject.SetActive(false);
        isPaused = false;
    }

    // Name:        ToggleMenu
    // Description: Opens the menu when Escape is pressed. Stops time and brings up the menu, and stops gameplay controls just in case.
    // Parameters:  None
    // Returns:     None
    public void ToggleMenu()
    {
        if (!isPaused)
        {
            gameObject.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            _inputManager.ToggleGameplayControls(false);
        }
    }

    // Name:        Resume
    // Description: Resumes the game when Resume is pressed. Resumes time and removes the menu, and re-enables gameplay controls just in case.
    // Parameters:  None
    // Returns:     None
    public void Resume()
    {
        if (isPaused)
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            _inputManager.ToggleGameplayControls(true);
        }
    }

    // Name:        Restart
    // Description: Restarts the level when Restart is presed. Resumes time, toggles controls back on, and reloads the scene.
    // Parameters:  None
    // Returns:     None
    public void Restart()
    {
        _inputManager.ToggleGameplayControls(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Name:        ExitLevel
    // Description: Exits back to the main menu when Exit is pressed. Resumes time and changes back to the main menu scene.
    // Parameters:  None
    // Returns:     None
    public void ExitLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    // Name:        OnDestroy
    // Description: Called when the script is destroyed.
    // Parameters:  None
    // Returns:     None
    private void OnDestroy()
    {
        //??!! Why doesn't this work when the scene is changed via other menus? !!??//
        _inputManager.DeregisterPulseAction("Pause", ToggleMenu);
    }


    
}
