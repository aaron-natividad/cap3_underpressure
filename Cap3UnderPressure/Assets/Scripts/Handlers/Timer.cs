using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Action OnTimerFinish;
    public static Action OnTimerHalfway;
    public static Action OnTimerCritical;
    public static Action<int> OnTimerSecondsChanged;

    [SerializeField] private int timerSeconds;

    private int internalTimerSeconds;
    private bool halfwayPassed = false;
    private bool criticalPassed = false;

    public void Initialize()
    {
        internalTimerSeconds = timerSeconds;
        if (timerSeconds > 0) StartCoroutine(RunTimer());
    }

    public void ForceChangeTimer(int time)
    {
        internalTimerSeconds = time;
        OnTimerSecondsChanged?.Invoke(internalTimerSeconds);
        SendTimerData();
    }

    public void ForceResetTimer()
    {
        internalTimerSeconds = timerSeconds;
        OnTimerSecondsChanged?.Invoke(internalTimerSeconds);
        SendTimerData();
    }

    private IEnumerator RunTimer()
    {
        OnTimerSecondsChanged?.Invoke(internalTimerSeconds);
        while (internalTimerSeconds > 0)
        {
            yield return new WaitForSeconds(1f);
            internalTimerSeconds--;
            OnTimerSecondsChanged?.Invoke(internalTimerSeconds);
            SendTimerData();
        }
        OnTimerFinish?.Invoke();
    }

    private void SendTimerData()
    {
        if (internalTimerSeconds <= 15f && !criticalPassed)
        {
            Debug.Log("Crit");
            OnTimerCritical?.Invoke();
            criticalPassed = true;
            return;
        }


        if ((float)internalTimerSeconds/timerSeconds <= 0.5f && !halfwayPassed)
        {
            Debug.Log("Half");
            OnTimerHalfway?.Invoke();
            halfwayPassed = true;
            return;
        }
    }
}
