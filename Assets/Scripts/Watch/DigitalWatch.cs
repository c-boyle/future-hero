using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DigitalWatch : Watch
{
    [SerializeField] TMP_Text digits;
    private int numJibberish = 4;

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

    protected override IEnumerator TransitionEffect() {
        notTransitioning = false;
        for(int i = 0; i < numJibberish; i++) {
            System.Random rnd = new System.Random();
            int jibbMin = rnd.Next(100);
            int jibbSec = rnd.Next(minuteLength);

            DisplayTime(jibbMin, jibbSec);
            yield return new WaitForSeconds(0.19f);;
        }
        notTransitioning = true;
    } 

    private void DisplayTime(int minutes, int seconds) {
        digits.text = seconds.ToString("D2") + ":" + minutes.ToString("D2");
    }
}
