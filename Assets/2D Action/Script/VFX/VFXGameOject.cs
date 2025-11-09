using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class VFXGameOject : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private int layerIndex = 0;
    [SerializeField] private bool isDestroyAfteranimEnd;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (isDestroyAfteranimEnd) StartCoroutine(WaitForAnimEnd());
    }

    private IEnumerator WaitForAnimEnd()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
        // Wait one frame for state to settle
        yield return null;

        // Loop until normalizedTime hits 1.0+ (end of clip)
        while (stateInfo.normalizedTime < 1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
        }

        // Extra frame check for transitions—poof!
        yield return null;
        Destroy(gameObject);
    }
}
