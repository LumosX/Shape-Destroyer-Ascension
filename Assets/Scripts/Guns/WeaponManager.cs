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
    }

    public void SetWeapon(int index) {
        // disable all others, enable this one
        for (var i = 0; i < Weapons.Length; i++) {
            Weapons[i].SetActive(i == index);
        }
        player.CurWeapon = index;
    }
}