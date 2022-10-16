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
        digits.text = seconds.ToString("D2") + ":" + minutes.ToString("D2");
    }
}
