﻿//Import Statements
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : Singleton<GameController> {

    private string mainMenu = "-100 - Main Menu";	//The name of the main menu scene.

    //Declare private class level constant variables.
    private const float DEFAULT_FADE = 5.0f;	//The default fade time for the screen and audio.

    /// <summary>
    /// 
    /// </summary>
    void Start() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name != mainMenu)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Load the main menu.
            ResetGame();
        }
    }
    /**
        * Method Name: OnLevelWasLoaded
        * Description: Method handles behavior when a new level is loaded.
        * @param index.
        */
    void OnLevelWasLoaded(int index)
    {
        //Print that the level was loaded.
        Debug.Log("Loaded");
        //If the index of the current level is the loaded level...
        if (index == SceneManager.GetActiveScene().buildIndex)
        {
            //...Stop all the active coroutines.
            StopAllCoroutines();
            //...Start audio fade in.
            AudioController.Instance.AudioFadeIn(DEFAULT_FADE);
            //...Start scene fade in.
            FadeController.Instance.SceneFadeIn(DEFAULT_FADE);
        }
    }
    /**
 * Method Name: LoadLevel
 * Description: Method loads a specified level.
 * @param level, duration
 */
    public void LoadLevel(string level, float duration)
    {
        //Start the LoadLevel coroutine.
        StartCoroutine(LoadLevelCoroutine(level, duration));
    }
    /**
	 * Method Name: RestartLevel
	 * Description: Method reloads the currently loaded level.
	 */
    public void RestartLevel()
    {
        //Start the LoadLevel coroutine.
        StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().name, DEFAULT_FADE));
    }
    /**
	 * Method Name: ResetGame
	 * Description: Method reloads the entire game.
	 */
    public void ResetGame()
    {
        //Start the LoadLevel coroutine.
        StartCoroutine(LoadLevelCoroutine(mainMenu, DEFAULT_FADE));
    }
    /**
 * Coroutine Name: LoadLevelCoroutine
 * Description   : Coroutine loads the specified level.
 */
    private IEnumerator LoadLevelCoroutine(string level, float duration)
    {
        //Print that the level is loading.
        Debug.Log("Loading level...");
        //Fade out the scene.
        FadeController.Instance.SceneFadeOut(duration);
        //Fade out the audio.
        AudioController.Instance.AudioFadeOut(duration);
        //Wait for a period of time.
        yield return new WaitForSeconds(duration + 1.0f);
        //Load the level specified.
        SceneManager.LoadSceneAsync(level);
    }
}