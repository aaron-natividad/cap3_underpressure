using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EvaluationScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] texts;

    public void PlayAnimation()
    {
        StartCoroutine(CO_PlayAnim());
    }

    private IEnumerator CO_PlayAnim()
    {
        StartCoroutine(GenAnim.PlayText(texts[0], "Evaluating...", 0.05f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(GenAnim.PlayText(texts[1], "Required Deliveries: " + DataManager.instance.previousQuota.ToString(), 0.02f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(GenAnim.PlayText(texts[2], "Bots Delivered: " + DataManager.instance.previousScore.ToString(), 0.02f));
        yield return new WaitForSeconds(1f);
    }
}
