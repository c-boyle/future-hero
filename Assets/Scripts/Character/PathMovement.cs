using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class PathMovement : MonoBehaviour {

  [SerializeField] private List<Path> paths;
  [SerializeField] private int pointOfNoReturn = 2;
  [SerializeField] private int currentPathIndex = 0;
  [SerializeField] private bool loop = false;
  [SerializeField][ReadOnly] private int currentPathPoint = 0;
  [field: SerializeField] public bool Moving { get; set; } = false;
  private float stepSpeed = -1f;
  private Vector3 normalizedDirection = Vector3.zero;
  private float currentPointOriginalTime = -1f; // The number of seconds to reach a point upon first reaching that point

  // Update is called once per frame
  void Update() {
    if (!Moving) {
      return;
    }
    if (currentPathPoint >= paths[currentPathIndex].PathPoints.Count) {
      if (loop) {
        paths[currentPathIndex].PathPoints[currentPathPoint - 1].SecondsToReachPoint = currentPointOriginalTime;
        currentPathPoint = 0;
      } else {
        normalizedDirection = Vector3.zero;
        return;
      }
    }

    if (stepSpeed > 0f && paths[currentPathIndex].PathPoints[currentPathPoint].SecondsToReachPoint <= 0f) {
      paths[currentPathIndex].PathPoints[currentPathPoint].SecondsToReachPoint = currentPointOriginalTime;
      currentPathPoint++;
      stepSpeed = -1f;
      normalizedDirection = Vector3.zero;
      return;
    }

    if (stepSpeed < 0f) {
      currentPointOriginalTime = paths[currentPathIndex].PathPoints[currentPathPoint].SecondsToReachPoint;

      var targetPosition = paths[currentPathIndex].PathPoints[currentPathPoint].Transform.position;
      var position = transform.position;
      Vector3 direction = targetPosition - position;

      float distToTarget;
      if (paths[currentPathIndex].PathPoints[currentPathPoint].MatchHeight) {
        normalizedDirection = direction.normalized;
        distToTarget = direction.magnitude;
        direction.y = 0f;
      } else {
        direction.y = 0f;
        distToTarget = direction.magnitude;
        normalizedDirection = direction.normalized;
      }

      transform.LookAt(position + direction, Vector3.up);

      if (paths[currentPathIndex].PathPoints[currentPathPoint].UseFixedSpeed) {
        stepSpeed = paths[currentPathIndex].PathPoints[currentPathPoint].FixedSpeed;
        paths[currentPathIndex].PathPoints[currentPathPoint].SecondsToReachPoint = distToTarget / stepSpeed;
      } else {
        // Calculate the step speed required to reach the point after the required amount of time.
        if (paths[currentPathIndex].PathPoints[currentPathPoint].SecondsToReachPoint <= 0) {
          Debug.LogError("Cannot reach point in 0 or less seconds.");
        }
        stepSpeed = distToTarget / paths[currentPathIndex].PathPoints[currentPathPoint].SecondsToReachPoint;
      }
    }

    paths[currentPathIndex].PathPoints[currentPathPoint].SecondsToReachPoint -= Time.deltaTime;
  }

  private void FixedUpdate() {
    if (stepSpeed >= 0) {
      transform.position = transform.position + stepSpeed * normalizedDirection * Time.fixedDeltaTime;
    }
  }

  public void SetPathPoint(int pathPointIndex) {
    if (pathPointIndex >= 0 && pathPointIndex < paths[currentPathIndex].PathPoints.Count) {
      paths[currentPathIndex].PathPoints[currentPathPoint].SecondsToReachPoint = currentPointOriginalTime;
      currentPathPoint = pathPointIndex;
    } else {
      Debug.LogError("Attempt to set the current path of " + this.gameObject.name + " to an path index that is out of bounds.");
    }
    stepSpeed = -1f;
  }

  public void SetPath(int pathIndex) {
    if (currentPathPoint >= pointOfNoReturn) {
      return;
    }
    if (pathIndex >= 0 && pathIndex < paths.Count) {
      paths[currentPathIndex].PathPoints[currentPathPoint].SecondsToReachPoint = currentPointOriginalTime;
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
    [field: SerializeField] public bool MatchHeight { get; set; }

    [SerializeField] private bool _useFixedSpeed = true;
    public bool UseFixedSpeed { get => _useFixedSpeed; set => _useFixedSpeed = value; }

    [field: ConditionalField(nameof(_useFixedSpeed))][field: SerializeField] public float FixedSpeed = 1f;
    [field: ConditionalField(nameof(_useFixedSpeed), inverse: true)][field: SerializeField] public float SecondsToReachPoint { get; set; } = 10f;
  }
}
