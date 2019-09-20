using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private Image fadeThrough = null;

    [Range(0.1f, 10f)]
    [SerializeField] private float fadeSpeed = 1f;


    private void Start()
    {
        fadeThrough.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
    }


    public void FadeToClear(bool fading)
    {
        
        if (fading == true)
        {
            if (fadeThrough.color.a > 0.0001f)
            {
                print("Clearing Screen");
                StartCoroutine("FadeThrough", Color.clear);
            }
            else if (fadeThrough.color.a == 0)
            {
                fadeThrough.color = Color.clear;
                fading = false;
            }
        }
    }


    public void FadeToBlack(bool fading)
    {
        if (fading == true)
        {

            if (fadeThrough.color.a < 0.9999f)
            {
                print("Fading to black");
                StartCoroutine("FadeThrough", Color.black);
            }
            else if (fadeThrough.color.a == 1)
            {
                fadeThrough.color = Color.black;
                fading = false;
            }
        }
    }


    IEnumerator FadeThrough(Color color)
    {
        yield return new WaitForSeconds(0.01f);
        fadeThrough.color = Color.LerpUnclamped(fadeThrough.color, color, fadeSpeed * Time.deltaTime);
        if (color.a == 1)
        {
            FadeToBlack(true);
        }
        else if (color.a == 0)
        {
            FadeToClear(true);
        }

    }

}
