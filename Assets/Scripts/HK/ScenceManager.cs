using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenceManager : MonoBehaviour
{
    public static ScenceManager instance;

    public Image fadeImage;
    public float fadeInTime;
    public float fadeOutTIme;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartCoroutine(Fadein());
    }
    public IEnumerator Fadein()
    {
        Color originalColor = fadeImage.color;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeInTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeInTime);
            fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    public IEnumerator Fadeout()
    {
        Color originalColor = fadeImage.color;
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeInTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInTime);
            fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
