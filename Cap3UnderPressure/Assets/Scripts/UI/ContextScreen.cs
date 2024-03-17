using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContextScreen : MonoBehaviour
{
    [SerializeField] private SceneHandler sceneHandler;
    [SerializeField] private IntroGroup introGroup;

    [Header("Components")]
    [SerializeField] private Image introPanel;
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private GameObject introButton;

    [Header("Parameters")]
    [SerializeField] private float introDelay;

    private bool inAnimation;
    private int itemIndex;

    private void OnEnable()
    {
        SceneHandler.OnSceneReady += InitializeIntro;
        GenAnim.OnAnimationStateChanged += ChangeAnimationState;
    }

    private void OnDisable()
    {
        SceneHandler.OnSceneReady -= InitializeIntro;
        GenAnim.OnAnimationStateChanged -= ChangeAnimationState;
    }

    public void SetEnabled(bool isEnabled)
    {
        introText.text = isEnabled ? introText.text : "";
        introText.enabled = isEnabled;
    }

    private void InitializeIntro()
    {
        itemIndex = 0;
        StartCoroutine(GenAnim.PlayText(introText, introGroup.introItems[itemIndex].text, introDelay));
        Cursor.lockState = CursorLockMode.None;
    }

    public void MoveToNextItem()
    {
        if (inAnimation) return;

        itemIndex++;
        if (itemIndex < introGroup.introItems.Length)
        {
            UpdatePanel(introGroup.introItems[itemIndex].panel, 0.25f);
            StartCoroutine(GenAnim.PlayText(introText, introGroup.introItems[itemIndex].text, introDelay));
        }
        else
        {
            introButton.SetActive(false);
            sceneHandler.LoadNextScene();
        }
    }

    private void UpdatePanel(Sprite currentPanel, float duration)
    {
        introPanel.color = new Color(1f, 1f, 1f, 0f);
        introPanel.sprite = currentPanel;
        StartCoroutine(GenAnim.Fade(introPanel, 1f, duration));
    }

    private void ChangeAnimationState(bool animState)
    {
        inAnimation = animState;
    }
}
