using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureSeer : MonoBehaviour {

  [SerializeField] private GameObject presentObjectsRoot;

  [SerializeField] private List<GameObject> futureObjectsRoots = new List<GameObject>();

  [SerializeField] private int currentFutureIndex = 0;

  [SerializeField] private bool timeVisionEnabled = false;

  public void ToggleFutureVision() {
    if (timeVisionEnabled) {
      futureObjectsRoots[currentFutureIndex].SetActive(false);
      presentObjectsRoot.SetActive(true);
    } else {
      futureObjectsRoots[currentFutureIndex].SetActive(true);
      presentObjectsRoot.SetActive(false);
    }
    timeVisionEnabled = !timeVisionEnabled;
  }

  public void SetFutureScene(int index) {
    if (index < 0 || index >= futureObjectsRoots.Count) {
      Debug.LogError("Index out of bounds");
      return;
    }
    futureObjectsRoots[currentFutureIndex].SetActive(false);
    futureObjectsRoots[index].SetActive(true);
    currentFutureIndex = index;
  }
}
