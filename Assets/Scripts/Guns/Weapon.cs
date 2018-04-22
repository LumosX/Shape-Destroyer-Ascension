using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public string Name;
    public float RefireDelay = 0.1f;
    public float NumberOfPellets = 1;
    public GameObject BulletPrefab;
    public bool UsesAmmoMags = true;

    public int MagSize = 30;
    public float ReloadDuration = 1.7f;

    public float AmmoRegenWaitTime = 0;
    public float AmmoRegenFireDelay = 4;


    private int curAmmo;
    private bool canFire;
    private float nextFireTime;
    private float nextAmmoRegenTime;

    public void Start() {
        curAmmo = MagSize;
        nextFireTime = 0;
        nextAmmoRegenTime = 0;
    }

    public void Update() {
        canFire = curAmmo > 0 && Time.time > nextFireTime;

        if (Input.GetMouseButton(0) && canFire) {
            var camTF = PlayerInstance.MainCameraTransform();
            for (int i = 0; i < NumberOfPellets; i++) {
                Instantiate(BulletPrefab, camTF.position + camTF.forward * 0.4f, camTF.rotation);
            }

            nextFireTime = Time.time + RefireDelay;
            nextAmmoRegenTime = nextFireTime + AmmoRegenFireDelay;
            curAmmo -= 1;
        }
        else if (UsesAmmoMags && Input.GetKeyDown(KeyCode.R) && curAmmo < MagSize && GameController.AmmoMags > 0) {
            nextFireTime = Time.time + ReloadDuration;
            nextAmmoRegenTime = nextFireTime + AmmoRegenFireDelay;
            curAmmo = MagSize;
            if (UsesAmmoMags) GameController.AmmoMags -= 1;
        }

        // ammo regen for the PDW
        if (AmmoRegenWaitTime > 0 && Time.time > nextAmmoRegenTime && curAmmo < MagSize) {
            curAmmo += 1;
            nextAmmoRegenTime = Time.time + AmmoRegenWaitTime;
        }

    }

    void OnGUI() {
        // weapons print their own ammo and shit
        GUI.Label(new Rect(10, Screen.height - 80, 600, 20), "Equipped: " + Name);
        GUI.Label(new Rect(10, Screen.height - 60, 600, 20),
            curAmmo + "/" + MagSize + (UsesAmmoMags ? " | Mags: " + GameController.AmmoMags : ""));
    }

}