using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static Action OnSceneReady;
    public static Action OnSceneLoading;

    [SerializeField] private Cover cover;
    public string nextScene;

    private void OnEnable()
    {
        DialogueHandler.OnDialogueEnd += LoadNextScene;
    }

    private void OnDisable()
    {
        DialogueHandler.OnDialogueEnd -= LoadNextScene;
    }

    private void Start()
    {
        if (Player.instance != null) Player.instance.state = PlayerState.Disabled;
        StartCoroutine(CO_StartScene());
    }

    public void LoadNextScene()
    {
        StartCoroutine(CO_LoadScene(nextScene));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(CO_LoadScene(sceneName));
    }

    private IEnumerator CO_StartScene()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(cover.UncoverScreen(0.5f, 0.5f));
        yield return new WaitForSeconds(1f);
        OnSceneReady?.Invoke();
    }

    private IEnumerator CO_LoadScene(string sceneName)
    {
        OnSceneLoading?.Invoke();
        Player.instance?.ChangeState(PlayerState.Disabled);
        StartCoroutine(cover.CoverScreen(0.5f, 0.5f));
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(sceneName);
    }
}
