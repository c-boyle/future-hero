using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureSeer : MonoBehaviour {

  [SerializeField] private GameObject presentObjectsRoot;

  [SerializeField] private GameObject futureObjectsRoot;

  [SerializeField] private bool timeVisionEnabled = false;

  public void ToggleFutureVision() {
    if (timeVisionEnabled) {
      futureObjectsRoot.SetActive(false);
      presentObjectsRoot.SetActive(true);
    } else {
      futureObjectsRoot.SetActive(true);
      presentObjectsRoot.SetActive(false);
    }
    timeVisionEnabled = !timeVisionEnabled;
  }

  public void SetFutureScene(GameObject futureObjectsRoot) {
    this.futureObjectsRoot.SetActive(false);
    futureObjectsRoot.SetActive(true);
    this.futureObjectsRoot = futureObjectsRoot;
  }
}
