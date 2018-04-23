using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardClearWeapon : MonoBehaviour {

    void OnGUI() {

        var ammoLabel = GameController.GameUIScreen.AmmoLabel;

        ammoLabel.text = "<b>Colony Manager</b>\n\n\n\n\n\n";

    }
}
