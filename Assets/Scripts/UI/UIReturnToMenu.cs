using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIReturnToMenu : MonoBehaviour {

    public void ReturnToMenuClicked() {
        SceneManager.LoadScene("MainMenu");
    }
}
