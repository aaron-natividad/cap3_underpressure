using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalPlatform : MonoBehaviour
{
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float speed;

    private void Update()
    {
        Vector3 currentPos = transform.position;

        if (currentPos.x < minDistance)
        {
            currentPos.x = maxDistance;
        }
        else if (currentPos.x > maxDistance)
        {
            currentPos.x = minDistance;
        }
        else
        {
            currentPos.x += speed * Time.deltaTime;
        }

        transform.position = currentPos;
    }
}
