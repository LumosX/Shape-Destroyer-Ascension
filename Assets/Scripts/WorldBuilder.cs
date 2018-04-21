using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldBuilder : MonoBehaviour {

    public GameObject tilePrefab;
    public GameObject backdropPrefab;
    public GameObject skydomePlanePrefab;

    public int worldSize = 11;
    public int WORLD_SCALE_MULTIPLIER = 10;
    public static Quaternion TILE_SPAWN_ROT = Quaternion.Euler(90, 0, 0);

	// Use this for initialization
	void Start () {
		BuildWorld();
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    void BuildWorld() {

        // Spawn underlying object to give tiles small edges in the fastest and hackiest way ever.
        var backdropOffset = new Vector3(WORLD_SCALE_MULTIPLIER / 2, 0.01f, WORLD_SCALE_MULTIPLIER / 2);
        var backdrop = Instantiate(backdropPrefab, transform.localPosition - backdropOffset, TILE_SPAWN_ROT, transform);
        backdrop.name = "Backdrop";
        backdrop.transform.localScale = Vector3.one * WORLD_SCALE_MULTIPLIER * worldSize;
        
        // Spawn tiles.
        var offset = Enumerable.Range(0, worldSize).Select(x => x - worldSize / 2).ToList(); // int round down
        var coords = from x in offset from z in offset select new Vector3(x, 0, z);
        foreach (var coord in coords) {
            var newObj = Instantiate(tilePrefab, transform.localPosition + coord * WORLD_SCALE_MULTIPLIER, TILE_SPAWN_ROT, transform);
            newObj.name = "Tile X " + coord.x + ", Z " + coord.z;
            newObj.transform.localScale = Vector3.one * WORLD_SCALE_MULTIPLIER - Vector3.one * 0.2f;
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
    }

    void SpawnSkyplane(Vector3 posOffset, Quaternion rotation, string name, Vector3 scale) {
        var targetPos = transform.localPosition + posOffset + new Vector3(-WORLD_SCALE_MULTIPLIER / 2, 0, -WORLD_SCALE_MULTIPLIER / 2);
        var t = Instantiate(skydomePlanePrefab, targetPos, rotation, transform);
        t.transform.localScale = scale;
        t.name = "Skyplane " + name;
    }

}
