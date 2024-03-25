using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EvalElevator : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip elevatorPassSFX;
    [SerializeField] private AudioClip elevatorFailSFX;

    [Header("Parameters")]
    [SerializeField] private float passSpeed;
    [SerializeField] private float failSpeed;
    [SerializeField] private float tpDistance;

    public void MoveElevator(bool passed)
    {
        float speed = passed ? passSpeed : failSpeed;
        audioSource.PlayOneShot(passed ? elevatorPassSFX : elevatorFailSFX);
        StartCoroutine(CO_MoveElevator(speed));
    }

    private IEnumerator CO_MoveElevator(float speed)
    {
        while (true)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
            if (transform.position.y >= tpDistance || transform.position.y <= -tpDistance)
                transform.position = Vector3.zero;
            yield return null;
        }
    }
}
