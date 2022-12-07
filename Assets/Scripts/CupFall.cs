using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupFall : Anim
{
    [SerializeField] GameObject puddle;
    [SerializeField] private AudioSource smashSound;
    // Start is called before the first frame update
    protected override IEnumerator TriggerAfterAnimation(Animation animate) {
        while(animate.isPlaying) yield return null;

        gameObject.SetActive(false);
        if (puddle != null) puddle.SetActive(true);
        smashSound.Play();
        // yield return new WaitForSeconds(0.2f);
        
    }
}
