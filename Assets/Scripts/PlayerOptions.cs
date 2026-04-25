/* Author:              Professor Wolfe
 * Editor:              William Grant
 * Filename:            PlayerOptions.cs
 * Version:             0.1.0
 * Previous Version:    0.0.1
 * Last Modified:       April 5th 2026
 * Course:              CPSC 323-10
 * Professor Name:      Professor Wolfe
 * Project:             Project 1
 * Purpose:             This file stores Music and SFX volume options, allowing for persistance across an instance.
 * Version Changes:     Created, edited from Professor Wolfe's original PlayerOptions.cs
 */

using UnityEngine;

public static class PlayerOptions
{

    // The volume of the game's music
    public static float MusicVolume { get; set; } = 0;

    // The volume of the game's sound effects
    public static float SFXVolume { get; set; } = 1;

}
