using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SymptomScreen : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip typewriterSound;

    [Header("Prefabs")]
    [SerializeField] private GameObject symptomIntroPrefab;

    [Header("Groups")]
    [SerializeField] private Canvas symptomScreen;
    [SerializeField] private SymptomGroup symptomGroup;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject playButton;

    private SymptomInfo introPanel;
    private Symptom viewedSymptom;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        title.enabled = false;
        continueButton.SetActive(false);
        playButton.SetActive(false);
    }

    private void Start()
    {
        if (DataManager.instance.currentSymptoms.Count <= 0)
            SceneManager.LoadScene(DataManager.instance.queuedScene);
        StartCoroutine(Intro());
    }

    public void DoOutro()
    {
        StartCoroutine(Outro());
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }

    public void UpdateViewedSymptom(Symptom symptom)
    {
        viewedSymptom = symptom;
    }

    public void MoveScreen(int screenIndex)
    {
        switch (screenIndex)
        {
            case 0:
                StartCoroutine(CO_SymptomIntro());
                break;
            case 1:
                StartCoroutine(CO_AllSymptoms());
                break;
        }
    }

    private IEnumerator CO_SymptomIntro()
    {
        symptomGroup.DespawnSymptomInfo();
        playButton.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        SpawnIntroPanel();
        yield return new WaitForSeconds(0.5f);
        continueButton.SetActive(true);
    }

    private IEnumerator CO_AllSymptoms()
    {
        DespawnIntroPanel();
        continueButton.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        symptomGroup.SpawnSymptomInfo(1f, 0.25f);
        yield return new WaitForSeconds(0.5f);
        playButton.SetActive(true);
    }
    private IEnumerator Intro()
    {
        yield return new WaitForSeconds(0.5f);
        title.enabled = true;
        audioSource.PlayOneShot(typewriterSound);
        viewedSymptom = DataManager.instance.currentSymptoms[DataManager.instance.currentSymptoms.Count - 1];
        StartCoroutine(CO_SymptomIntro());
    }

    private IEnumerator Outro()
    {
        title.enabled = false;
        symptomGroup.DespawnSymptomInfo();
        playButton.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(DataManager.instance.queuedScene);
    }

    public void SpawnIntroPanel()
    {
        introPanel = Instantiate(symptomIntroPrefab, symptomScreen.transform).GetComponent<SymptomInfo>();
        introPanel.transform.localPosition = Vector3.zero;
        introPanel.UpdateInfo(viewedSymptom);
        introPanel.Spawn();
    }

    public void DespawnIntroPanel()
    {
        audioSource.PlayOneShot(typewriterSound);
        if (introPanel == null) return;
        introPanel.Despawn();
        introPanel = null;
    }
}
