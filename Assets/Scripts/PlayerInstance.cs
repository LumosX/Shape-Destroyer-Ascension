using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Input = UnityEngine.Input;

public class PlayerInstance : MonoBehaviour {

    public static bool CanInteract = true;

    public int CurWeapon = 0;
    public float InteractionDistance = 40f;

    public RectTransform pnlCreateNewBuilding;


    private FirstPersonController controller;
    private RectTransform openUIScreen;

    private static Camera mainCamera;

	void Awake () {
	    controller = GetComponent<FirstPersonController>();
	    openUIScreen = null;

        // Start the game with the clipboard out
        GetComponent<WeaponManager>().SetWeapon(0);

        // get camera reference
	    mainCamera = GetComponentInChildren<Camera>();
	}

    public void CloseMenu() {
        openUIScreen = null;
        controller.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CanInteract = true;
    }

    public static Transform MainCameraTransform() {
        return mainCamera.transform;
    }

	void Update () {

	    if (Input.GetKeyDown(KeyCode.E)) {
	        // Close any open UI screens
	        if (openUIScreen != null) {
	            openUIScreen.gameObject.SetActive(false);
	            CloseMenu();
	        }
            // Open a menu
	        else {
	            // Create new building menu
	            if (GameController.HighlightedTile != null && GameController.CanBuildOnHighlightedTile()) {
	                pnlCreateNewBuilding.gameObject.SetActive(true);
	                openUIScreen = pnlCreateNewBuilding;
	                controller.enabled = false;
	                CanInteract = false;
	                Cursor.lockState = CursorLockMode.None;
	                Cursor.visible = true;
	            }
                Debug.Log(GameController.CanBuildOnHighlightedTile());
	            
	        }


	    }

        
        
        
	}
}