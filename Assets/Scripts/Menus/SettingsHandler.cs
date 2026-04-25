/* Author:              Professor Wolfe
 * Editor:              William Grant
 * Filename:            SettingsHandler.cs
 * Version:             0.1.0
 * Previous Version:    0.0.1
 * Last Modified:       April 5th 2026
 * Course:              CPSC 323-10
 * Professor Name:      Professor Wolfe
 * Project:             Project 1
 * Purpose:             This file handles all music and sfx changing and editing, mainly done in the main menu.
 * Version Changes:     Created, frankenstiened between SoundSettings and AudioManager.
 */
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.InputSystem.DefaultInputActions;

public class SettingsHandler : MonoBehaviour
{
    [Tooltip("The slider for SFX volume")]
    public Slider _sfxVolumeSlider;

    [Tooltip("The slider for music volume")]
    public Slider _musicVolumeSlider;

    [SerializeField]
    [Tooltip("The audio group for all music sources")]
    private AudioMixerGroup _musicMixerGroup;

    [SerializeField]
    [Tooltip("The audio group for all sound effect sources")]
    private AudioMixerGroup _sfxMixerGroup;

    [SerializeField]
    [Tooltip("The decibel modifier at maximum volume")]
    private float _maximumDecibel = 0;

    [SerializeField]
    [Tooltip("The decibel modifier at minimum volume")]
    private float _minimumDecibel = -80;

    // Name:        Start
    // Description: Called immediately before the object's first Update frame
    // Parameters:  None
    // Returns:     None
    void Start()
    {
        _sfxVolumeSlider.value = PlayerOptions.SFXVolume;
        _musicVolumeSlider.value = PlayerOptions.MusicVolume;
    }

    // Name:        UpdateSFXVolume
    // Description: Updates the SFX volume modifier to the slider's value.
    // Parameters:  None
    // Returns:     None
    public void UpdateSFXVolume()
    {
        PlayerOptions.SFXVolume = _sfxVolumeSlider.value;
        _musicMixerGroup.audioMixer.SetFloat("SFXExposed", ModifierToDecibels(_sfxVolumeSlider.value));
    }

    // Name:        UpdateMusicVolume
    // Description: Updates the music volume modifier to the slider's value.
    // Parameters:  None
    // Returns:     None
    public void UpdateMusicVolume()
    {
        PlayerOptions.MusicVolume = _musicVolumeSlider.value;
        _musicMixerGroup.audioMixer.SetFloat("MusicExposed", ModifierToDecibels(_musicVolumeSlider.value));
    }

    // Name:        ModifierToDecibels
    // Description: Converts a modifier to decibels
    // Parameters:  float providedModifier - A modifier, typically from 0 - 1
    // Returns:     float - The associated decibel value
    private float ModifierToDecibels(float providedModifier)
    {
        return (_minimumDecibel + (_maximumDecibel - _minimumDecibel) * providedModifier);
    }
}
