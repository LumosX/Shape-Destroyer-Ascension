using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeBuilding : MonoBehaviour {

    public static Building CurrentBuilding;

    public Text UpgradeTitle;
    public Text Description;
    public Text CostField;

    public Button UpgradeButton;

    void OnEnable() {
        if (CurrentBuilding?.UpgradesTo == null) {
            throw new Exception("Can't open the upgrades screen without a building set.");
        }

        var targetBuilding = CurrentBuilding.UpgradesTo;

        UpgradeTitle.text = ("Upgrade to " + targetBuilding.Name).ToUpper();
        Description.text = "Upgrading a building tends to be more cost- and power-efficient than\n" + 
                           "building more buildings of the same type.";

        Func<int, int, string> helper = (target, current) => {
            var diff = target - current;
            return " (" + (diff >= 0 ? "+" : "") + diff + ")";
        };

        var upkeepDiff = helper(targetBuilding.Upkeep, CurrentBuilding.Upkeep);
        var happinessDiff = helper(targetBuilding.Produces.Happiness, CurrentBuilding.Produces.Happiness);
        var matsDiff = helper(targetBuilding.Produces.Materials, CurrentBuilding.Produces.Materials);
        var ammoDiff = helper(targetBuilding.Produces.Ammunition, CurrentBuilding.Produces.Ammunition);

        var prod = targetBuilding.Produces;
        var produceDiffStr = "";
        if (prod.Happiness != 0) produceDiffStr += "Hap: " + prod.Happiness + happinessDiff + ", ";
        if (prod.Materials != 0) produceDiffStr += "Mats: " + prod.Materials + matsDiff + ", ";
        if (prod.Ammunition != 0) produceDiffStr += "Ammo: " + prod.Ammunition + ammoDiff + ", ";
        produceDiffStr = produceDiffStr.Substring(0, produceDiffStr.Length - 2);


        CostField.text = "Cost: " + targetBuilding.GetBuildCost() + "\n" +
                         "Upkeep: " + targetBuilding.GetUpkeep() + upkeepDiff + "\n" +
                         "Produces: " + produceDiffStr;

        UpgradeButton.onClick.RemoveAllListeners();
        UpgradeButton.onClick.AddListener(() => {
            GameController.OnNewBuildingCreated(targetBuilding);

            // Close self.
            GameController.PlayerInstance.CloseMenu();
            gameObject.SetActive(false);
        });
    }

    void Update() {
        // check resources
        var targetBuilding = CurrentBuilding.UpgradesTo;
        UpgradeButton.interactable = GameController.Materials >= targetBuilding.CostMats &&
                                     GameController.Power > targetBuilding.CostPower &&
                                     GameController.Population >= targetBuilding.CostPop;
    }

}
