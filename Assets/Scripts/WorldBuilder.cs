using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WorldBuilder : MonoBehaviour {

    public GameObject tilePrefab;
    public GameObject backdropPrefab;
    public GameObject skydomePlanePrefab;
    public GameObject skydomeGeneratorPrefab;

    public int worldSize = 11;
    public int WORLD_SCALE_MULTIPLIER = 20;
    public static Quaternion TILE_SPAWN_ROT = Quaternion.Euler(90, 0, 0);

    public static int WorldScaleMultiplier = 20;
    public static int WorldSize = 20;

    private static NavMeshSurface navMesh;

    // Enemy spawn data. Should put them outside of "dome", but not out of the navmesh.
    private static float MIN_SPAWN_X, MIN_SPAWN_Z, MAX_SPAWN_X, MAX_SPAWN_Z;
        
	void Start () {
	    navMesh = GetComponentInChildren<NavMeshSurface>();

		BuildWorld();
        GameController.InitialiseGame();
	}


    void BuildWorld() {
        // I'm dumb
        WorldScaleMultiplier = WORLD_SCALE_MULTIPLIER;
        WorldSize = worldSize;

        // Spawn underlying object to give tiles small edges in the fastest and hackiest way ever.
        var backdropOffset = new Vector3(WORLD_SCALE_MULTIPLIER / 2, 0.01f, WORLD_SCALE_MULTIPLIER / 2);
        var backdrop = Instantiate(backdropPrefab, transform.localPosition - backdropOffset, TILE_SPAWN_ROT, transform);
        backdrop.name = "Backdrop";
        backdrop.transform.localScale = Vector3.one * WORLD_SCALE_MULTIPLIER * worldSize;
        
        // Init controller matrix
        GameController.TileMatrix = new TileController[worldSize,worldSize];
        
        // Spawn tiles.
        var posOffset = worldSize / 2; // int round down
        var offset = Enumerable.Range(0, worldSize).Select(x => x - posOffset).ToList();
        var coords = from x in offset from z in offset select new Vector3(x, 0, z);
        foreach (var coord in coords) {
            var newObj = Instantiate(tilePrefab, transform.localPosition + coord * WORLD_SCALE_MULTIPLIER, TILE_SPAWN_ROT, transform);
            newObj.name = "Tile X " + coord.x + ", Z " + coord.z;
            newObj.transform.localScale = Vector3.one * WORLD_SCALE_MULTIPLIER - Vector3.one * 0.2f;

            var tc = newObj.GetComponent<TileController>();
            tc.SetPos((int)coord.x, (int)coord.z);

            // Set special buildings from the get go
            if (coord.x == 0 && coord.z == 0) {
                tc.SetBuilding(Buildings.Generator);
                // spawn skydome generator
                var genny = Instantiate(skydomeGeneratorPrefab, Vector3.zero, Quaternion.identity);
                genny.transform.SetParent(newObj.transform);
            }
            else if (coord.x <= 1 && coord.x >= -1 && coord.z <= 1 && coord.z >= -1) {
                tc.SetBuilding(Buildings.Road);
                

            }


            GameController.TileMatrix[(int)coord.x + posOffset, (int)coord.z + posOffset] = tc;


        }

        // Build Skydome
        var skydomeYPos = 50;
        var skydomeUpScale = Vector3.one * WORLD_SCALE_MULTIPLIER * worldSize;
        var skydomeSideScale = skydomeUpScale;
        skydomeSideScale.y = skydomeYPos;

        var vertOffset = Vector3.up * skydomeYPos / 2;
        var skydomePosMult = WORLD_SCALE_MULTIPLIER * worldSize / 2;
        SpawnSkyplane(Vector3.forward * skydomePosMult + vertOffset, Quaternion.identity, "Forward", skydomeSideScale);
        SpawnSkyplane(Vector3.back * skydomePosMult + vertOffset, Quaternion.identity, "Back", skydomeSideScale);
        SpawnSkyplane(Vector3.left * skydomePosMult + vertOffset, Quaternion.Euler(0, 90, 0), "Left", skydomeSideScale);
        SpawnSkyplane(Vector3.right * skydomePosMult + vertOffset, Quaternion.Euler(0, 90, 0), "Right", skydomeSideScale);
        SpawnSkyplane(Vector3.up * skydomeYPos, TILE_SPAWN_ROT, "Up", skydomeUpScale);

        // Make navmesh
        navMesh.size = new Vector3(WORLD_SCALE_MULTIPLIER * worldSize * 1.5f, 12, WORLD_SCALE_MULTIPLIER * worldSize * 1.5f);
        navMesh.center = new Vector3(0, 5, 0);
        navMesh.BuildNavMesh();

        // set spawn limits
        MIN_SPAWN_X = MIN_SPAWN_Z = WORLD_SCALE_MULTIPLIER * worldSize / 2 + 10;
        MAX_SPAWN_X = MAX_SPAWN_Z = (WORLD_SCALE_MULTIPLIER * worldSize * 1.5f / 2) - 5;
        Debug.Log(MIN_SPAWN_X +"," +MAX_SPAWN_X);

    }

    void SpawnSkyplane(Vector3 posOffset, Quaternion rotation, string name, Vector3 scale) {
        var targetPos = transform.localPosition + posOffset + new Vector3(-WORLD_SCALE_MULTIPLIER / 2, 0, -WORLD_SCALE_MULTIPLIER / 2);
        var t = Instantiate(skydomePlanePrefab, targetPos, rotation, transform);
        t.transform.localScale = scale;
        t.name = "Skyplane " + name;
    }

    public static void RebuildNavmesh() {
        if (navMesh.navMeshData != null) navMesh.UpdateNavMesh(navMesh.navMeshData);
    }
    
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public static Vector3 GetRandomEnemySpawnPoint() {
        // also makes them negative sometimes
        var pos1 = (UnityEngine.Random.value * 100) % 2 == 0;
        var pos2 = (UnityEngine.Random.value * 100) % 2 == 0;
        var x = UnityEngine.Random.Range(MIN_SPAWN_X, MAX_SPAWN_X) * (pos1 ? 1 : -1);
        var z = UnityEngine.Random.Range(MIN_SPAWN_Z, MAX_SPAWN_Z) * (pos1 ? 1 : -1);

        return new Vector3(x, 0.1f, z);
    }

}
