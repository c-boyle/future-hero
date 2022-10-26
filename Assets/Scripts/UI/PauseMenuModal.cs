using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenuModal : BaseModal {
  [SerializeField] private Button continueButton;
  [SerializeField] private Button controlsButton;
  [SerializeField] private Button restartButton;
  [SerializeField] private Button exitButton;
  [SerializeField] private BaseModal controlsModal;

  private void Start() {
    continueButton.onClick.AddListener(OnContinuePressed);
    controlsButton.onClick.AddListener(OnControlsPressed);
    restartButton.onClick.AddListener(OnRestartPressed);
    exitButton.onClick.AddListener(OnExitPressed);
  }

  private void OnContinuePressed() {
    CloseAll();
  }

  private void OnControlsPressed() {
    OpenSubModal(controlsModal);
  }

  private void OnRestartPressed() {
    CloseAll();
    Helpers.ResetScene();
  }

  private void OnExitPressed() {
#if UNITY_EDITOR
    EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif

  }
}
