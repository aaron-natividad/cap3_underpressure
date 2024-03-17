using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationManager : MonoBehaviour
{
    [SerializeField] private SceneHandler sceneHandler;
    
    [Header("Elevator")]
    [SerializeField] private GameObject elevator;
    [Space(10)]
    [SerializeField] private float passDistance;
    [SerializeField] private float passDuration;
    [Space(10)]
    [SerializeField] private float failDistance;
    [SerializeField] private float failDuration;

    [Header("Light")]
    [SerializeField] private Renderer lightMesh;
    [SerializeField] private Light spotLight;

    [Header("Screens")]
    [SerializeField] private EvaluationScreen evalScreen;
    [SerializeField] private ResultScreen resultScreen;

    private void OnEnable()
    {
        DialogueHandler.OnIntroFinish += PlayEvaluationSequence;
    }

    private void OnDisable()
    {
        DialogueHandler.OnIntroFinish -= PlayEvaluationSequence;
    }

    private void Start()
    {
        lightMesh.material = new Material(lightMesh.material);
    }

    private void PlayEvaluationSequence()
    {
        StartCoroutine(CO_EvaluationSequence());
    }

    private void ChangeLightColor(Color color)
    {
        lightMesh.material.SetColor("_EmissionColor",color);
        spotLight.color = color;
    }

    private IEnumerator CO_EvaluationSequence()
    {
        evalScreen.gameObject.SetActive(true);
        evalScreen.PlayAnimation();
        ChangeLightColor(Color.black);
        yield return new WaitForSeconds(4f);

        bool passed = DataManager.instance.EvaluateQuota();
        evalScreen.gameObject.SetActive(false);
        resultScreen.gameObject.SetActive(true);
        resultScreen.ShowResult(passed);
        ChangeLightColor(passed ? Color.green : Color.red);
        yield return new WaitForSeconds(2f);

        DataManager.instance.AddRandomSymptom();
        float elevDistance = passed ? passDistance : failDistance;
        float elevDuration = passed ? passDuration : failDuration;
        LeanTween.move(elevator, elevator.transform.position + Vector3.up * elevDistance, elevDuration);
        yield return new WaitForSeconds(1f);

        if(DataManager.instance.queuedScene == "BurnoutIntro")
        {
            sceneHandler.LoadScene(DataManager.instance.queuedScene);
            DataManager.instance.queuedScene = "Tutorial 2";
        }
        else
        {
            sceneHandler.LoadNextScene();
        }
    }
}
