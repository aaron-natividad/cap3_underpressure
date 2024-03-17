using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public string queuedScene;
    [Space(10)]
    public int previousScore;
    public int previousQuota;

    [Header("Symptoms")]
    public int symptomLimit = 4;
    public List<Symptom> availableSymptoms;
    public List<Symptom> currentSymptoms;

    private void OnEnable()
    {
        ScoreHandler.OnScoreChanged += ReceiveScore;
    }

    private void OnDisable()
    {
        ScoreHandler.OnScoreChanged -= ReceiveScore;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddSymptom(int index)
    {
        if (index > availableSymptoms.Count || currentSymptoms.Count >= symptomLimit) return;
        currentSymptoms.Add(availableSymptoms[index]);
        availableSymptoms.RemoveAt(index);
    }

    public void AddRandomSymptom()
    {
        if (availableSymptoms.Count <= 0 || currentSymptoms.Count >= symptomLimit) return;
        int index = Random.Range(0, availableSymptoms.Count);
        AddSymptom(index);
    }

    public void ReceiveScore(int score, int quota)
    {
        previousScore = score;
        previousQuota = quota;
    }

    public bool EvaluateQuota()
    {
        return previousScore >= previousQuota;
    }
}
