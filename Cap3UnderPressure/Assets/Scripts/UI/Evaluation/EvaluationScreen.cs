using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EvaluationScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] texts;

    [HideInInspector] public bool isEnabled;

    public void SetEnabled(bool isEnabled)
    {
        this.isEnabled = isEnabled;
        foreach (TextMeshProUGUI t in texts)
            t.enabled = isEnabled;
    }

    public void ShowEvaluation()
    {
        StartCoroutine(CO_ShowEvaluation());
    }

    private IEnumerator CO_ShowEvaluation()
    {
        StartCoroutine(GenAnim.PlayTextByLetter(texts[0], "Evaluating...", 0.05f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(GenAnim.PlayTextByLetter(texts[1], "Required Deliveries: " + DataManager.instance.previousQuota.ToString(), 0.02f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(GenAnim.PlayTextByLetter(texts[2], "Bots Delivered: " + DataManager.instance.previousScore.ToString(), 0.02f));
        yield return new WaitForSeconds(1f);
    }
}
