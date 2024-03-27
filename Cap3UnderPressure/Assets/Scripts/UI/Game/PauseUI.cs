using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Volume blurVolume;

    [SerializeField] private AudioClip clickSound;
    [SerializeField] private CanvasGroup[] pauseGroups;

    [SerializeField] private GameObject skipTutorialButton;
    [SerializeField] private bool canSkipTutorial;

    private AudioSource audioSource;
    private CanvasGroup mainGroup;
    private bool isEnabled;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        mainGroup = GetComponent<CanvasGroup>();
        skipTutorialButton.SetActive(canSkipTutorial);
    }

    public void SetEnabled(bool isEnabled)
    {
        blurVolume.enabled = isEnabled;
        mainGroup.alpha = isEnabled ? 1 : 0;
        mainGroup.interactable = isEnabled;
        mainGroup.blocksRaycasts = isEnabled;
        this.isEnabled = isEnabled;

        if (isEnabled)
        {
            SetCanvasGroup(0);
            pauseGroups[0].alpha = 1f;
        }
    }

    public bool GetEnabled()
    {
        return isEnabled;
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }

    public void FadeCanvasGroup(int index)
    {
        audioSource.PlayOneShot(clickSound);
        SetCanvasGroup(index);
        StartCoroutine(GenAnim.Fade(pauseGroups[index], 1, 0.25f));
    }

    public void Unpause()
    {
        Manager.instance.TogglePause();
    }

    public void LoadTitleScreen()
    {
        GameObject dataManager = DataManager.instance.gameObject;

        Manager.instance.TogglePause();
        Player.instance.state = PlayerState.Disabled;
        Time.timeScale = 0;
        Manager.instance.GetComponent<SceneHandler>().LoadScene("Title");
        DataManager.instance = null;
        Destroy(dataManager);
    }

    public void LoadNextScene()
    {
        Manager.instance.TogglePause();
        Player.instance.state = PlayerState.Disabled;
        Time.timeScale = 0;
        Manager.instance.GetComponent<SceneHandler>().LoadNextScene();
    }

    private void DisableCanvasGroups()
    {
        foreach (CanvasGroup cg in pauseGroups)
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    private void SetCanvasGroup(int index)
    {
        DisableCanvasGroups();
        pauseGroups[index].interactable = true;
        pauseGroups[index].blocksRaycasts = true;
    }
}
