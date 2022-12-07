using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallThrow : Anim
{
    [SerializeField] private UnityEvent afterAnimation;

    protected override IEnumerator TriggerAfterAnimation(Animation animate) {
        while(animate.isPlaying) yield return null;

        yield return new WaitForSeconds(0.2f);

        afterAnimation.Invoke();
    }
}
