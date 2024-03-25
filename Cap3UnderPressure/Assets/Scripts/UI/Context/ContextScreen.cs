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
    [SerializeField] private float introDelay;
    [SerializeField] private float typewriterSoundDelay;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typewriterSound;
    [SerializeField] private AudioClip clickSound;

    [Header("Components")]
    [SerializeField] private Image introPanel;
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private TextMeshProUGUI lmbText;
    [SerializeField] private GameObject introButton;

    private bool inAnimation;
    private int itemIndex;

    private void OnEnable()
    {
        SceneHandler.OnSceneReady += InitializeIntro;
        GenAnim.OnAnimationStateChanged += ChangeAnimationState;
    }

    private void OnDisable()
    {
        SceneHandler.OnSceneReady -= InitializeIntro;
        GenAnim.OnAnimationStateChanged -= ChangeAnimationState;
    }

    private void InitializeIntro()
    {
        itemIndex = 0;
        StartCoroutine(PlayTypewriterSound());
        PlayCurrentItem();
        Cursor.lockState = CursorLockMode.None;
    }

    public void MoveToNextItem()
    {
        if (inAnimation) return;

        itemIndex++;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(clickSound);
        if (itemIndex < introGroup.contextItems.Length)
        {
            PlayCurrentItem();
        }
        else
        {
            StopAllCoroutines();
            introButton.SetActive(false);
            OnContextFinish?.Invoke();
            sceneHandler.LoadNextScene();
        }
    }

    private void PlayCurrentItem()
    {
        if (introGroup.contextItems[itemIndex].panel != null)
            UpdatePanel(introGroup.contextItems[itemIndex].panel, 0.25f);
        StartCoroutine(GenAnim.PlayTextByLetter(introText, introGroup.contextItems[itemIndex].text, introDelay));
    }

    private void UpdatePanel(Sprite currentPanel, float duration)
    {
        introPanel.color = new Color(1f, 1f, 1f, 0f);
        introPanel.sprite = currentPanel;
        StartCoroutine(GenAnim.Fade(introPanel, 1f, duration));
    }

    private void ChangeAnimationState(bool animState)
    {
        inAnimation = animState;
        lmbText.enabled = !inAnimation;
    }

    private IEnumerator PlayTypewriterSound()
    {
        while (true)
        {
            if (inAnimation)
            {
                audioSource.pitch = Random.Range(1f, 1.4f);
                audioSource.PlayOneShot(typewriterSound);
            }
            yield return new WaitForSeconds(typewriterSoundDelay);
        }
    }
}
