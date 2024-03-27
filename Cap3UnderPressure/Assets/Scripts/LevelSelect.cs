using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private SymptomLoader symptomLoader;
    [SerializeField] private SceneHandler sceneHandler;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image screenshot;
    [Space(10)]
    [SerializeField] private LevelInfo[] levels;

    private CanvasGroup mainGroup;
    private string queuedLevel;
    private int levelIndex;

    private void Awake()
    {
        mainGroup = GetComponent<CanvasGroup>();
        levelIndex = 0;
        title.text = levels[0].title;
        screenshot.sprite = levels[0].screenshot;
        queuedLevel = levels[0].queuedLevel;
    }

    public void MoveLeft()
    {
        levelIndex--;
        if (levelIndex < 0)
        {
            levelIndex = levels.Length - 1;
        }
        else if (levelIndex >= levels.Length)
        {
            levelIndex = 0;
        }
        UpdateLevel();
    }

    public void MoveRight()
    {
        levelIndex++;
        if (levelIndex < 0)
        {
            levelIndex = levels.Length - 1;
        }
        else if (levelIndex >= levels.Length)
        {
            levelIndex = 0;
        }
        UpdateLevel();
    }

    public void PlayLevel()
    {
        symptomLoader.LoadSymptoms();
        mainGroup.interactable = false;
        mainGroup.blocksRaycasts = false;
        DataManager.instance.queuedScene = queuedLevel;
        sceneHandler.LoadScene("Symptom Screen");
    }

    private void UpdateLevel()
    {
        title.text = levels[levelIndex].title;
        screenshot.sprite = levels[levelIndex].screenshot;
        screenshot.color = new Color(1, 1, 1, 0);
        queuedLevel = levels[levelIndex].queuedLevel;
        StartCoroutine(GenAnim.Fade(screenshot, 1, 0.25f));
    }
}
