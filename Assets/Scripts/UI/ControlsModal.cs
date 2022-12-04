using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsModal : BaseModal {

  [SerializeField] private Button gamepadButton;
  [SerializeField] private Button keyboardButton;
  [SerializeField] private Button backButton;
  [SerializeField] private GameObject gamepadControls;
  [SerializeField] private GameObject keyboardControls;

  void Start() {
    gamepadButton.onClick.AddListener(OnGamepadPressed);
    keyboardButton.onClick.AddListener(OnKeyboardPressed);
    backButton.onClick.AddListener(OnBackPressed);
  }

  public override void Open() {
    base.Open();
    if (PlayerInput.ControlType == ControlsPrompt.ControlType.Gamepad) {
      OnGamepadPressed();
      gamepadButton.Select();
    } else {
      OnKeyboardPressed();
      keyboardButton.Select();
    }
  }

  private void OnGamepadPressed() {
    gamepadControls.SetActive(true);
    keyboardControls.SetActive(false);
  }

  private void OnKeyboardPressed() {
    gamepadControls.SetActive(false);
    keyboardControls.SetActive(true);
  }

  private void OnBackPressed() {
    CloseAll();
  }
}
