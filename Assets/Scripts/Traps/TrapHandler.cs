/* Author:              William Grant
 * Filename:            TrapHandler.cs
 * Version:             0.1.0
 * Previous Version:    0.0.1
 * Last Modified:       April 5th 2026
 * Course:              CPSC 323-10
 * Professor Name:      Professor Wolfe
 * Project:             Project 1
 * Purpose:             This file explicitly handles the interaction between player and trap.
 *                      While the only function here could be done in PlayerController.cs,
 *                      this was created for expandability.
 * Version Changes:     Created.
 */
using UnityEngine;

public class TrapHandler : MonoBehaviour
{
    // Name:        OnTriggerEnter
    // Description: Called when a collider markes as isTrigger collides with this object.
    // Parameters:  Collider collision - The opposing collider that this object interacted with.
    // Returns:     None
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().KillPlayer();
        }
    }
}
