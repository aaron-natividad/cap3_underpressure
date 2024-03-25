using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI instance;

    [Header("Main Groups")]
    public GameUI gameUI;
    public DialogueUI dialogueUI;
    public PauseUI pauseUI;
    public GameObject crosshair;
    public SymptomsBar symptomsBar;

    private bool gameUIState;
    private bool dialogueUIState;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    private void Start()
    {
        gameUI.SetEnabled(false);
        dialogueUI.SetEnabled(false);
        pauseUI.SetEnabled(false);
    }

    public void TogglePause(bool isPaused)
    {
        if (isPaused)
        {
            gameUIState = gameUI.GetEnabled();
            dialogueUIState = dialogueUI.GetEnabled();
        }

        gameUI.SetEnabled(isPaused ? false : gameUIState);
        dialogueUI.SetEnabled(isPaused ? false : dialogueUIState);
        pauseUI.SetEnabled(isPaused);
        pauseUI.PlayClickSound();
        crosshair.SetActive(!isPaused);

        Cursor.lockState = isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
    }
}
