using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmissionLight : MonoBehaviour
{
    [SerializeField] private Renderer lightMesh;
    [SerializeField] private Light pointLight;

    [Header("Colors")]
    [SerializeField] private Color baseColor;
    [SerializeField] private Color correctColor;
    [SerializeField] private Color wrongColor;
    [SerializeField] private float lightIntensity;

    private void Awake()
    {
        lightMesh.material = new Material(lightMesh.material);
        lightMesh.material.color = baseColor;
        pointLight.color = baseColor;
        pointLight.intensity = 0;
    }

    public void DisplayColor(bool isCorrect, float time)
    {
        StartCoroutine(CO_DisplayColor(isCorrect, time));
    }

    private IEnumerator CO_DisplayColor(bool isCorrect, float time)
    {
        float t = 0;
        Color lightColor = isCorrect ? correctColor : wrongColor;
        lightMesh.material.color = lightColor;
        pointLight.color = lightColor;
        pointLight.intensity = lightIntensity;
        
        while (t < time)
        {
            t += Time.deltaTime;
            lightMesh.material.color = Color.Lerp(lightColor, baseColor, t / time);
            pointLight.intensity = Mathf.Lerp(lightIntensity, 0f, t / time);
            yield return null;
        }

        lightMesh.material.color = baseColor;
        pointLight.color = baseColor;
        pointLight.intensity = 0;
    }
}
