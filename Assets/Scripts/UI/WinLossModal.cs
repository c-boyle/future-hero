using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinLossModal : BaseModal {
  [SerializeField] private Button restartButton;
  [SerializeField] private Button mainMenuButton;
  [SerializeField] private TMP_Text completionTimeText;

  private const string COMPLETION_TIME_TEXT = "COMPLETED IN: ";

  private void Start() {
    restartButton.onClick.AddListener(OnRestartPressed);
    mainMenuButton.onClick.AddListener(OnMainMenuPressed);
  }

  public override void Open() {
    base.Open();
    if (completionTimeText != null) {
      var completionTime = new TimeSpan(0, 0, (int)LevelTimer.SecondsSpentInLevel);
      completionTimeText.text = COMPLETION_TIME_TEXT + completionTime.ToString("g").Substring(3);
    }
  }

  protected override void OnCancel(InputAction.CallbackContext ctx) { }

  private void OnRestartPressed() {
    CloseAll();
    Helpers.ResetScene();
  }

  private void OnMainMenuPressed() {
    CloseAll();
    SceneManager.LoadScene("MainMenu");
  }
}
