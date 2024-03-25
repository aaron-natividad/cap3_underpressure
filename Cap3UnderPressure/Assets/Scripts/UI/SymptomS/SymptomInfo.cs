using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SymptomInfo : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private AudioClip clickSound;

    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Components")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;

    [Header("Parameters")]
    [SerializeField] private Color effectColor;

    [HideInInspector] public SymptomScreen manager;
    private CanvasGroup cg;
    private AudioSource audioSource;
    private Symptom storedSymptom;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
        cg.alpha = 0f;
    }

    public void UpdateInfo(Symptom symptom)
    {
        storedSymptom = symptom;
        icon.sprite = symptom.icon;
        if (videoPlayer != null) videoPlayer.clip = symptom.clip;
        title.text = symptom.id.ToUpper();
        description.text = symptom.description + 
            "\n\nEffect:\n<color=#" + effectColor.ToHexString() + ">" 
            + symptom.effect + "</color>";
    }

    public void SelectSymptom()
    {
        audioSource.PlayOneShot(clickSound);
        manager.UpdateViewedSymptom(storedSymptom);
        manager.MoveScreen(0);
    }

    public void Spawn()
    {
        audioSource.PlayOneShot(spawnSound);
        StartCoroutine(CO_SpawnAnim(1f, 0f, 0.5f));
    }

    public void Despawn()
    {
        StartCoroutine(CO_SpawnAnim(0f, -50f, 0.5f));
    }

    private IEnumerator CO_SpawnAnim(float toAlpha, float moveDistance, float duration)
    {
        cg.interactable = false;
        cg.blocksRaycasts = false;

        LeanTween.moveLocalY(gameObject, moveDistance, duration).setEase(LeanTweenType.easeOutCubic);
        StartCoroutine(GenAnim.Fade(cg, toAlpha, duration));
        yield return new WaitForSeconds(duration);

        cg.interactable = true;
        cg.blocksRaycasts = true;
        if (toAlpha <= 0) Destroy(gameObject);
    }
}
