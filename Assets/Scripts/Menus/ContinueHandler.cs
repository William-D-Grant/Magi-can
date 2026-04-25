/* Author:              William Grant
 * Filename:            ContinueHandler.cs
 * Version:             0.1.0
 * Previous Version:    0.0.1
 * Last Modified:       April 5th 2026
 * Course:              CPSC 323-10
 * Professor Name:      Professor Wolfe
 * Project:             Project 1
 * Purpose:             This file handles completing a level and the menu that pops up to continue or exit a level.
 * Version Changes:     Created.
 */

//# All menus based off BMo's tutorial on Youtube, 6 Minute PAUSE MENU Unity Tutorial https://www.youtube.com/watch?v=9dYDBomQpBQ taken on April 3rd, 2026 #//
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueHandler : MonoBehaviour
{
    // Name:        Awake
    // Description: Called when the script instance is being loaded.
    // Parameters:  None
    // Returns:     None
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    // Name:        NextLevel
    // Description: Called when the player touches the trigger for the next level. Brings up the menu and stops time.
    // Parameters:  None
    // Returns:     None
    public void NextLevel()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    // Name:        Continue
    // Description: Called when the player clicks continue. Resumes time and goes to the next level, which the object is tagged after.
    // Parameters:  None
    // Returns:     None
    public void Continue()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameObject.tag);
    }

    // Name:        ExitLevel
    // Description: Called when the player clicks exit. Goes back to the main menu and resumes time.
    // Parameters:  None
    // Returns:     None
    public void ExitLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
