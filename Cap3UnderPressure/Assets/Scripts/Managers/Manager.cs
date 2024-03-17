using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Action OnStartGame;
    public static Action OnEndGame;

    public static Manager instance;

    private bool canPause;
    private bool isPaused;
    private PlayerState storedPlayerState;

    private void OnEnable()
    {
        Timer.OnTimerFinish += EndGame;
    }

    private void OnDisable()
    {
        Timer.OnTimerFinish -= EndGame;
    }

    private void Awake()
    {
        CreateSingleton();
        DialogueHandler.OnIntroFinish += StartGame;
        isPaused = false;
        canPause = false;
        OnAwake();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            TogglePause();
        }
    }

    protected void CreateSingleton()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    protected virtual void OnAwake()
    {

    }

    protected virtual void StartGame()
    {
        DialogueHandler.OnIntroFinish -= StartGame;
        PlayerUI.instance.gameUI.SetEnabled(true);
        Player.instance.state = PlayerState.Normal;
        canPause = true;
        OnStartGame?.Invoke();
    }

    protected virtual void EndGame()
    {
        canPause = false;
        OnEndGame?.Invoke();
    }

    public virtual void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        PlayerUI.instance.TogglePause(isPaused);

        if (isPaused)
        {
            storedPlayerState = Player.instance.state;
            Player.instance.state = PlayerState.Disabled;
        }
        else
        {
            Player.instance.state = storedPlayerState;
        }
    }
}
