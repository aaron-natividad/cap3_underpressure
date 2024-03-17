using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] pauseGroups;

    private CanvasGroup mainGroup;
    private bool isEnabled;

    private void Awake()
    {
        mainGroup = GetComponent<CanvasGroup>();
    }

    public void SetEnabled(bool isEnabled)
    {
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

    public void FadeCanvasGroup(int index)
    {
        SetCanvasGroup(index);
        StartCoroutine(GenAnim.Fade(pauseGroups[index], 1, 0.25f));
    }

    //CHANGE
    public void Unpause()
    {
        Manager.instance.TogglePause();
    }

    //CHANGE
    public void LoadTitleScreen()
    {
        Manager.instance.TogglePause();
        Player.instance.state = PlayerState.Disabled;
        Manager.instance.GetComponent<SceneHandler>().LoadScene("Title");
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
