/* Author:              William Grant
 * Filename:            DeathHandler.cs
 * Version:             0.1.0
 * Previous Version:    0.0.1
 * Last Modified:       April 5th 2026
 * Course:              CPSC 323-10
 * Professor Name:      Professor Wolfe
 * Project:             Project 1
 * Purpose:             This file handles dying, pulling up the menu after death, and restarting or exiting after dying.
 * Version Changes:     Created.
 */
//# All menus based off BMo's tutorial on Youtube, 6 Minute PAUSE MENU Unity Tutorial https://www.youtube.com/watch?v=9dYDBomQpBQ taken on April 3rd, 2026 #//
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathHandler : MonoBehaviour
{
    // Name:        Awake
    // Description: Called when the script instance is being loaded.
    // Parameters:  None
    // Returns:     None
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    // Name:        DeathMenu
    // Description: Called when the player dies due to collision with enemy or trap. Brings up the death menu and pauses time.
    // Parameters:  None
    // Returns:     None
    public void DeathMenu()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    // Name:        Restart
    // Description: Called when restart is pressed. Resumes time and reloads the level to effectively restart.
    // Parameters:  None
    // Returns:     None
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Name:        ExitLevel
    // Description: Called when exit is pressed. Resumes time and goes back to main menu.
    // Parameters:  None
    // Returns:     None
    public void ExitLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
