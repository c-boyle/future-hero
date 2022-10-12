using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogueWatch : Watch
{
    [SerializeField] Transform minuteHand;
    [SerializeField] Transform secondHand;

    protected override void UpdateWatch() {
        int minutes, seconds;
        if (isFuture) {
            minutes = futureMinutes;
            seconds = futureSeconds;
        } else {
            minutes = presentMinutes;
            seconds = presentSeconds;
        }

        SetHands(minutes, seconds);
        
    }

    private void SetHands(int minutes, int seconds) {
        //-- calculate pointer angles
        float rotationSeconds = (360.0f / (float)minuteLength) * seconds;
        float rotationMinutes = (360.0f / (float)minuteLength) * minutes;

        //-- draw pointers
        minuteHand.localEulerAngles = new Vector3(0.0f, 0.0f, rotationSeconds);
        secondHand.localEulerAngles = new Vector3(0.0f, 0.0f, rotationMinutes);
    }
}
