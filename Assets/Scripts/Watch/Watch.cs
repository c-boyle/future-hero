using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Watch : MonoBehaviour
{
    protected int futureSeconds = 0;
    protected int futureMinutes = 0;

    protected int presentSeconds = 0;
    protected int presentMinutes = 0;

    protected const int minuteLength = 60;
    protected bool isFuture = false;
   

    private void Start() {
        LevelTimer.TimerUpdated += SetPresent;
    }

    // Update is called once per frame
    void Update() {
        UpdateWatch();
    }

    private void SetPresent(object sender, LevelTimer.TimerUpdateEventArgs e) {
        int secondsLeft = (int)e.SecondsLeft;
        presentSeconds = (int)Math.Floor((double)(secondsLeft/minuteLength));
        presentMinutes = secondsLeft%minuteLength;
    }

    protected virtual void UpdateWatch() {
        return;
    }

    public void toggleFutureTime(bool timeVision) {
        isFuture = timeVision;
    }
}
