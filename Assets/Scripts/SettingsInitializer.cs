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

  void Start() {
    gameAudioMixer.SetFloat(MASTER_VOLUME, PlayerPrefs.GetFloat(MASTER_VOLUME, -8f));
    PlayerPrefs.GetFloat(SENSITIVITY, 2.4f);
    timeInLevelTextView.gameObject.SetActive(PlayerPrefs.GetInt(IN_GAME_TIMER, 0) == 1);
  }

}
