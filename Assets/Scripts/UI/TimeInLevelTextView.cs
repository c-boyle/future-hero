using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeInLevelTextView : MonoBehaviour {

  [SerializeField] private TMP_Text timerText;

  private void Update() {
    var timeLeft = new TimeSpan(0, 0, (int)LevelTimer.SecondsSpentInLevel);
    timerText.text = timeLeft.ToString("g").Substring(3);
  }
}
