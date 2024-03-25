using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUI : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip passSFX;
    [SerializeField] private AudioClip failSFX;

    [Header("Light")]
    [SerializeField] private Renderer lightMesh;
    [SerializeField] private Light spotLight;

    [Header("Screens")]
    [SerializeField] private EvaluationScreen evalScreen;
    [SerializeField] private ResultScreen resultScreen;
    
    private Material lightMat;
    private bool passed;

    private void Start()
    {
        passed = DataManager.instance.EvaluateQuota();
        lightMesh.material = new Material(lightMesh.material);
        lightMat = lightMesh.material;
    }

    public void PlayEvaluation()
    {
        StartCoroutine(CO_PlayEvaluation());
    }

    private void ChangeLightColor(Color color)
    {
        lightMat.SetColor("_EmissionColor", color);
        spotLight.color = color;
    }

    private void ShiftScreen(int index)
    {
        evalScreen.SetEnabled(index == 0);
        resultScreen.SetEnabled(index == 1);
    }

    private IEnumerator CO_PlayEvaluation()
    {
        ShiftScreen(0);
        evalScreen.ShowEvaluation();
        ChangeLightColor(Color.black);
        yield return new WaitForSeconds(4f);

        ShiftScreen(1);
        resultScreen.ShowResult(passed);
        ChangeLightColor(passed ? Color.green : Color.red);
        audioSource.PlayOneShot(passed ? passSFX : failSFX);
    }
}
