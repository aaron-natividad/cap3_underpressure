using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIEnabler : MonoBehaviour
{
    private void OnEnable()
    {
        Manager.OnStartGame += OnStartGame;
        Manager.OnEndGame += OnEndGame;
    }

    private void OnDisable()
    {
        Manager.OnStartGame -= OnStartGame;
        Manager.OnEndGame -= OnEndGame;
    }

    private void OnStartGame()
    {
        PlayerUI.instance.gameUI.SetEnabled(true);
        PlayerUI.instance.gameUI.UpdateTimer(0);
        PlayerUI.instance.gameUI.UpdateColorRequirement(RobotColor.Red);
    }

    private void OnEndGame()
    {

    }
}
