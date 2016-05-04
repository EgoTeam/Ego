using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour {
    public void OnStartButtonClicked() {
        SceneManager.LoadSceneAsync(1);
    }
    public void OnQuitButtonClicked() {
        //Quit the Application.
        Application.Quit();
    }
}
