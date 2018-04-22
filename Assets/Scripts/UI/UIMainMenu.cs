using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour {

    public void OnStartGameClicked(int numDays) {
        GameController.DayTarget = numDays;
        SceneManager.LoadScene("GameScene");
    }

    public void OnQuitClicked() {
        Application.Quit();
    }

}
