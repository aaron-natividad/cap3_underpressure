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

    #region Unity Messages
    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void Awake()
    {
        CreateSingleton();
        isPaused = false;
        canPause = false;
        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            TogglePause();
        }
    }
    #endregion

    #region Initializers
    protected virtual void AddListeners()
    {
        Timer.OnTimerFinish += EndGame;
        Elevator.OnElevatorLift += EndGame;
        SceneHandler.OnSceneReady += EnablePause;
        DialogueHandler.OnDialogueStart += StartGame;
    }

    protected virtual void RemoveListeners()
    {
        Timer.OnTimerFinish -= EndGame;
        Elevator.OnElevatorLift -= EndGame;
        SceneHandler.OnSceneReady -= EnablePause;
        DialogueHandler.OnDialogueStart -= StartGame;
    }
    
    protected virtual void OnAwake()
    {

    }

    protected virtual void OnStart()
    {

    }

    protected void CreateSingleton()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
    #endregion

    #region In-Game
    protected virtual void StartGame()
    {
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
    #endregion

    #region Pause Methods
    protected virtual void EnablePause()
    {
        canPause = true;
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
            AudioManager.instance.Pause();
        }
        else
        {
            Player.instance.state = storedPlayerState;
            AudioManager.instance.Play();
        }
    }
    #endregion
}
