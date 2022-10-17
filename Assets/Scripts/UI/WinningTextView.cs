using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinningTextView : MonoBehaviour {

  [SerializeField] private TMP_Text winningText;
  
  void Start() {
    LevelTimer.LevelEnd += OnLevelEnd;
  }

  private void OnDestroy() {
    LevelTimer.LevelEnd -= OnLevelEnd;
  }

  private void OnLevelEnd(object sender, EventArgs e) {
    winningText.enabled = true;
  }
}
