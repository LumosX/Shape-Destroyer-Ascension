using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileController : MonoBehaviour {
    public Building CurrentBuilding { get; private set; }

    private MeshRenderer renderer;
    public Color terrainColour;

    public int X;
    public int Z;


    private BoxCollider buildingCollider;

    private Transform buildingPivot;
    private Transform animatingPoint;
    private Transform oldAnimatingPoint;
    private const float PIVOT_STARTING_POINT_Z = 2.3f;
    private bool buildingAnimating = false;
    private float animSpeed = 1.5f;

    void Awake() {
        renderer = GetComponent<MeshRenderer>();
        terrainColour = renderer.material.color;

        buildingCollider = GetComponent<BoxCollider>();
        buildingCollider.enabled = false;

        buildingPivot = transform.GetChild(0);
    }

    public void SetPos(int x, int z) {
        X = x;
        Z = z;
    }

    public void SetBuilding(Building building) {
        
        //if (UIUpgradeBuilding.CurrentBuilding != null) Debug.Log("Setting building to " + building.Name);

        if (building == Buildings.Road) {
            // only roads get their terrain colour changed
            terrainColour = Buildings.RoadColour;
            renderer.material.color = Buildings.RoadColour;
        }
        if (building != Buildings.Road && building != Buildings.Generator) {
            // otherwise, turn the collider on and "build" the building
            buildingCollider.enabled = true;
            // Keep track of old prefab if one exists
            oldAnimatingPoint = animatingPoint;
            // set all other children to disabled
            var prefab = WorldBuilder.AllPossibleBuildings.First(x => x.name == building.Name);
            animatingPoint = Instantiate(prefab, buildingPivot.position, Quaternion.identity, transform).transform;
            animatingPoint.localEulerAngles = Vector3.zero;
            StartPivotAnim();
        }

        CurrentBuilding = building;
        WorldBuilder.RebuildNavmesh();
    }

    private void StartPivotAnim() {
        animatingPoint.localPosition = Vector3.forward * PIVOT_STARTING_POINT_Z;
        buildingAnimating = true;
    }

    private void Update() {
        // simple animation so the building rises up from the ground
        if (buildingAnimating) {
            animatingPoint.localPosition -= Vector3.forward * animSpeed * Time.deltaTime;
            if (animatingPoint.localPosition.z < 0) {
                animatingPoint.localPosition = Vector3.zero;
                buildingAnimating = false;
                if (oldAnimatingPoint != null) Destroy(oldAnimatingPoint.gameObject);
            }
        }
    }


    public void OnMouseEnter() {
        var player = GameController.PlayerInstance;
        var dist = player.InteractionDistance * player.InteractionDistance;

        // Only allow "interaction" if player is using proper "weapon" and tile is within range.
        if (!PlayerInstance.CanInteract) return;
        // and if it's day
        if (GameController.IsNight) return;
        var playerPos = player.transform.position;
        if (player.CurWeapon != 0) return; // 0 is the clipboard
        if ((playerPos - transform.position).sqrMagnitude > dist) return;
        // And if the player is not ON the tile (because that's easiest)
        var offset = WorldBuilder.WorldScaleMultiplier / 2;
        if ((playerPos.x > transform.position.x - offset && playerPos.x < transform.position.x + offset) &&
            (playerPos.z > transform.position.z - offset && playerPos.z < transform.position.z + offset)) return;

        renderer.material.color = Color.cyan;
        GameController.HighlightedTile = this;
    }

    public void OnMouseOver() {
        // Forcefully de-highlight the tile if the player steps on it
        if (!PlayerInstance.CanInteract) return;
        if (GameController.IsNight) return;
        var playerPos = GameController.PlayerInstance.transform.position;
        var offset = WorldBuilder.WorldScaleMultiplier / 2;
        if ((playerPos.x > transform.position.x - offset && playerPos.x < transform.position.x + offset) &&
            (playerPos.z > transform.position.z - offset && playerPos.z < transform.position.z + offset)) OnMouseExit();
    }

    public void OnMouseExit() {
        if (!PlayerInstance.CanInteract) return;
        if (GameController.IsNight) return;
        renderer.material.color = terrainColour;
        GameController.HighlightedTile = null;
    }
}
