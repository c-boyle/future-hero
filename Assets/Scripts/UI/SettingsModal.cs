using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsModal : BaseModal {

  [SerializeField] private Slider volumeSlider;
  [SerializeField] private Slider sensitivitySlider;
  [SerializeField] private Toggle inGameTimerToggle;
  [SerializeField] private Button backButton;
  [SerializeField] private TimeInLevelTextView timeInLevelTextView;
  [SerializeField] private AudioMixer gameAudioMixer;

  private const string MASTER_VOLUME = "master_volume";
  private const string SENSITIVITY = "sensitivity";
  private const string IN_GAME_TIMER = "in_game_timer";

  private void Start() {
    volumeSlider.value = PlayerPrefs.GetFloat(MASTER_VOLUME, -8f);
    sensitivitySlider.value = PlayerPrefs.GetFloat(SENSITIVITY, 2.4f);
    inGameTimerToggle.isOn = PlayerPrefs.GetInt(IN_GAME_TIMER, 0) == 1;

    volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    inGameTimerToggle.onValueChanged.AddListener(OnInGameTimerToggleChanged);
    backButton.onClick.AddListener(OnBackPressed);
  }

  private void OnVolumeChanged(float newValue) {
    if (newValue <= volumeSlider.minValue) {
      newValue = -80f;
    }
    gameAudioMixer.SetFloat(MASTER_VOLUME, newValue);
    PlayerPrefs.SetFloat(MASTER_VOLUME, newValue);
  }

  private void OnSensitivityChanged(float newValue) {
    PlayerPrefs.SetFloat(SENSITIVITY, newValue);
  }

  private void OnInGameTimerToggleChanged(bool newValue) {
    PlayerPrefs.SetInt(IN_GAME_TIMER, newValue ? 1 : 0);
    timeInLevelTextView.gameObject.SetActive(newValue);
  }

  private void OnBackPressed() {
    PlayerPrefs.Save();
    CloseAll();
  }

  protected override void OnCancel(InputAction.CallbackContext ctx) {
    PlayerPrefs.Save();
    base.OnCancel(ctx);
  }
}
