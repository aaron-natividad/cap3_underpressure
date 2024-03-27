using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    private Timer timer;
    private ScoreHandler scoreHandler;

    void Start()
    {
        timer = Manager.instance.gameObject.GetComponent<Timer>();
        scoreHandler = Manager.instance.gameObject.GetComponent<ScoreHandler>();
    }

    public void ForceChangeScore(int score)
    {
        if (scoreHandler == null) return;
        scoreHandler.ForceChangeScore(score);
    }

    public void ForceChangeScoreToQuota()
    {
        if (scoreHandler == null) return;
        scoreHandler.ForceChangeScoreToQuota();
    }

    public void ForceChangeTimer(int time)
    {
        if (timer == null) return;
        timer.ForceChangeTimer(time);
    }

    public void ForceResetTimer()
    {
        if (timer == null) return;
        timer.ForceResetTimer();
    }
}
