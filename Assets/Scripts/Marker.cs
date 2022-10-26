using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour {

  [SerializeField] private Sprite icon;

  private const float markerDuration = 10f;

  private static Transform uiRoot = null;

  private static Camera mainCamera = null;

  // Start is called before the first frame update
  void Start() {
    if (uiRoot == null) {
      uiRoot = UIEventListener.Instance.transform;
    }
  }

  public void Activate() {
    GameObject markerObject = new("UIMarker for: " + name, typeof(Image));
    markerObject.transform.SetParent(uiRoot, false);
    StartCoroutine(DisplayMarker(markerObject.transform as RectTransform));
  }

  private IEnumerator DisplayMarker(RectTransform uiMarker) {
    if (mainCamera == null) {
      mainCamera = Camera.main;
    }
    uiMarker.GetComponent<Image>().sprite = icon;
    uiMarker.sizeDelta = icon.textureRect.max;
    float markerTimer = markerDuration;
    while (markerTimer >= 0) {
      var screenPoint = mainCamera.WorldToScreenPoint(transform.position);
      var rect = uiMarker.rect;
      Vector2 extents = rect.size / 2;
      //Debug.Log(screenPoint);
      float sign = Mathf.Sign(screenPoint.z);
      float xPos = Mathf.Clamp(sign * screenPoint.x, extents.x, Screen.width - extents.x);
      float yPos = Mathf.Clamp(sign * screenPoint.y, extents.y, Screen.height - extents.y);
      uiMarker.position = new(xPos, yPos, screenPoint.z);

      markerTimer -= Time.deltaTime;

      yield return null;
    }
    Destroy(uiMarker.gameObject);
  }
}
