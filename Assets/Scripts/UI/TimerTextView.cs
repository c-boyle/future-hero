using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerTextView : MonoBehaviour {

  [SerializeField] private TMP_Text timerText;
  [SerializeField] private LevelTimer timer;

  private const float timeRemainingForFlashingText = 20f;
  private const float tickTime = 0.25f;
  private float tickTimer = 0
  ;
  private bool tickBool = false;

  // Update is called once per frame
  void Update() {
    if (tickTimer <= 0f) {
      var timeLeft = new TimeSpan(0, 0, (int)timer.SecondsLeft);
      timerText.text = "Time Until Future Event: " + timeLeft.ToString("g").Substring(3);
      tickTimer = tickTime;
      if (timer.SecondsLeft <= timeRemainingForFlashingText) {
        HandleFlashingText();
      } else {
        timerText.color = Color.red;
      }
      tickBool = !tickBool;
    }
    tickTimer -= Time.deltaTime;
  }

  private void HandleFlashingText() {
    if (tickBool || timer.SecondsLeft <= timeRemainingForFlashingText / 2) {
      if (timerText.color == Color.red) {
        timerText.color = Color.yellow;
      } else {
        timerText.color = Color.red;
      }
    }
  }
}
