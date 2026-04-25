/* Author:              William Grant
 * Filename:            IceBeamHandler.cs
 * Version:             0.1.0
 * Previous Version:    0.0.1
 * Last Modified:       April 5th 2026
 * Course:              CPSC 323-10
 * Professor Name:      Professor Wolfe
 * Project:             Project 1
 * Purpose:             This file handles firing the ice beam and most interactions between the ice beam and objects. It also handles
 *                      the rendering of the ice beam.
 * Version Changes:     Created.
 */

using UnityEngine;
using UnityEngine.Rendering;

public class IceBeamHandler : MonoBehaviour
{
    // To store the ice beam target for global use.
    private RaycastHit iceBeamTarget;

    [SerializeField]
    [Tooltip("Line Renderer for Ice Beam")]
    private LineRenderer line;

    [SerializeField]
    [Tooltip("Position of wand to change where it is pointing")]
    private Transform wandPosition;

    [SerializeField]
    [Tooltip("Position of wand head to fire beam from")]
    private Transform wandHeadPosition;

    [SerializeField]
    [Tooltip("Freezing Audio Clips")]
    private GameObject[] audioClipPrefabs;

    // Name:        Awake
    // Description: Called when the script instance is being loaded.
    // Parameters:  None
    // Returns:     None
    //# LineRenderer usage inspired by Tehnique from https://discussions.unity.com/t/making-a-raycast-visible/135103 taken on April 3rd, 2026 #//
    //# Mostly gathered from Unity Documentation https://docs.unity3d.com/6000.3/Documentation/ScriptReference/LineRenderer.html taken on April 3rd, 2026 #//
    private void Awake()
    {
        line.startWidth = 0.2f;
        line.endWidth = 0.2f;
        line.positionCount = 2;
    }

    // Name:        IceBeamCast
    // Description: Called upon holding left click. Raycasts from the camera to figure out where the beam should
    //              land, and preforms various checks to figure out what to follow up with.
    // Parameters:  Camera playerCamera - The camera from which the player sees through and will be raycast from.
    // Returns:     None
    public void IceBeamCast(Camera playerCamera)
    {
        Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out iceBeamTarget, 100);
        if (iceBeamTarget.collider.tag == "Ice")
        {
            FreezeWater();
        }
        if (iceBeamTarget.collider.tag == "Warrior" || iceBeamTarget.collider.tag == "Archer")
        {
            // Frozen processes handled in SkeletonHandler. Might be a cleaner way to do this.
            if (!iceBeamTarget.collider.GetComponent<SkeletonHandler>().isFrozen)
            {
                Instantiate(audioClipPrefabs[2], iceBeamTarget.point, iceBeamTarget.transform.rotation);
                //AudioSource.PlayClipAtPoint(audioClips[2], iceBeamTarget.point);
                iceBeamTarget.collider.GetComponent<SkeletonHandler>().FreezeEnemy();
            }
        }
        if (iceBeamTarget.collider.tag == "Trap")
        {
            FreezeTrap();
        }
    }

    // Name:        FreezeWater()
    // Description: Called when the ice beam touches an Ice object. Effectively makes the ice object visible and tangible, and plays a sound.
    // Parameters:  None
    // Returns:     None
    private void FreezeWater()
    {
        // Might be a cleaner way to have the ice render without grabbing each one's specific mesh renderer.
        MeshRenderer iceRenderer = iceBeamTarget.collider.GetComponent<MeshRenderer>();
        if (iceRenderer != null && iceBeamTarget.collider.isTrigger == true)
        {
            // Might be better to just have colliders off by default and enable them, rather than using trigger as a no-collide.
            iceBeamTarget.collider.isTrigger = false;
            Instantiate(audioClipPrefabs[0], iceBeamTarget.point, iceBeamTarget.transform.rotation);
            //AudioSource.PlayClipAtPoint(audioClips[0], iceBeamTarget.point);
            iceRenderer.enabled = true;
        }
    }

    // Name:        FreezeTrap()
    // Description: Called when the ice beam touches a Trap object. Effectively enables the ice child that covers the beam, and plays a sound.
    // Parameters:  None
    // Returns:     None
    private void FreezeTrap()
    {
        if (iceBeamTarget.collider.isTrigger == true)
        {
            Instantiate(audioClipPrefabs[1], iceBeamTarget.point, iceBeamTarget.transform.rotation);
            //AudioSource.PlayClipAtPoint(audioClips[1], iceBeamTarget.point);
            iceBeamTarget.collider.isTrigger = false;
            // Surely there's a better way to get a child than through specific index? I can't find it.
            iceBeamTarget.transform.GetChild(0).gameObject.SetActive(true);
        }
        
    }

    // Name:        BeamRender
    // Description: Called during the process of IceBeamCast to draw a line for the ice beam visual, and tilts the wand towards the landing spot.
    // Parameters:  Camera playerCamera - The camera from which the player sees through and will be raycast from.
    // Returns:     None
    //# LineRenderer usage inspired by Tehnique from https://discussions.unity.com/t/making-a-raycast-visible/135103 taken on April 3rd, 2026 #//
    //# Mostly gathered from Unity Documentation https://docs.unity3d.com/6000.3/Documentation/ScriptReference/LineRenderer.html taken on April 3rd, 2026 #//
    public void BeamRender(Camera playerCamera)
    {
        wandPosition.LookAt(iceBeamTarget.point);
        line.enabled = true;
        line.SetPosition(0, wandHeadPosition.position);
        line.SetPosition(1, iceBeamTarget.point);
    }

    // Name:        BeamStop
    // Description: Reverts everything that BeamRender does back to its normal state, and stops drawing the line.
    // Parameters:  Camera playerCamera - The camera from which the player sees through and will be raycast from.
    // Returns:     None
    //# LineRenderer usage inspired by Tehnique from https://discussions.unity.com/t/making-a-raycast-visible/135103 taken on April 3rd, 2026 #//
    //# Mostly gathered from Unity Documentation https://docs.unity3d.com/6000.3/Documentation/ScriptReference/LineRenderer.html taken on April 3rd, 2026 #//
    public void BeamStop(Camera playerCamera)
    {
        // I wanted the wand to point up when not in use, but Unity did not agree with me. Requires more review.
        wandPosition.rotation = playerCamera.transform.rotation;
        line.enabled = false;
    }
}
