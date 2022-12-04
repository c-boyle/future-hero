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
    volumeSlider.value =  SettingsInitializer.Instance.MasterVolume;
    sensitivitySlider.value =  SettingsInitializer.Instance.Sensitiviy;
    inGameTimerToggle.isOn = SettingsInitializer.Instance.InGameTimerEnabled;

    volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    inGameTimerToggle.onValueChanged.AddListener(OnInGameTimerToggleChanged);
    backButton.onClick.AddListener(OnBackPressed);
  }

  private void OnVolumeChanged(float newValue) {
    if (newValue <= volumeSlider.minValue) {
      newValue = -80f;
    }
    SettingsInitializer.Instance.MasterVolume = newValue;
  }

  private void OnSensitivityChanged(float newValue) {
    SettingsInitializer.Instance.Sensitiviy = newValue;
  }

  private void OnInGameTimerToggleChanged(bool newValue) {
    SettingsInitializer.Instance.InGameTimerEnabled = newValue;
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
