using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private SceneHandler sceneHandler;
    [SerializeField] private ParticleSystem desertDust;

    [Header("Parameters")]
    [SerializeField] private float distance;
    [SerializeField] private float duration;
    [SerializeField] private float delay;
    
    [Header("Title Screen")]
    [SerializeField] private GameObject titleButton;
    [SerializeField] private CanvasGroup titleGroup;

    [Header("Menu Groups")]
    [SerializeField] private CanvasGroup mainMenuGroup;
    [SerializeField] private CanvasGroup settingsGroup;
    [SerializeField] private CanvasGroup creditsGroup;

    [Header("Background")]
    [SerializeField] private GameObject[] bgElements;

    [Header("Buttons")]
    [SerializeField] private Button[] mainMenuButtons;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void MoveToMainMenu()
    {
        StopAllCoroutines();
        StartCoroutine(CO_MainMenu());
    }

    public void MoveToTitleScreen()
    {
        StopAllCoroutines();
        StartCoroutine(CO_TitleScreen());
    }

    public void StartGame()
    {
        StopAllCoroutines();
        StartCoroutine(CO_StartGame());
    }

    public void FadeCanvasGroup(int groupIndex)
    {
        mainMenuGroup.gameObject.SetActive(false);
        settingsGroup.gameObject.SetActive(false);
        creditsGroup.gameObject.SetActive(false);
        mainMenuGroup.alpha = 0;
        settingsGroup.alpha = 0;
        creditsGroup.alpha = 0;

        switch (groupIndex)
        {
            case 0:
                mainMenuGroup.gameObject.SetActive(true);
                StartCoroutine(GenAnim.Fade(mainMenuGroup, 1, 0.25f));
                break;
            case 1:
                settingsGroup.gameObject.SetActive(true);
                StartCoroutine(GenAnim.Fade(settingsGroup, 1, 0.25f));
                break;
            case 2:
                creditsGroup.gameObject.SetActive(true);
                StartCoroutine(GenAnim.Fade(creditsGroup, 1, 0.25f));
                break;
        }
    }

    private IEnumerator CO_StartGame()
    {
        StartCoroutine(GenAnim.Fade(mainMenuGroup, 0, 0.5f));
        foreach (Button b in mainMenuButtons) b.enabled = false;
        yield return new WaitForSeconds(0.5f);
        sceneHandler.LoadScene(Constants.SCENE_INTRO);
    }

    private IEnumerator CO_MainMenu()
    {
        titleButton.SetActive(false);
        desertDust.Stop();

        StartCoroutine(GenAnim.Fade(titleGroup, 0, duration / 2));
        MoveElementsWithDelay(bgElements, distance);

        yield return new WaitForSeconds(duration);
        FadeCanvasGroup(0);
    }

    private IEnumerator CO_TitleScreen()
    {
        StartCoroutine(GenAnim.Fade(mainMenuGroup, 0, 0.25f));
        yield return new WaitForSeconds(0.25f);

        mainMenuGroup.gameObject.SetActive(false);
        MoveElementsWithDelay(bgElements, -distance);
        yield return new WaitForSeconds(duration);

        StartCoroutine(GenAnim.Fade(titleGroup, 1, duration / 2));
        titleButton.SetActive(true);
        desertDust.Play();
    }

    private void MoveElementsWithDelay(GameObject[] elements, float dist)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            LeanTween.moveLocal(
                elements[i], 
                elements[i].transform.localPosition + Vector3.up * dist, 
                duration + i * delay
                ).setEase(LeanTweenType.easeInOutCubic);
        }
    }
}
