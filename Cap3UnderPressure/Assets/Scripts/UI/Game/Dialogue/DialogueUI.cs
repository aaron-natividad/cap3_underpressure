using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogueUI : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip textSFX;
    [SerializeField] private AudioClip personSFX;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private Image speakerBox;
    [SerializeField] private TextMeshProUGUI dialogue;

    [Header("Parameters")]
    [SerializeField] private float dialogueDuration;

    [HideInInspector] public bool inAnimation;

    private CanvasGroup mainGroup;
    private AudioSource audioSource;
    private bool isEnabled;

    private void Awake()
    {
        mainGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetEnabled(bool isEnabled)
    {
        mainGroup.alpha = isEnabled ? 1 : 0;
        this.isEnabled = isEnabled;
    }

    public bool GetEnabled()
    {
        return isEnabled;
    }

    public void UpdateText(DialogueItem item)
    {
        SetEnabled(true);
        speakerName.text = item.speakerName;
        speakerBox.enabled = !(speakerName.text == "" || speakerName.text == null);
        dialogue.text = item.dialogueText;
        PlaySound(item.speakerType);
        StartCoroutine(CO_TextFade(item.lookTime));
    }

    private void PlaySound(SpeakerType type)
    {
        if (type == SpeakerType.Text)
            StartCoroutine(CO_TextSFX());
        else if (type == SpeakerType.Person)
            StartCoroutine(CO_PersonSFX(0.13f, Random.Range(3,5)));
    }

    private IEnumerator CO_TextSFX()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(textSFX);
        yield return null;
    }

    private IEnumerator CO_PersonSFX(float delay, int count)
    {
        for (int i = 0; i < count; i++)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(personSFX);
            yield return new WaitForSeconds(delay + Random.Range(0,0.03f));
        }
    }

    private IEnumerator CO_TextFade(float lookTime)
    {
        inAnimation = true;
        dialogue.alpha = 0f;
        StartCoroutine(GenAnim.Fade(dialogue, 1, 0.25f));
        yield return new WaitForSeconds(Mathf.Max(lookTime, dialogueDuration));
        inAnimation = false;
    }
}
