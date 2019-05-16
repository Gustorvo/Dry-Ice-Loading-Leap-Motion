using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour {

    public static FadeManager instance;
    public Image FadeImage;
    public Color startColor;
    public Color endColor;
    public float duration;

    private bool isFading;

    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {
        FadeIn();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void FadeOutToScene(int sceneIndex)
    {
        StartCoroutine(BeginFade(sceneIndex));
    }

    public void FadeIn()
    {
        StartCoroutine(BeginFade());
    }
    private IEnumerator BeginFade()
    {
        isFading = true;
        float timer = -1; //added 1 sec for longer fade in
        while (timer <= duration)
        {
            FadeImage.color = Color.Lerp(startColor, endColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        isFading = false;
    }

    private IEnumerator BeginFade(int sceneIndex)
    {
        isFading = true;
        float timer = 0;
        while (timer <= duration)
        {
            FadeImage.color = Color.Lerp(endColor, startColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        isFading = false;
        SceneManager.LoadScene(sceneIndex);
    }
}
