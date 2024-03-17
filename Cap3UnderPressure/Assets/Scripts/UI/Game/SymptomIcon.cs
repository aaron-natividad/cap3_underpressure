using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SymptomIcon : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI counter;

    public void UpdateIcon(Sprite sprite)
    {
        icon.sprite = sprite;
        icon.color = Color.white;
    }

    public void UpdateCounter(int count)
    {
        counter.text = count.ToString("00");
    }

    public void DisableIcon()
    {
        icon.enabled = false;
        counter.enabled = false;
    }
}
