using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCostReference : MonoBehaviour {

    public enum NewBuildingType {
        Road,
        Residential,
        Commercial,
        Industrial,
        Military,
    }

    public NewBuildingType buildingType;

    // dynamic binding for building costs
    void OnEnable () {
        var label = GetComponent<Text>();

        var buildingRef = Buildings.Road;
        switch (buildingType) {
            case NewBuildingType.Road:
                buildingRef = Buildings.Road;
                break;
            case NewBuildingType.Residential:
                buildingRef = Buildings.Residential1;
                break;
            case NewBuildingType.Commercial:
                buildingRef = Buildings.Commercial1;
                break;
            case NewBuildingType.Industrial:
                buildingRef = Buildings.Industrial1;
                break;
            case NewBuildingType.Military:
                buildingRef = Buildings.Military1;
                break;
        }

        // set label with real data so no manual updates are needed
        label.text = "Cost: " + buildingRef.GetBuildCost() + "\n" +
                     "Upkeep: " + buildingRef.GetUpkeep() + "\n" +
                     "Produces: " + buildingRef.Produces;

        // also update button if resources are sufficient
        var btn = GetComponentInParent<Button>();
        btn.interactable = GameController.Materials >= buildingRef.CostMats &&
                           GameController.Power > buildingRef.CostPower &&
                           GameController.Population >= buildingRef.CostPop;
    }
}
