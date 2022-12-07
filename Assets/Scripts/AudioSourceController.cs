using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class AudioSourceController : MonoBehaviour {
  [SerializeField] private AudioSource audioSource;
  [SerializeField] private bool playAfterDelay = false;
  [SerializeField][ConditionalField(nameof(playAfterDelay))] private float playDelay = 3f;
  [SerializeField] private bool loop = false;

  // Start is called before the first frame update
  void Start() {
    if (playAfterDelay) {
      StartCoroutine(PlayAfterDelay());
    }
  }

  private IEnumerator PlayAfterDelay() {
    var waitForClipAndPlayDelay = new WaitForSeconds(audioSource.clip.length + playDelay);
    var waitForPlayDelay = new WaitForSeconds(playDelay);
    yield return waitForPlayDelay;
    audioSource.Play();
    while (loop && (audioSource.enabled || (!audioSource.enabled && gameObject.activeSelf))) {
      yield return waitForClipAndPlayDelay;
      audioSource.Play();
    }
  }
}
