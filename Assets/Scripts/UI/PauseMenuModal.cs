using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class PauseMenuModal : BaseModal {
  [SerializeField] private Button continueButton;
  [SerializeField] private Button controlsButton;
  [SerializeField] private Button exitButton;
  [SerializeField] private BaseModal controlsModal;

  private void Start() {
    continueButton.onClick.AddListener(OnContinuePressed);
    controlsButton.onClick.AddListener(OnControlsPressed);
    exitButton.onClick.AddListener(OnExitPressed);
  }

  private void OnContinuePressed() {
    CloseAll();
  }

  private void OnControlsPressed() {
    OpenSubModal(controlsModal);
  }

  private void OnExitPressed() {
    Application.Quit();
  }
}
