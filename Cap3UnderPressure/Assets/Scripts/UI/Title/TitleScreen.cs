using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    const float SCREEN_HEIGHT = 1080f;

    [SerializeField] private SceneHandler sceneHandler;
    [SerializeField] private ParticleSystem desertDust;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    [Header("Parameters")]
    [SerializeField] private float animDuration;

    [Header("Canvas Groups")]
    [SerializeField] private TitleBackground bg;
    [SerializeField] private CanvasGroup titleGroup;
    [SerializeField] private CanvasGroup[] menuGroups;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void MoveToMainMenu()
    {
        StopAllCoroutines();
        StartCoroutine(CO_MoveToMainMenu());
    }

    public void MoveToTitleScreen()
    {
        StopAllCoroutines();
        StartCoroutine(CO_MoveToTitleScreen());
    }

    public void StartGame()
    {
        StopAllCoroutines();
        StartCoroutine(CO_StartGame());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Click()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(clickSound);
    }

    #region Menu Group Handlers
    public void SetMenuGroup(int groupIndex)
    {
        Click();
        SetGroup(groupIndex);
    }

    private void SetGroup(int groupIndex)
    {
        for (int i = 0; i < menuGroups.Length; i++)
        {
            SetInteraction(menuGroups[i], false);
            menuGroups[i].alpha = 0;

            if (i == groupIndex)
                StartCoroutine(FadeGroup(menuGroups[i]));
        }
    }

    private void SetInteraction(CanvasGroup group, bool isEnabled)
    {
        group.interactable = isEnabled;
        group.blocksRaycasts = isEnabled;
    }

    private IEnumerator FadeGroup(CanvasGroup group)
    {
        StartCoroutine(GenAnim.Fade(group, 1, 0.25f));
        yield return new WaitForSeconds(0.25f);
        SetInteraction(group, true);
    }
    #endregion

    #region Animations
    private IEnumerator CO_StartGame()
    {
        Click();
        SetInteraction(menuGroups[0], false);
        StartCoroutine(GenAnim.Fade(menuGroups[0], 0, 0.25f));
        yield return new WaitForSeconds(0.25f);
        sceneHandler.LoadScene(Constants.SCENE_INTRO);
    }

    private IEnumerator CO_MoveToMainMenu()
    {
        Click();
        desertDust.Stop();
        SetInteraction(titleGroup, false);
        StartCoroutine(GenAnim.Fade(titleGroup, 0, animDuration / 2));
        bg.Move(animDuration, SCREEN_HEIGHT * 2);
        yield return new WaitForSeconds(animDuration);
        SetGroup(0);
    }

    private IEnumerator CO_MoveToTitleScreen()
    {
        Click();
        SetInteraction(menuGroups[0], false);
        StartCoroutine(GenAnim.Fade(menuGroups[0], 0, 0.25f));
        yield return new WaitForSeconds(0.25f);

        bg.Move(animDuration, -SCREEN_HEIGHT * 2);
        yield return new WaitForSeconds(animDuration);

        StartCoroutine(GenAnim.Fade(titleGroup, 1, animDuration / 2));
        desertDust.Play();
        yield return new WaitForSeconds(animDuration / 2);
        SetInteraction(titleGroup, true);
    }
    #endregion
}
