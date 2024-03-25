using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        float x = transform.position.x - Player.instance.transform.position.x;
        float z = transform.position.z - Player.instance.transform.position.z;
        float angle = Mathf.Atan2(x, z) * Mathf.Rad2Deg + 180;
        rotation.x = 45f;
        rotation.y = angle;
        transform.rotation = Quaternion.Euler(rotation);
    }
}
