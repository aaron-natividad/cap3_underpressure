using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymptomGroup : MonoBehaviour
{
    [SerializeField] private SymptomScreen symptomScreen;
    [SerializeField] private GameObject symptomInfoPrefab;
    [SerializeField] private float spawnDistance;

    private List<Symptom> symptoms;
    private List<SymptomInfo> infoPanels = new List<SymptomInfo>();

    public void SpawnSymptomInfo(float delay, float interval)
    {
        StartCoroutine(SpawnSymptomInfoIEnum(delay, interval));
    }

    public void DespawnSymptomInfo()
    {
        StopAllCoroutines();
        if (infoPanels.Count <= 0) return;
        foreach (SymptomInfo panel in infoPanels)
        {
            panel.Despawn();
        }
        infoPanels.Clear();
    }

    private IEnumerator SpawnSymptomInfoIEnum(float delay, float interval)
    {
        symptoms = DataManager.instance.currentSymptoms;
        float startSpawnX = -(spawnDistance * (symptoms.Count - 1)) / 2;

        for (int i = 0; i < symptoms.Count; i++)
        {
            SymptomInfo infoPanel = Instantiate(symptomInfoPrefab, transform).GetComponent<SymptomInfo>();
            infoPanel.manager = symptomScreen;
            infoPanel.transform.localPosition = new Vector3(startSpawnX + spawnDistance * i, -50f, 0);
            infoPanel.UpdateInfo(symptoms[i]);
            infoPanel.Spawn();
            infoPanels.Add(infoPanel);
            yield return new WaitForSeconds(interval);
        }
    }
}
