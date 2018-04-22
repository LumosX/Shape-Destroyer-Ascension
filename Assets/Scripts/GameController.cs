using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static int Materials; // currency;
    public static int Power; // second currency / generator HP
    public static int Population; // free pop
    public static int Happiness; // population happiness; percentage modifier
    public static int AmmoMags; 
    public static float GeneratorResilience; // % chance of genny to not take damage

    
    public static TileController[,] TileMatrix;
    public static TileController HighlightedTile = null;

    // Each point of happiness changes materials income by 5%
    public int IncomeModifierPercent => Happiness * 5;


    public PlayerInstance playerInstanceReference;

    public static PlayerInstance PlayerInstance {
        get {
            if (inst.playerInstanceReference == null) {
                inst.playerInstanceReference = FindObjectOfType<PlayerInstance>();
            }
            return inst.playerInstanceReference;
        }
    }

    private static GameController inst;

    void Awake() {
        if (inst == null) inst = this;
    }

    public static void InitialiseGame() {
        Materials = 100;
        Power = 20;
        Population = 0;
        Happiness = 0;
        AmmoMags = 5;
        GeneratorResilience = 0.2f;
        HighlightedTile = null;
    }

    public static void OnNewBuildingCreated(Building type) {
        if (HighlightedTile == null) return;

        // subtract costs and get highlighted tile to build this building
        Materials -= type.CostMats;
        Power -= type.CostPower;
        Population -= type.CostPop;
        //Debug.Log("buildign yupe" + type.Name);
        HighlightedTile.SetBuilding(type);
    }

    // Buildings can only be built if adjacent to roads
    public static bool CanBuildOnHighlightedTile() {
        if (HighlightedTile == null) return false;
        if (HighlightedTile.CurrentBuilding != null) return false;

        var arrayOffset = WorldBuilder.WorldSize / 2;
        var x = HighlightedTile.X + arrayOffset;
        var z = HighlightedTile.Z + arrayOffset;
        var maxX = TileMatrix.GetLength(0);
        var maxZ = TileMatrix.GetLength(1);

        if (x < 0 || x > maxX) return false;
        if (z < 0 || z > maxZ) return false;


        // check adjacencies now; only NESW directions considered
        // LEFT
        if (x - 1 > 0 && TileMatrix[x - 1, z].CurrentBuilding == Buildings.Road) return true;
        // RIGHT
        if (x + 1 < maxX && TileMatrix[x + 1, z].CurrentBuilding == Buildings.Road) return true;
        // BACK
        if (z - 1 > 0 && TileMatrix[x, z - 1].CurrentBuilding == Buildings.Road) return true;
        // FORWARD
        if (z + 1 < maxZ && TileMatrix[x, z + 1].CurrentBuilding == Buildings.Road) return true;

        return false;
    }


    private static Tuple<Produce, int> CalculateTotalProduceAndUpkeep() {
        var result = new Produce();
        var upkeep = 0;
        foreach (var tile in TileMatrix) {
            if (tile != null && tile.CurrentBuilding != null) {
                var curProd = tile.CurrentBuilding.Produces;
                if (curProd != null) result += curProd;

                upkeep += tile.CurrentBuilding.Upkeep;
            }
        }
        return Tuple.Create(result, upkeep);
    }


    void OnGUI() {
        var dailyChanges = CalculateTotalProduceAndUpkeep();
        var totalProduce = dailyChanges.Item1;
        var totalUpkeep = dailyChanges.Item2;

        var labels = new string[] {
            "Power: " + Power,
            "Generator Resilience: " + (GeneratorResilience * 100).ToString("F0") + "%",
            "Free Population: " + Population,
            "Materials: " + Materials + " (" + (totalProduce.Materials - totalUpkeep) + ")",
            "Happiness: " + Happiness + " (" + totalProduce.Happiness + ")",
            "Ammo Mags: " + AmmoMags + " (" + totalProduce.Ammunition + ")",
            "Highlighted Tile: " + (HighlightedTile == null ? "none" : HighlightedTile.name),
            "CurrentBuilding: " + (HighlightedTile == null || HighlightedTile.CurrentBuilding == null ? "none" : HighlightedTile.CurrentBuilding.Name)
        };

        int startY = 10, offsetY = 20;
        foreach (var label in labels) {
            GUI.Label(new Rect(10, startY, 600, 20), label);
            startY += offsetY;
        }



    }


}
