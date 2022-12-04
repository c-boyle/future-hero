using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.Audio;

public class SettingsInitializer : Singleton<SettingsInitializer> {

  [SerializeField] private AudioMixer gameAudioMixer;
  [SerializeField] private TimeInLevelTextView timeInLevelTextView;

  private const string MASTER_VOLUME = "master_volume";
  private const string SENSITIVITY = "sensitivity";
  private const string IN_GAME_TIMER = "in_game_timer";

  public float MasterVolume {
    get {
      return PlayerPrefs.GetFloat(MASTER_VOLUME, -8f);
    }
    set {
      PlayerPrefs.SetFloat(MASTER_VOLUME, value);
      gameAudioMixer.SetFloat(MASTER_VOLUME, value);
    }
  }

  public float Sensitiviy {
    get {
      return PlayerPrefs.GetFloat(SENSITIVITY, 2.4f);
    }
    set {
      PlayerPrefs.SetFloat(SENSITIVITY, value);
    }
  }

  public bool InGameTimerEnabled {
    get {
      return PlayerPrefs.GetInt(IN_GAME_TIMER, 0) == 1;
    }
    set {
      PlayerPrefs.SetInt(IN_GAME_TIMER, value ? 1 : 0);
      if (timeInLevelTextView != null) {
        timeInLevelTextView.gameObject.SetActive(value);
      }
    }
  }

  void Start() {
    gameAudioMixer.SetFloat(MASTER_VOLUME, MasterVolume);
    if (timeInLevelTextView != null) {
      timeInLevelTextView.gameObject.SetActive(InGameTimerEnabled);
    }
  }

  public void SetInGameTimerHidden(bool hidden) {
    if (hidden) {
      timeInLevelTextView.gameObject.SetActive(false);
    } else {
      timeInLevelTextView.gameObject.SetActive(InGameTimerEnabled);
    }
  }
}
