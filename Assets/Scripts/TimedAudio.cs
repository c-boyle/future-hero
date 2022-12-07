using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class TimedAudio : MonoBehaviour {
  [SerializeField] private AudioSource audioSource;
  [SerializeField] private float secondsLeftForChanges = 40f;
  [SerializeField] private bool playWhenUnderSecondsLeft = true;
  [SerializeField] private bool changePitch = true;
  [SerializeField][ConditionalField(nameof(changePitch))] private float endPitch = 1.5f;

  private float startPitch;


  private void Start() {
    startPitch = audioSource.pitch;
  }

  private void Update() {
    audioSource.mute = Time.deltaTime == 0f;
    if (LevelTimer.Instance.SecondsLeft <= secondsLeftForChanges) {
      if (changePitch) {
        float pitchDiff = endPitch - startPitch;
        float pitchChangePercent = 1f - (LevelTimer.Instance.SecondsLeft / secondsLeftForChanges);
        audioSource.pitch = startPitch + (pitchDiff * pitchChangePercent);
      }
      if (playWhenUnderSecondsLeft) {
        if (audioSource.volume == 0) {
          audioSource.Play();
        }
        if (audioSource.volume <= 1) {
          audioSource.volume += 0.1f;
        }
      }
    } else {
      if (changePitch && audioSource.pitch != startPitch) {
        var diffBefore = Mathf.Abs(startPitch - audioSource.pitch);
        audioSource.pitch += 0.0005f * Mathf.Sign(startPitch - audioSource.pitch);
        var diffAfter = Mathf.Abs(startPitch - audioSource.pitch);
        if (diffBefore <= diffAfter) {
          audioSource.pitch = startPitch;
        }
      }
      if (playWhenUnderSecondsLeft) {
        audioSource.Stop();
        audioSource.volume = 0;
      }
    }
  }
}
