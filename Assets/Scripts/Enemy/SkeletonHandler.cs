/* Author:              William Grant
 * Filename:            SkeletonHandler.cs
 * Version:             0.1.0
 * Previous Version:    0.0.1
 * Last Modified:       April 5th 2026
 * Course:              CPSC 323-10
 * Professor Name:      Professor Wolfe
 * Project:             Project 1
 * Purpose:             This file handles enemy skeleton movement, freezing, and dying.
 *                      Some functions are left for future expandability.
 * Version Changes:     Created.
 */

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

//# Structured off of a post by Dee_Va in Unity Forums https://discussions.unity.com/t/how-to-make-enemy-chase-player-basic-ai/45275 #//
public class SkeletonHandler : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Player's center")]
    private Transform playerTransform;
    [SerializeField]
    [Tooltip("Enemy character controller")]
    private CharacterController enemyController;
    [SerializeField]
    [Tooltip("Animator for enemy")]
    private Animator enemyAnimator;

    [Header("Skeleton AI")]
    [SerializeField]
    [Tooltip("Minimum distance to lock on")]
    private float minDist;
    [SerializeField]
    [Tooltip("Maximum distance to lock on")]
    private float maxDist;
    [SerializeField]
    [Tooltip("Movement speed of skeleton")]
    private float moveSpeed;

    // If the enemy has been hit by the ice beam
    public bool isFrozen;

    // If the enemy has run into a trap or hit the death plane.
    private bool hasDied;

    // Name:        Update
    // Description: Called every frame.
    // Parameters:  None
    // Returns:     None
    void Update()
    {
        if (!isFrozen && !hasDied)
        {
            enemyAnimator.SetBool("hasTarget", false);
            MoveTowardsPlayer();
        }
    }

    // Name:        MoveTowardsPlayer
    // Description: Called in update and checks if the player is in range. If they are, has different functions
    //              depending on the type. Warriors move slowly towards the player.
    // Parameters:  None
    // Returns:     None
    private void MoveTowardsPlayer()
    {
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        if (Vector3.Distance(transform.position, playerTransform.position) <= maxDist)
        {
            enemyAnimator.SetBool("hasTarget", true);
            if (tag == "Warrior")
            {
                enemyController.SimpleMove(transform.forward * moveSpeed);
            }
            if (tag == "Archer")
            {
                FireArrow();
            }
        }
    }

    // Name:        FireArrow
    // Description: Called if the Archer sees the player. Would have created an arrow object that moves to the player's position at the time of calling.
    // Parameters:  None
    // Returns:     None
    private void FireArrow()
    {
        // instantiate arrow prefab with horizontal velocity facing towards player
    }

    // Name:        FreezeEnemy
    // Description: Handles freezing the enemy by disabling movement, enabling the right animation, and activating the ice block collider.
    // Parameters:  None
    // Returns:     None
    public void FreezeEnemy()
    {
        isFrozen = true;
        transform.GetChild(0).gameObject.SetActive(true);
        enemyAnimator.SetBool("isFrozen", true);
    }

    // Name:        OnTriggerEnter
    // Description: Handles collision between a trigger and this object.
    // Parameters:  Collider collision - The opposing collider that this object interacted with.
    // Returns:     None
    private void OnTriggerEnter(Collider collision)
    {
        if (!isFrozen)
        {
            if (collision.transform.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerController>().KillPlayer();
            }
            if (collision.transform.tag == "Trap")
            {
                hasDied = true;
                isFrozen = true;
                enemyAnimator.SetBool("hasDied", true);
                this.GetComponent<CapsuleCollider>().isTrigger = false;
            }
        }
   
    }

}
