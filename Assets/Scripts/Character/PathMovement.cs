using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovement : MonoBehaviour {

  [SerializeField] private List<Path> paths;
  [SerializeField] private Rigidbody rb;
  [SerializeField] private int pointOfNoReturn = 2;
  [SerializeField] private int currentPathIndex = 0;
  private int currentPathPoint = 0;
  private float stepSpeed = -1f;
  private Vector3 normalizedDirection = Vector3.zero;
  private float disabledTime = -1f;

  // Update is called once per frame
  void Update() {
    if (currentPathPoint >= paths[currentPathIndex].PathPoints.Count) {
      rb.velocity = Vector3.zero;
      return;
    }

    if (paths[currentPathIndex].PathPoints[currentPathPoint].SecondsToReachPoint <= 0f) {
      currentPathPoint++;
      stepSpeed = -1f;
      return;
    }

    if (stepSpeed < 0f) {
      var targetPosition = paths[currentPathIndex].PathPoints[currentPathPoint].Transform.position;
      var rbPosition = rb.position;
      Vector3 direction = targetPosition - rbPosition;
      direction.y = 0f;

      var distToTarget = direction.magnitude;

      normalizedDirection = direction.normalized;
      if (stepSpeed < 0f) {
        stepSpeed = distToTarget / paths[currentPathIndex].PathPoints[currentPathPoint].SecondsToReachPoint;
      }
      transform.LookAt(rbPosition + normalizedDirection, Vector3.up);
    }

    paths[currentPathIndex].PathPoints[currentPathPoint].SecondsToReachPoint -= Time.deltaTime;
  }

  private void FixedUpdate() {
    if (stepSpeed >= 0) {
      rb.velocity = stepSpeed * normalizedDirection;
    }
  }

  public void SetPath(int pathIndex) {
    if (currentPathPoint >= pointOfNoReturn) {
      return;
    }
    if (pathIndex >= 0 && pathIndex < paths.Count) {
      currentPathIndex = pathIndex;
    } else {
      Debug.LogError("Attempt to set the current path of " + this.gameObject.name + " to an path index that is out of bounds.");
    }
    stepSpeed = -1f;
  }

  [System.Serializable]
  private class Path {
    [field: SerializeField] public List<PathPoint> PathPoints { get; set; }
  }

  [System.Serializable]
  private class PathPoint {
    [field: SerializeField] public Transform Transform { get; set; }
    [field: SerializeField] public float SecondsToReachPoint { get; set; } = 10f;
  }
}
