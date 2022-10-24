using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
  // Name acts as a key
  [SerializeField] private string itemName;

  private Vector3 originalScale;
  private int originalLayer;
  private List<GameObject> children = new List<GameObject>();
  
  [field: SerializeField] public Rigidbody Rigidbody { get; private set; }

  // The getter code below here is overcomplicated so that the result of calling GetComponent can be cached
  // because I have trust issues with GetComponent's performance...
  private Interactable attachedInteractable = null;
  private bool checkedBefore = false;
  public Interactable Interactable {
    get {
      if (!checkedBefore) {
        TryGetComponent<Interactable>(out attachedInteractable);
        checkedBefore = true;
      }
      return attachedInteractable;
    }
  }

  void Start() {
    originalScale = transform.lossyScale;
    originalLayer = gameObject.layer;
    foreach(Transform childTransform in transform) {
      children.Add(childTransform.gameObject);
    }
  }

  void Update()
  {
    // When the mop moves (specifically rotates) the scale gets altered in a non-preferable way
    // Later should edit to only fix scale on rotation. For now we always call FixScale when items move...
    if (transform.hasChanged)
    {
        FixScale();
    }
  }

  public override bool Equals(object other) {
    return base.Equals(other);
  }

  public override int GetHashCode() {
    return itemName.GetHashCode();
  }

  public static bool operator ==(Item a, Item b) {
    if (a is Item itemA && b is Item itemB) {
      return a.itemName == b.itemName;
    } else if (a is null && b is null) {
      return true;
    }
    return false;
  }
  public static bool operator !=(Item a, Item b) {
    if (a is Item itemA && b is Item itemB) {
      return a.itemName != b.itemName;
    } else if (a is null && b is null) {
      return false;
    }
    return true;
  }

  public void FixScale() {
    var newGlobalScale = transform.lossyScale;
    if (newGlobalScale == originalScale) { // No point fixing a scale that's not altered...
      return;
    }

    var itemScale = transform.localScale;

    // Revert the size of the item picked to what it was before being picked up
    transform.localScale = new(itemScale.x * originalScale.x / newGlobalScale.x, 
                               itemScale.y * originalScale.y / newGlobalScale.y, 
                               itemScale.z * originalScale.z / newGlobalScale.z);
  }

  public void setLayer(int layer){ 
    foreach (GameObject go in children) {
      go.layer = layer;
    }
  }

  void OnCollisionStay(Collision collision) {
    FixScale();
  }

  public void ReturnToOriginal() {
    FixScale();
    setLayer(originalLayer);
  }

}
