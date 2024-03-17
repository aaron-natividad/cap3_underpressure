using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SymptomInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Color effectColor;

    [HideInInspector] public SymptomScreen manager;
    private CanvasGroup cg;
    private Symptom storedSymptom;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0f;
    }

    public void UpdateInfo(Symptom symptom)
    {
        storedSymptom = symptom;
        icon.sprite = symptom.icon;
        title.text = symptom.id;
        description.text = symptom.description + 
            "\n\nEffect:\n<color=#" + effectColor.ToHexString() + ">" 
            + symptom.effect + "</color>";
    }

    public void SelectSymptom()
    {
        manager.UpdateViewedSymptom(storedSymptom);
        manager.MoveScreen(0);
    }

    public void Spawn()
    {
        StartCoroutine(CO_SpawnAnim(1f, 0f, 0.5f));
    }

    public void Despawn()
    {
        StartCoroutine(CO_SpawnAnim(0f, -50f, 0.5f));
    }

    private IEnumerator CO_SpawnAnim(float toAlpha, float moveDistance, float duration)
    {
        LeanTween.moveLocalY(gameObject, moveDistance, duration).setEase(LeanTweenType.easeOutCubic);
        StartCoroutine(GenAnim.Fade(cg, toAlpha, duration));
        yield return new WaitForSeconds(duration);
        if (toAlpha <= 0) Destroy(gameObject);
    }
}
