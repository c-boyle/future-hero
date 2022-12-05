using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuModal : BaseModal {

  [SerializeField] private Button playButton;
  [SerializeField] private Button controlsButton;
  [SerializeField] private Button settingsButton;
  [SerializeField] private Button quitButton;
  [SerializeField] private ControlsModal controlsModal;
  [SerializeField] private SettingsModal settingsModal;

  // Start is called before the first frame update
  void Start() {
    playButton.onClick.AddListener(OnPlayPressed);
    controlsButton.onClick.AddListener(OnControlsPressed);
    settingsButton.onClick.AddListener(OnSettingsPressed);
    quitButton.onClick.AddListener(OnQuitPressed);
    PlayerInput.Controls = new();
    PlayerInput.Controls.UI.Enable();
    Open();
  }

  protected override void OnCancel(UnityEngine.InputSystem.InputAction.CallbackContext ctx) { }

  private void OnPlayPressed() {
    CloseAll();
    SettingsInitializer.Instance.IsTutorial = true;
    SettingsInitializer.Instance.GlowHelp = true;
    PlayerInput.Controls = null;
    SceneManager.LoadScene("Level1");
  }

  private void OnControlsPressed() {
    OpenSubModal(controlsModal);
  }

  private void OnSettingsPressed() {
    OpenSubModal(settingsModal);
  }

  private void OnQuitPressed() {
#if UNITY_EDITOR
    EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif

  }
}
