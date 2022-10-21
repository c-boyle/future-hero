using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerTextView : MonoBehaviour {

  [SerializeField] private TMP_Text timerText;

  private const float timeRemainingForFlashingText = 20f;
  private const float tickTime = 0.25f;
  private float tickTimer = 0;
  private bool tickBool = false;

  private void Start() {
    LevelTimer.LevelEnd += OnLevelEnd;
    LevelTimer.TimerUpdated += OnLevelTimerUpdate;
  }

  private void OnDestroy() {
    LevelTimer.LevelEnd -= OnLevelEnd;
    LevelTimer.TimerUpdated -= OnLevelTimerUpdate;
  }

  private void OnLevelEnd(object sender, EventArgs e) {
    gameObject.SetActive(false);
  }

  private void OnLevelTimerUpdate(object sender, LevelTimer.TimerUpdateEventArgs e) {
    if (tickTimer <= 0f) {
      var timeLeft = new TimeSpan(0, 0, (int)e.SecondsLeft);
      timerText.text = "Time Until Future Event: " + timeLeft.ToString("g").Substring(3);
      tickTimer = tickTime;
      if (e.SecondsLeft <= timeRemainingForFlashingText) {
        HandleFlashingText(e);
      } else {
        timerText.color = Color.red;
      }
      tickBool = !tickBool;
    }
    tickTimer -= e.DeltaTime;
  }

  private void HandleFlashingText(LevelTimer.TimerUpdateEventArgs e) {
    if (tickBool || e.SecondsLeft <= timeRemainingForFlashingText / 2) {
      if (timerText.color == Color.red) {
        timerText.color = Color.yellow;
      } else {
        timerText.color = Color.red;
      }
    }
  }
}
