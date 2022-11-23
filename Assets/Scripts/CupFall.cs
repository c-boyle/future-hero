using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupFall : Anim
{
    [SerializeField] GameObject puddle;
    // Start is called before the first frame update
    protected override IEnumerator TriggerAfterAnimation(Animation animate) {
        while(animate.isPlaying) yield return null;

        yield return new WaitForSeconds(0.2f);

        if (puddle != null) puddle.SetActive(true);
        gameObject.SetActive(false);
    }
}
