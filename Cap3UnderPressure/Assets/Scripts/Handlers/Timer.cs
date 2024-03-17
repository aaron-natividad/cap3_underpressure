using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Action OnTimerFinish;
    public static Action<int> OnTimerSecondsChanged;

    [SerializeField] private int timerSeconds;

    public void Initialize()
    {
        if (timerSeconds > 0) StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer()
    {
        OnTimerSecondsChanged?.Invoke(timerSeconds);
        while (timerSeconds > 0)
        {
            yield return new WaitForSeconds(1f);
            timerSeconds--;
            OnTimerSecondsChanged?.Invoke(timerSeconds);
        }
        OnTimerFinish?.Invoke();
    }
}
