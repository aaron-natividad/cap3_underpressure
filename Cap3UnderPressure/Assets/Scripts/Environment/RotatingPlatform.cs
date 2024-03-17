using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3[] movePoints;
    [SerializeField] private float delay;

    private void Start()
    {
        StartCoroutine(MoveAround());
    }

    private IEnumerator MoveAround()
    {
        while(true)
        {
            for(int i = 0; i < movePoints.Length; i++)
            {
                LeanTween.move(gameObject, movePoints[i], delay);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
