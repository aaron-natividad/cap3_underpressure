using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class MaterialAnimation
{
    public string name;
    public float frameRate;
    [Space(10)]
    public List<Texture2D> frames;
}

public class MaterialAnimator : MonoBehaviour
{
    [SerializeField] private Renderer rend;
    [SerializeField] private MaterialAnimation[] animations;

    private int animationIndex = 0;

    private void Start()
    {
        rend.material = new Material(rend.material);
    }

    public void StopAnimation()
    {
        StopAllCoroutines();
    }

    public void SwitchAnimation(string animationName)
    {
        for (int i = 0; i < animations.Length; i++)
        {
            if (animations[i].name == animationName)
            {
                animationIndex = i;
                return;
            }
        }
    }

    public IEnumerator LoopAnimation(string animationName)
    {
        SwitchAnimation(animationName);
        int frameIndex = 0;
        MaterialAnimation curAnim = animations[animationIndex];

        while (true)
        {
            rend.material.SetTexture("_BaseMap", curAnim.frames[frameIndex]);
            yield return new WaitForSeconds(1f / curAnim.frameRate);
            frameIndex = (frameIndex + 1) % curAnim.frames.Count;
        }
    }

    public IEnumerator PlayAnimationOnce(string animationName)
    {
        SwitchAnimation(animationName);
        MaterialAnimation curAnim = animations[animationIndex];

        for (int i = 0; i < curAnim.frames.Count; i++)
        {
            rend.material.SetTexture("_BaseMap", curAnim.frames[i]);
            yield return new WaitForSeconds(1f / curAnim.frameRate);
        }
    }
}
