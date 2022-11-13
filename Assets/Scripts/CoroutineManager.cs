using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class CoroutineManager : Singleton<CoroutineManager> {
  public void ScheduleCoroutine(IEnumerator enumerator) {
    StartCoroutine(enumerator);
  }
}
