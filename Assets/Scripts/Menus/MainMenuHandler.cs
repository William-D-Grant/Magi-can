/* Author:              William Grant
 * Filename:            MainMenuHandler.cs
 * Version:             0.1.0
 * Previous Version:    0.0.1
 * Last Modified:       April 5th 2026
 * Course:              CPSC 323-10
 * Professor Name:      Professor Wolfe
 * Project:             Project 1
 * Purpose:             This file handles the Main Menu start button and exit game.
 * Version Changes:     Created.
 */

using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuHandler : MonoBehaviour
{
    // Name:        StartGame
    // Description: Called when Start is pressed. Just loads Level 1.
    // Parameters:  None
    // Returns:     None
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    // Name:        ExitGame
    // Description: Called when Exit is pressed. Quits the game.
    // Parameters:  None
    // Returns:     None
    public void ExitGame()
    {
        Application.Quit();
    }
}
