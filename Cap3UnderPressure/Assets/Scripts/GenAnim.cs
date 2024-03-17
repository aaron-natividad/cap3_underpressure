using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class GenAnim
{
    public static Action<bool> OnAnimationStateChanged;

    public static IEnumerator Fade(CanvasGroup group, float toAlpha, float fadeTime)
    {
        float startAlpha = group.alpha;
        float currentAlpha;
        float t = 0;

        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, toAlpha, t / fadeTime);
            group.alpha = currentAlpha;
            yield return null;
        }

        group.alpha = toAlpha;
    }

    public static IEnumerator Fade(Image image, float toAlpha, float fadeTime)
    {
        float startAlpha = image.color.a;
        float currentAlpha;
        float t = 0;
        Color currentColor = image.color;

        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, toAlpha, t / fadeTime);
            currentColor.a = currentAlpha;
            image.color = currentColor;
            yield return null;
        }

        currentColor.a = toAlpha;
        image.color = currentColor;
    }

    public static IEnumerator Fade(TextMeshProUGUI textObject, float toAlpha, float fadeTime)
    {
        float startAlpha = textObject.color.a;
        float currentAlpha;
        float t = 0;
        Color currentColor = textObject.color;

        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, toAlpha, t / fadeTime);
            currentColor.a = currentAlpha;
            textObject.color = currentColor;
            yield return null;
        }

        currentColor.a = toAlpha;
        textObject.color = currentColor;
    }

    public static IEnumerator PlayText(TextMeshProUGUI tmp, string text, float delay, AudioSource audioSource = null, AudioClip audioClip = null)
    {
        OnAnimationStateChanged?.Invoke(true);
        bool inCommand = false;
        string currentText = "";

        for (int i = 0; i < text.Length; i++)
        {
            currentText += text[i];

            if (text[i] == '<')
            {
                inCommand = true;
                continue;
            }
            else if (text[i] == '>')
            {
                inCommand = false;
                continue;
            }
            if (inCommand) continue;

            tmp.text = currentText;

            if (audioClip != null)
            {
                audioSource.pitch = Random.Range(0.6f, 1.2f);
                audioSource?.PlayOneShot(audioClip);
            }
            
            yield return new WaitForSeconds(delay);
        }
        OnAnimationStateChanged?.Invoke(false);
    }
}
