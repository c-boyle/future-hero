using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinningTextView : MonoBehaviour {

  [SerializeField] private TMP_Text winningText;
  
  void Start() {
    LevelTimer.LevelWon += OnLevelWon;
  }

  private void OnDestroy() {
    LevelTimer.LevelWon -= OnLevelWon;
  }

  private void OnLevelWon(object sender, EventArgs e) {
    winningText.enabled = true;
  }
}
