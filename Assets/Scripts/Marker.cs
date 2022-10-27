using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour {

  [SerializeField] private Sprite icon;
  [SerializeField] private float iconBoxSize;

  private const float markerDuration = 5f;

  private static RectTransform uiRoot = null;
  private static Camera mainCamera = null;

  public void Activate() {
    if (uiRoot == null) {
      uiRoot = UIEventListener.Instance.transform as RectTransform;
    }
    GameObject markerObject = new("UIMarker for: " + name, typeof(Image));
    markerObject.transform.SetParent(uiRoot, false);
    StartCoroutine(DisplayMarker(markerObject.transform as RectTransform));
  }

  private IEnumerator DisplayMarker(RectTransform uiMarker) {
    if (mainCamera == null) {
      mainCamera = Camera.main;
    }
    uiMarker.GetComponent<Image>().sprite = icon;
    uiMarker.sizeDelta = new(iconBoxSize, iconBoxSize);
    //uiMarker.sizeDelta = icon.textureRect.max;
    float markerTimer = markerDuration;
    float edgeBuffer = uiMarker.rect.size.x / 2;
    while (markerTimer >= 0) {
      var screenPoint = WorldToScreenPointProjected(mainCamera, transform.position);
      uiMarker.position = ScreenPointEdgeClamp(screenPoint, edgeBuffer);

      markerTimer -= Time.deltaTime;

      // This is a pretty cruddy fix for hiding these on game pause, but it's better than nothing for now (for a better implementation, UIEventListener should probably hide these on pause)
      if (Time.deltaTime == 0f) {
        uiMarker.gameObject.SetActive(false);
      } else {
        uiMarker.gameObject.SetActive(true);
      }

      yield return null;
    }
    Destroy(uiMarker.gameObject);
  }

  // Citation: https://forum.unity.com/threads/camera-worldtoscreenpoint-bug.85311/
  public static Vector2 WorldToScreenPointProjected(Camera camera, Vector3 worldPos) {
    Vector3 camNormal = camera.transform.forward;
    Vector3 vectorFromCam = worldPos - camera.transform.position;
    float camNormDot = Vector3.Dot(camNormal, vectorFromCam);
    if (camNormDot <= 0) {
      // we are behind the camera forward facing plane, project the position in front of the plane
      Vector3 proj = (1.01f * camNormDot * camNormal);
      worldPos = camera.transform.position + (vectorFromCam - proj);
    }

    return RectTransformUtility.WorldToScreenPoint(camera, worldPos);
  }

  // Citation: https://forum.unity.com/threads/camera-worldtoscreenpoint-bug.85311/
  public static Vector3 ScreenPointEdgeClamp(Vector2 screenPos, float edgeBuffer) {
    bool needsClamp = (screenPos.x > Screen.width - edgeBuffer || screenPos.x < edgeBuffer) || (screenPos.y > Screen.height - edgeBuffer || screenPos.y < edgeBuffer);
    if (needsClamp) {
      // Take the direction of the screen point from the screen center to push it out to the edge of the screen
      // Use the shortest distance from projecting it along the height and width
      Vector2 screenCenter = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
      Vector2 screenDir = (screenPos - screenCenter).normalized;
      float angleRad = Mathf.Atan2(screenDir.x, screenDir.y);
      float distHeight = Mathf.Abs((screenCenter.y - edgeBuffer) / Mathf.Cos(angleRad));
      float distWidth = Mathf.Abs((screenCenter.x - edgeBuffer) / Mathf.Cos(angleRad + (Mathf.PI * 0.5f)));
      float dist = Mathf.Min(distHeight, distWidth);
      return screenCenter + (screenDir * dist);
    }
    return screenPos;
  }
}
