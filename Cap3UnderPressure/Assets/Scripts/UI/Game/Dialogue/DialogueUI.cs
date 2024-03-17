using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI dialogue;
    [SerializeField] private float dialogueDelay;

    [HideInInspector] public bool inAnimation;

    private CanvasGroup mainGroup;
    private AudioSource audioSource;
    private bool isEnabled;

    private void OnEnable()
    {
        GenAnim.OnAnimationStateChanged += ChangeAnimationState;
    }

    private void OnDisable()
    {
        GenAnim.OnAnimationStateChanged -= ChangeAnimationState;
    }

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

    public void UpdateText(string name, string text, AudioClip clip = null)
    {
        speakerName.text = name;
        StartCoroutine(GenAnim.PlayText(dialogue, text, dialogueDelay, audioSource, clip));
    }

    private void ChangeAnimationState(bool animState)
    {
        inAnimation = animState;
    }
}
