using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.Audio;

public class SettingsInitializer : Singleton<SettingsInitializer> {

  [SerializeField] private AudioMixer gameAudioMixer;

  private const string MASTER_VOLUME = "master_volume";

  void Start() {
    gameAudioMixer.SetFloat(MASTER_VOLUME, PlayerPrefs.GetFloat(MASTER_VOLUME, -8f));
  }
}
