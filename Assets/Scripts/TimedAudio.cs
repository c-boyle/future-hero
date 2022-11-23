using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedAudio : MonoBehaviour {
  [SerializeField] private AudioSource audioSource;
  [SerializeField] private float secondsLeftForPitchChange = 40f;
  [SerializeField] private float endPitch = 1.5f;

  private float startPitch;
  

  private void Start() {
    startPitch = audioSource.pitch;
  }

  private void Update() {
    if (LevelTimer.Instance.SecondsLeft <= secondsLeftForPitchChange) {
      float pitchDiff = endPitch - startPitch;
      float pitchChangePercent = 1f - (LevelTimer.Instance.SecondsLeft / secondsLeftForPitchChange);
      audioSource.pitch = startPitch + (pitchDiff * pitchChangePercent);
    } else {
      if (audioSource.pitch != startPitch) {
        var diffBefore = Mathf.Abs(startPitch - audioSource.pitch);
        audioSource.pitch += 0.0005f * Mathf.Sign(startPitch - audioSource.pitch);
        var diffAfter = Mathf.Abs(startPitch - audioSource.pitch);
        if (diffBefore <= diffAfter) {
          audioSource.pitch = startPitch;
        }
      }
    }
  }
}
