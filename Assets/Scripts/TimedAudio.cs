using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedAudio : MonoBehaviour {
  [SerializeField] private AudioSource audioSource;
  [SerializeField] private LevelTimer timer;
  [SerializeField] private float secondsLeftForPitchChange = 40f;

  [SerializeField] private float startPitch = 1f;
  [SerializeField] private float endPitch = 1.5f;

  private void Update() {
    if (timer.SecondsLeft <= secondsLeftForPitchChange) {
      float stepSize = (endPitch - startPitch) / secondsLeftForPitchChange;
      audioSource.pitch += stepSize * Time.deltaTime;
    } else {
      audioSource.pitch = startPitch;
    }
  }
}
