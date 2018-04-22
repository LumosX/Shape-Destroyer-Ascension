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
    public RectTransform pnlUpgradeGenerator;
    public RectTransform pnlUpgradeBuilding;
    public RectTransform pnlVictory;
    public RectTransform pnlDefeat;


    private FirstPersonController controller;
    private RectTransform openUIScreen;

    private static Camera mainCamera;

    void Awake() {
        controller = GetComponent<FirstPersonController>();
        openUIScreen = null;

        // get camera reference
        mainCamera = GetComponentInChildren<Camera>();
    }

    void Start() {
        // Start the game with the clipboard out
        GetComponent<WeaponManager>().SetWeapon(0);
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

    void Update() {

        if (Input.GetKeyDown(KeyCode.E)) {
            // Close any open UI screens
            if (openUIScreen != null) {
                openUIScreen.gameObject.SetActive(false);
                CloseMenu();
            }
            // Open a menu
            else if (GameController.HighlightedTile != null) {
                // Create new building menu
                if (GameController.CanBuildOnHighlightedTile()) {
                    pnlCreateNewBuilding.gameObject.SetActive(true);
                    openUIScreen = pnlCreateNewBuilding;
                    controller.enabled = false;
                    CanInteract = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else if (GameController.HighlightedTileIsGenerator()) {
                    pnlUpgradeGenerator.gameObject.SetActive(true);
                    openUIScreen = pnlUpgradeGenerator;
                    controller.enabled = false;
                    CanInteract = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else if (GameController.CanUpgradeHighlightedTile()) {
                    UIUpgradeBuilding.CurrentBuilding = GameController.HighlightedTile.CurrentBuilding;
                    pnlUpgradeBuilding.gameObject.SetActive(true);
                    openUIScreen = pnlUpgradeBuilding;
                    controller.enabled = false;
                    CanInteract = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

            }


        }

        // Night triggers.
        if (Input.GetKeyDown(KeyCode.K) && !GameController.IsNight) {
            GameController.NightTriggered();
        }


    }

    public void WinGame() {
        Time.timeScale = 0;
        pnlVictory.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoseGame() {
        Time.timeScale = 0;
        pnlDefeat.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}