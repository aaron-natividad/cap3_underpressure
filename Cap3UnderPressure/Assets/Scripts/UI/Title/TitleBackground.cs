using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBackground : MonoBehaviour
{
    [SerializeField] private float movementDelay;

    [Header("Background Elements")]
    [SerializeField] private GameObject[] elements;

    public void Move(float duration, float dist)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            LeanTween.moveLocal(
                elements[i],
                elements[i].transform.localPosition + Vector3.up * dist,
                duration + i * movementDelay
                ).setEase(LeanTweenType.easeInOutCubic);
        }
    }
}
