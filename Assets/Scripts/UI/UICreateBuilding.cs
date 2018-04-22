using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICreateBuilding : MonoBehaviour {

    public void OnCreateBuildingClicked(RectTransform caller) {
        // Find building reference (this is so awfully written)
        var callerName = caller.name.Substring(3);
        var buildingRef = Buildings.Road;
        //Debug.Log("caller name = " + callerName);
        if (callerName == "Road")
            buildingRef = Buildings.Road;
        else if (callerName == "Residential") {
            buildingRef = Buildings.Residential1;
        }
        else if (callerName == "Commercial") {
            buildingRef = Buildings.Commercial1;
        }
        else if (callerName == "Industrial") {
            buildingRef = Buildings.Industrial1;
        }
        else if (callerName == "Military") {
            buildingRef = Buildings.Military1;
        }

        GameController.OnNewBuildingCreated(buildingRef);

        // Close self.
        GameController.PlayerInstance.CloseMenu();
        gameObject.SetActive(false);
    }
}
