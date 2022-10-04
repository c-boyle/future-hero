using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovement : MonoBehaviour {

  [SerializeField] private List<Path> paths;
  [SerializeField] private CharacterMovement movement;
  [SerializeField] private int pointOfNoReturn = 2;
  [SerializeField] private int currentPathIndex = 0;
  private int currentPathPoint = 0;

  // Update is called once per frame
  void Update() {
    if (currentPathPoint >= paths[currentPathIndex].PathPoints.Count) {
      movement.Move(Vector2.zero);
      return;
    }

    var targetPosition = paths[currentPathIndex].PathPoints[currentPathPoint].position;
    Vector3 direction = targetPosition - movement.transform.position;
    direction.y = 0f;
    
    if (direction.magnitude <= 4f) {
      currentPathPoint++;
      return;
    }
    Debug.Log(direction);
    Debug.DrawRay(movement.transform.position, direction, Color.cyan);
    movement.LookInDirection(direction);
    /*
    direction = new(direction.z, -direction.x, 0f);
    Vector2 movementInput = direction.normalized;
    Debug.Log("movementInput: " + movementInput);
    */
    movement.Move(new Vector2(0f, 1f));
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
  }

  [System.Serializable]
  private class Path {
    [field: SerializeField] public List<Transform> PathPoints { get; set; }
  }
}
