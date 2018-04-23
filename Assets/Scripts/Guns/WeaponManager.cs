using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    public GameObject[] Weapons;

    private PlayerInstance player;

    private static KeyCode[] codes = new[] {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4};

    void Awake() {
        player = GetComponent<PlayerInstance>();
    }

    // weapon switching
    void Update() {
        var lim = codes.Length < Weapons.Length ? codes.Length : Weapons.Length;
        for (var i = 0; i < lim; i++) {
            var keyCode = codes[i];
            if (Input.GetKeyDown(keyCode)) {
                SetWeapon(i);
            }
        }

        // this is painful
        var weaponLabel = GameController.GameUIScreen.CurrentlyEquipped;
        var gunTexts = "";
        gunTexts += (player.CurWeapon == 0 ? "<b>" : "") + "1: Colony Manager" + (player.CurWeapon == 0 ? "</b>" : "") + "\n";
        gunTexts += (player.CurWeapon == 1 ? "<b>" : "") + "2: Type 7 PDW" + (player.CurWeapon == 1 ? "</b>" : "") + "\n";
        gunTexts += (player.CurWeapon == 2 ? "<b>" : "") + "3: Type 29 Coilgun" + (player.CurWeapon == 2 ? "</b>" : "") + "\n";
        gunTexts += (player.CurWeapon == 3 ? "<b>" : "") + "4: Type 93 Shotgun" + (player.CurWeapon == 3 ? "</b>" : "") + "\n";
        weaponLabel.text = gunTexts + "\n" + 
                           (GameController.IsNight ? "<b>" : "") + "K: Begin next night" + (GameController.IsNight ? "</b>" : "") + 
                           "\nE: Interact";

    }

    public void SetWeapon(int index) {
        // disable all others, enable this one
        for (var i = 0; i < Weapons.Length; i++) {
            Weapons[i].SetActive(i == index);
        }
        player.CurWeapon = index;
    }
}