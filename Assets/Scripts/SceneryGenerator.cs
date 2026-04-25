// Filename:            SceneryGenerator.cs
// Author:              Professor Wolfe
// Creation Date:       1/25/2026
// Last Modified:       2/13/2026
// Current Version:     0.1.2
// Previous Version:    ForestGenerator.cs v0.1.1
// Project:             Racing Game
// Purpose:             A script use to generate the scenery using psuedorandom a generator and preset assets
// Version Changes:     Changed filename
//                      Allowed users to provide their own set of prefabs in the inspector
//                      Made a struct to bundle together given scenery data
//                      Allowed for objects to be rescaled at spawn
//                      Allows for objects to be angled based on terrain on creation
// Editor:              William Grant
// Notes:               Entirely the same as from Racing Game, so I will not touch the comment block.
using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public struct GeneratedScenery
{
    [Tooltip("All possible prefabs of the scenery type that can be spawned")]
    public GameObject[] PrefabOptions;
    [Tooltip("Number to spawn, on average, per 100 square meters")]
    public float SpawnWeight;
    [Tooltip("Should it always be vertical no matter the terrain angle?")]
    public bool AlwaysVertical;
    [Tooltip("The variation of how far the prefabs can lean from its default angle, in degrees")]
    public float LeanVariation;
    [Tooltip("The variation of how much the size can vary, in decimal percentage")]
    public float ScaleVariation;
}

public class SceneryGenerator : MonoBehaviour
{
    [Tooltip("The parent GameObject which contains all terrain tiles to be considered for spawning")]
    public GameObject terrainTilesParent;

    [Tooltip("All prefabs and associated information that can be spawned")]
    public GeneratedScenery[] SceneryGroups;

    // The psuedorandom number generator used for this script
    private System.Random numberGenerator;

    // Name:        Start
    // Description: Called immediately before the first Update frame
    // Parameters:  None
    // Returns:     None
    void Start()
    {
        numberGenerator = new System.Random();

        foreach (GeneratedScenery sceneryGroup in SceneryGroups)
        {
            foreach (Transform terrainTile in terrainTilesParent.GetComponentInChildren<Transform>())
            {
                PopulateTerrainTile(terrainTile, sceneryGroup);
            }
        }
    }

    // Name:        PopulateTerrainTile
    // Description: Fills a given terrain tile with scenery
    // Parameters:  Transform terrainTile - The terrain tile being filled
    //              GeneratedScenery sceneryGroup - The group of scenery data to use for instantiation
    // Returns:     None
    private void PopulateTerrainTile(Transform terrainTile, GeneratedScenery sceneryGroup)
    {
        Vector3 xzwCornerGlobal = new Vector3(terrainTile.transform.localScale.x * -5f, 0, terrainTile.transform.localScale.z * -5f) + terrainTile.transform.position;
        float xtileSize = terrainTile.transform.localScale.x * 10f;
        float ztileSize = terrainTile.transform.localScale.z * 10f;
        float tileArea = xtileSize * ztileSize;

        Vector3 terrainAngleAdjustment = Vector3.zero;
        if (!sceneryGroup.AlwaysVertical)
        {
            terrainAngleAdjustment = terrainTile.transform.up;
        }

        int remainingSpawns = (int)(sceneryGroup.SpawnWeight / 100 * tileArea);
        for (int objectsSpawned = 0; objectsSpawned < remainingSpawns; objectsSpawned++)
        {
            Vector3 spawnLocation = new Vector3((float)numberGenerator.NextDouble() * xtileSize, -0.1f, (float)numberGenerator.NextDouble() * ztileSize) + xzwCornerGlobal;
            RaycastHit raycastData;
            //!! Temporary (the * 400) fix provided here for handling angled terrain tiles; a more rigorous trigonometric solution should be provided
            if (Physics.Raycast(spawnLocation + Vector3.up * 400, Vector3.down, out raycastData))
            {
                if (raycastData.collider.transform.parent.gameObject != terrainTilesParent)
                {
                    continue;
                }
                //!! Other half of temporary fix
                else
                {
                    spawnLocation.y = raycastData.point.y;
                }
            }
            //!! Temporary fix for the temporary fix
            else
            {
                continue;
            }
            int prefabIndex = numberGenerator.Next(0, sceneryGroup.PrefabOptions.Length);
            GameObject generatedObject = Instantiate(
                sceneryGroup.PrefabOptions[prefabIndex],
                spawnLocation,
                Quaternion.Euler(terrainAngleAdjustment + new Vector3(
                    (float)numberGenerator.NextDouble() * sceneryGroup.LeanVariation,
                    (float)numberGenerator.NextDouble() * 360,
                    (float)numberGenerator.NextDouble() * sceneryGroup.LeanVariation
                )),
                this.transform
            );
            float objectScale = 1 + (float)numberGenerator.NextDouble() * sceneryGroup.ScaleVariation * 2 - sceneryGroup.ScaleVariation;
            generatedObject.transform.localScale = new Vector3(objectScale, objectScale, objectScale);
        }
    }
}
