using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGeneratorUpgrade : MonoBehaviour {

    public Button btnExtraPower;
    public Button btnResilience;

    void Start() {
        btnExtraPower.onClick.AddListener(() => {
            GameController.Materials -= 100;
            GameController.Power += 1;
        });
        btnResilience.onClick.AddListener(() => {
            GameController.Materials -= 100;
            GameController.GeneratorResilience += 0.02f;
            if (GameController.GeneratorResilience > 0.7f) GameController.GeneratorResilience = 0.7f;
        });
    }


    void Update() {
        var upgradesAvailable = GameController.Materials >= 100;
        btnExtraPower.interactable = upgradesAvailable;
        btnResilience.interactable = upgradesAvailable;
        // 70% limit on resilience, just because
        if (GameController.GeneratorResilience >= 0.7f) btnResilience.interactable = false;
    }

}
