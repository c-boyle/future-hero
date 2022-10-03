using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour {
  [field: SerializeField] public float SecondsLeft { get; private set; } = 60f;

  // Update is called once per frame
  void Update() {
    SecondsLeft -= Time.deltaTime;
    if (SecondsLeft <= 0) {
      Helpers.ResetScene();
    }
  }

  public void SetSecondsLeft(float secondsLeft) {
    SecondsLeft = secondsLeft;
  }
}
