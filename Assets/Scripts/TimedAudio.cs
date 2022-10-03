using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedAudio : MonoBehaviour {
  [SerializeField] private AudioSource audioSource;
  [SerializeField] private LevelTimer timer;
  [SerializeField] private float secondsLeftForPitchChange = 40f;

  private float startPitch;
  [SerializeField] private float endPitch = 1.5f;

  private void Start() {
    startPitch = audioSource.pitch;
  }

  private void Update() {
    if (timer.SecondsLeft <= secondsLeftForPitchChange) {
      float stepSize = (endPitch - startPitch) / secondsLeftForPitchChange;
      audioSource.pitch += stepSize * Time.deltaTime;
    } else {
      if (audioSource.pitch != startPitch) {
        var diffBefore = Mathf.Abs(startPitch - audioSource.pitch);
        audioSource.pitch += 0.0003f * Mathf.Sign(startPitch - audioSource.pitch);
        var diffAfter = Mathf.Abs(startPitch - audioSource.pitch);
        if (diffBefore <= diffAfter) {
          audioSource.pitch = startPitch;
        }
      }
    }
  }
}
