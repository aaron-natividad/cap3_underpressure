using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBox : MonoBehaviour
{
    [SerializeField] private Cover cover;
    [SerializeField] private RecoveryStationGroup recoveryStationGroup;
    [Space(10)]
    [SerializeField] private Vector3 respawnPos = new Vector3(0f, 1f, 0f);

    private void OnTriggerEnter(Collider other)
    {
        // If player falls
        Player p = other.GetComponent<Player>();
        if (p != null)
        {
            StartCoroutine(Teleport(p));
            return;
        }

        // If item falls
        Item item = other.GetComponent<Item>();
        if (item != null)
        {
            recoveryStationGroup.TakeFallenItem(item);
            return;
        }
    }

    private IEnumerator Teleport(Player p)
    {
        StartCoroutine(cover.CoverScreen(0.25f,0f));
        yield return new WaitForSeconds(0.25f);

        recoveryStationGroup.TakePlayerItem();
        p.transform.position = respawnPos;
        StartCoroutine(cover.UncoverScreen(0.25f,0f));
    }
}
