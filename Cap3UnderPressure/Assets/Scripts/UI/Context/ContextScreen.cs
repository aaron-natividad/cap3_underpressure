using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ContextScreen : MonoBehaviour
{
    public static Action OnContextFinish;

    [SerializeField] private SceneHandler sceneHandler;
    [SerializeField] private ContextInfo introGroup;

    [Header("Parameters")]
    [SerializeField] private float textDelay;
    [SerializeField] private float soundDelay;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip textSound;
    [SerializeField] private AudioClip clickSound;

    [Header("Components")]
    [SerializeField] private Image introPanel;
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject skipButton;

    private int itemIndex;
    private Coroutine anim_CO;

    private void OnEnable()
    {
        SceneHandler.OnSceneReady += InitializeContext;
    }

    private void OnDisable()
    {
        SceneHandler.OnSceneReady -= InitializeContext;
    }

    private void InitializeContext()
    {
        itemIndex = 0;
        Cursor.lockState = CursorLockMode.None;

        nextButton.SetActive(true);
        skipButton.SetActive(true);

        StartCoroutine(PlaySound());
        PlayCurrentItem();
    }

    public void Click()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(clickSound);
    }

    public void EndContext()
    {
        StopAllCoroutines();
        nextButton.SetActive(false);
        skipButton.SetActive(false);

        OnContextFinish?.Invoke();
        sceneHandler.LoadNextScene();
    }

    public void MoveToNextItem()
    {
        if (SkipItem() || itemIndex >= introGroup.contextItems.Length) return;
        itemIndex++;

        if (itemIndex < introGroup.contextItems.Length)
            PlayCurrentItem();
        else
            EndContext();
    }

    private void PlayCurrentItem()
    {
        if (introGroup.contextItems[itemIndex].panel != null)
            UpdatePanel(introGroup.contextItems[itemIndex].panel, 0.25f);
        anim_CO = StartCoroutine(PlayText(introText, introGroup.contextItems[itemIndex].text, textDelay));
    }

    private bool SkipItem()
    {
        if (anim_CO != null)
        {
            StopCoroutine(anim_CO);
            anim_CO = null;
            introText.text = introGroup.contextItems[itemIndex].text;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdatePanel(Sprite currentPanel, float duration)
    {
        introPanel.color = new Color(1f, 1f, 1f, 0f);
        introPanel.sprite = currentPanel;
        StartCoroutine(GenAnim.Fade(introPanel, 1f, duration));
    }

    private IEnumerator PlaySound()
    {
        while (true)
        {
            if (anim_CO != null)
            {
                audioSource.pitch = Random.Range(1f, 1.4f);
                audioSource.PlayOneShot(textSound);
            }
            yield return new WaitForSeconds(soundDelay);
        }
    }

    private IEnumerator PlayText(TextMeshProUGUI tmp, string text, float delay)
    {
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
            yield return new WaitForSeconds(delay);
        }

        anim_CO = null;
    }
}
