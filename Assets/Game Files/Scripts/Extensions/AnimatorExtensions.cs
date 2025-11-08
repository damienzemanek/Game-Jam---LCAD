using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class AnimatorExtensions 
{
    public static void PlayWithHook(this Animator animator, string statename, MonoBehaviour mono, Action postHook)
        => mono.StartCoroutine(C_PlayWithHook(animator, statename, mono, postHook));

    public static IEnumerator C_PlayWithHook(this Animator animator, string statename, MonoBehaviour mono, Action postHook)
    {
        animator.Play(statename);

        yield return null;

        AnimatorStateInfo animInfo = animator.GetCurrentAnimatorStateInfo(0);
        float duration = animInfo.length / animator.speed;

        yield return new WaitForSeconds(duration);

        postHook?.Invoke();
    }
}
