using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpObject;

    [HideInInspector] public bool isEnabled;

    public void SetEnabled(bool isEnabled)
    {
        this.isEnabled = isEnabled;
        tmpObject.enabled = isEnabled;
    }

    public void ShowResult(bool passed)
    {
        tmpObject.text = passed ? "PASSED" : "FAILED";
    }
}
