using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DigitalWatch : Watch
{
    [SerializeField] TMP_Text digits;

    protected override void UpdateWatch() {
        int minutes, seconds;
        if (isFuture) {
            minutes = futureMinutes;
            seconds = futureSeconds;
        } else {
            minutes = presentMinutes;
            seconds = presentSeconds;
        }
        DisplayTime(minutes, seconds);
    }

    protected override IEnumerator TransitionTime() {
        while(!notTransitioning && (transitionTime < transitionMax)) {
            System.Random rnd = new System.Random();
            int jibbMin = rnd.Next(100);
            int jibbSec = rnd.Next(minuteLength);

            DisplayTime(jibbMin, jibbSec);
            yield return new WaitForSeconds(0.15f);
        }
    } 

    private void DisplayTime(int minutes, int seconds) {
        digits.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }
}
