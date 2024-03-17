using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : Machine
{
    [SerializeField] private AudioClip dispenserSound;
    [Space(10)]
    [SerializeField] private GameObject itemPrefab;

    [Header("Dispenser Particles")]
    [SerializeField] private ParticleSystem dustParticles;

    [Header("Dispenser Objects")]
    [SerializeField] private GameObject dispenserHead;
    [SerializeField] private Transform itemSpawnpoint;

    [Header("Dispenser Parameters")]
    [SerializeField] private float dispenserRecoilDistance;

    private MaterialAnimator animator;
    private Vector3 dispenserLocation;

    protected override void Initialize()
    {
        base.Initialize();
        dispenserLocation = dispenserHead.transform.position;
        animator = GetComponent<MaterialAnimator>();
    }

    public override void Interact(Player pc)
    {
        return;
    }

    public override IEnumerator PerformAction()
    {
        state = MachineState.Performing;

        animator.StartCoroutine(animator.PlayAnimationOnce("open"));
        yield return new WaitForSeconds(0.1f);
        if (itemPrefab != null) Instantiate(itemPrefab, itemSpawnpoint.position, Quaternion.identity);
        audioSource.PlayOneShot(dispenserSound);
        LeanTween.move(dispenserHead, dispenserLocation + new Vector3(0, dispenserRecoilDistance, 0), 0.05f);
        dustParticles.Play();
        yield return new WaitForSeconds(0.1f);

        animator.StartCoroutine(animator.PlayAnimationOnce("close"));
        LeanTween.move(dispenserHead, dispenserLocation, 0.8f).setEase(LeanTweenType.easeOutCubic);
        yield return new WaitForSeconds(0.8f);
        state = MachineState.Normal;
    }
}
