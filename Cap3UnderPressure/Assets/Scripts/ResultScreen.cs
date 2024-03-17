using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpObject;

    public void ShowResult(bool passed)
    {
        tmpObject.text = passed ? "PASSED" : "FAILED";
    }
}
