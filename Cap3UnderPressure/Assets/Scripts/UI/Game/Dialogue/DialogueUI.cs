using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private Image speakerBox;
    [SerializeField] private TextMeshProUGUI dialogue;
    [SerializeField] private float dialogueDelay;
    [SerializeField] private float dialogueSoundDelay;

    [HideInInspector] public bool inAnimation;

    private CanvasGroup mainGroup;
    private AudioSource audioSource;
    private AudioClip storedClip;
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
        StartCoroutine(PlayDialogueSound());
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
        speakerBox.enabled = !(speakerName.text == "" || speakerName.text == null);
        storedClip = clip;
        StartCoroutine(GenAnim.PlayTextByLetter(dialogue, text, dialogueDelay));
    }

    private void ChangeAnimationState(bool animState)
    {
        inAnimation = animState;
    }

    private IEnumerator PlayDialogueSound()
    {
        while (true)
        {
            if (inAnimation && storedClip != null)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(storedClip);
            }
            yield return new WaitForSeconds(dialogueSoundDelay);
        }
    }
}
