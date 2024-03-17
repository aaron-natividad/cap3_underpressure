using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cover : MonoBehaviour
{   
    [SerializeField] private Image coverImage;

    private void InitializeCover(float startAlpha)
    {
        Color coverColor = coverImage.color;
        coverColor.a = startAlpha;
        coverImage.color = coverColor;
        coverImage.enabled = true;
    }

    public IEnumerator CoverScreen(float duration, float delay)
    {
        InitializeCover(0f);
        yield return new WaitForSecondsRealtime(delay);
        StartCoroutine(GenAnim.Fade(coverImage, 1f, duration));
    }

    public IEnumerator UncoverScreen(float duration, float delay)
    {
        InitializeCover(1f);
        yield return new WaitForSecondsRealtime(delay);
        StartCoroutine(GenAnim.Fade(coverImage, 0f, duration));

        yield return new WaitForSecondsRealtime(duration);
        coverImage.enabled = false;
    }
}
