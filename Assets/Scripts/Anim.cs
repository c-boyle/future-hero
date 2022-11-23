using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    bool played = false;

    public void PlayAnimation(){ 
        Animation animate = GetComponent<Animation>();
        if (!played) {
            animate.Play();
            StartCoroutine(TriggerAfterAnimation(animate));
            played = true;
        }
    }

    protected virtual IEnumerator TriggerAfterAnimation(Animation animate) {
        Debug.Log("animating...");
        return null;
    }
}
