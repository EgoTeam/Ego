using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeController : Singleton<FadeController> {

    /*DATA MEMBERS*/
    [SerializeField]
    private Image _fadeImage;

    /**
	 * Method Name: Start
	 * Description: Method executes once after the Awake method.
	 */
    void Start()
    {
        _fadeImage = GetComponentInChildren<Image>();
    }
    /**
	 * Method Name: OnLevelWasLoaded
	 * Description: Method handles behavior when a new level is loaded.
	 * @param index.
	 */
    void OnLevelWasLoaded(int index)
    {
        //If the index of the current level is the loaded level...
        if (index == SceneManager.GetActiveScene().buildIndex)
        {
            //...update the canvas renderer.
            _fadeImage = GetComponentInChildren<Image>();
        }
    }
    /**
	 * Method Name: SceneFadeIn
	 * Description: Method calls the SceneFadeIn coroutine.
	 * @param duration.
	 */
    public void SceneFadeIn(float duration)
    {
        //Start the SceneFadeIn coroutine.
        StartCoroutine(SceneFadeInCoroutine(duration));
    }
    /**
	 * Method Name: SceneFadeOut
	 * Description: Method calls the SceneFadeOut coroutine.
	 * @param duration.
	 */
    public void SceneFadeOut(float duration)
    {
        //Start the SceneFadeOut coroutine.
        StartCoroutine(SceneFadeOutCoroutine(duration));
    }
    /**
	 * Coroutine Name: SceneFadeInCoroutine
	 * Description	 : Coroutine handles behavior for fading in a scene.
	 * @param duration
	 */
    private IEnumerator SceneFadeInCoroutine(float duration)
    {
        _fadeImage.enabled = true;
        //Set the alpha of the Canvas Renderer to completely black.
        _fadeImage.color = new Color(0f,0f,0f,1f);
        //Wait one second before starting fade in.
        yield return new WaitForSeconds(1.0f);
        //While the alpha value of the image is greater than zero fade...
        while (_fadeImage.color.a > 0.0f)
        {
            //...Decrease the alpha value of the image...
            _fadeImage.color = new Color(0f, 0f, 0f, _fadeImage.color.a - Time.deltaTime / duration);
            //Print the alpha value of the Image.
            //Debug.Log(_fadeImage.color.a);
            //...Yield until next frame update before continuing loop execution...
            yield return 0;
        }
        _fadeImage.enabled = false;
    }
    /**
	 * Coroutine Name: SceneFadeOutCoroutine
	 * Description	 : Coroutine handles behavior for fading out a scene.
	 * @param duration
	 */
    private IEnumerator SceneFadeOutCoroutine(float duration)
    {
        _fadeImage.enabled = true;
        //Set the alpha value of the image to fully transparent.
        _fadeImage.color = new Color(0f, 0f, 0f, 0f);
        //Wait one second before starting fade out.
        yield return new WaitForSeconds(1.0f);
        //While the alpha value fo the image is less than one fade out...
        while (_fadeImage.color.a < 1.0f)
        {
            //...Increase the alpha value of the image...
            _fadeImage.color = new Color(0f, 0f, 0f, _fadeImage.color.a + Time.smoothDeltaTime / duration);
            //Print the alpha value of the Image.
            //Debug.Log(_fadeImage.GetAlpha());
            //...Yield until the next frame update before continuing loop execution...
            yield return 0;
        }
    }
}
