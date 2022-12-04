using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenuModal : BaseModal {
  [SerializeField] private Button continueButton;
  [SerializeField] private Button controlsButton;
  [SerializeField] private Button settingsButton;
  [SerializeField] private Button restartButton;
  [SerializeField] private Button mainMenuButton;
  [SerializeField] private Button quitGameButton;
  [SerializeField] private BaseModal controlsModal;
  [SerializeField] private SettingsModal settingsModal;

  private void Start() {
    continueButton.onClick.AddListener(OnContinuePressed);
    controlsButton.onClick.AddListener(OnControlsPressed);
    settingsButton.onClick.AddListener(OnSettingsPressed);
    restartButton.onClick.AddListener(OnRestartPressed);
    mainMenuButton.onClick.AddListener(OnMainMenuPressed);
    quitGameButton.onClick.AddListener(OnExitPressed);
  }

  private void OnContinuePressed() {
    CloseAll();
  }

  private void OnControlsPressed() {
    OpenSubModal(controlsModal);
  }

  private void OnSettingsPressed() {
    OpenSubModal(settingsModal);
  }

  private void OnRestartPressed() {
    CloseAll();
    Helpers.ResetScene();
  }

  private void OnMainMenuPressed() {
    CloseAll();
    SceneManager.LoadScene("MainMenu");
  }

  private void OnExitPressed() {
#if UNITY_EDITOR
    EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif

  }
}
