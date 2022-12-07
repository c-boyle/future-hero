using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioToggler : MonoBehaviour {
  [SerializeField] private List<AudioSource> sources = new();
  [SerializeField] private float timeToToggle;
  private readonly Dictionary<AudioSource, float> audioToOriginalVolume = new();

  private bool toggledOn = true;

  public void Toggle(bool on) {
    if (on == toggledOn) {
      return;
    }
    toggledOn = on;
    if (on) {
      foreach (var source in sources) {
        StartCoroutine(FadeInAudioSource(source));
      }
    } else {
      SetOriginalVolumes();
      foreach (var source in sources) {
        StartCoroutine(FadeOutAudioSource(source));
      }
    }
  }

  private IEnumerator FadeOutAudioSource(AudioSource source) {
    float step = -source.volume / timeToToggle;
    float stepTime = 0.1f;
    var waitForStep = new WaitForSeconds(stepTime);
    while (source.volume > 0 && !toggledOn) {
      yield return waitForStep;
      source.volume += step * stepTime;
    }
    source.mute = true;
  }

  private IEnumerator FadeInAudioSource(AudioSource source) {
    source.mute = false;
    float step = (audioToOriginalVolume[source] - source.volume) / timeToToggle;
    float stepTime = 0.1f;
    var waitForStep = new WaitForSeconds(stepTime);
    while (source.volume <= audioToOriginalVolume[source] + 0.01f && source.volume >= audioToOriginalVolume[source] - 0.01f && toggledOn) {
      yield return waitForStep;
      source.volume += step * stepTime;
    }
  }

  private void SetOriginalVolumes() {
    foreach (AudioSource source in sources) {
      audioToOriginalVolume[source] = source.volume;
    }
  }

}
