using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.Events;

public class Item : MonoBehaviour {
  // Name acts as a key
  [SerializeField] private string itemName;
  [SerializeField] private UnityEvent onPickup;
  [SerializeField] private UnityEvent onDrop;

  private Vector3 originalScale;
  private int originalLayer;
  private float originalMass;

  private float riskSpeed = 3f;

  private List<GameObject> children = new List<GameObject>();

  public List<Collider> allColliders = new List<Collider>(); 

  public Bounds itemBounds;
  
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
    if (Rigidbody) originalMass = Rigidbody.mass;

    foreach(Transform childTransform in transform) {
      children.Add(childTransform.gameObject);
    }
    allColliders = new List<Collider>(GetComponentsInChildren<Collider>());

    if (allColliders.Count > 0) {
      itemBounds = allColliders[0].bounds;
      for(int i = 1; i < allColliders.Count; i++){
        itemBounds.Encapsulate(allColliders[i].bounds);
      }
    }

    if (itemBounds != null) {
      float delta = Time.deltaTime * 2;
      riskSpeed = Mathf.Min(itemBounds.extents.x / delta, itemBounds.extents.y / delta );
    }

  }

  public virtual void PickedUp() {
    onPickup?.Invoke();
  }

  public virtual void Dropped() {
    onDrop?.Invoke();
  }

  protected virtual void Update() {
    // When the mop moves (specifically rotates) the scale gets altered in a non-preferable way
    // Later should edit to only fix scale on rotation. For now we always call FixScale when items move...
    if (transform.hasChanged)
    {
        FixScale();
        transform.hasChanged = false;
    }
  }

  void FixedUpdate() {
    if (Rigidbody && Rigidbody.velocity.magnitude > riskSpeed ) {
      CheckForWall(Rigidbody.velocity);
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

  public void SetMass(float mass) {
    if (Rigidbody) Rigidbody.mass = mass; 
  }

  public void SetLayer(int layer){ 
    foreach (GameObject go in children) {
      go.layer = layer;
    }
  }

  public void CheckForWall(Vector3 velocity) {
    float speed = velocity.magnitude;
    float distance = speed * Time.deltaTime * 2; // approx. distance item will move in next 2 physics updates
    Vector3 position = transform.position;
    RaycastHit hit;
    if (Physics.BoxCast(position, transform.localScale, velocity, out hit, transform.rotation, distance)){
      Rigidbody.velocity *= (riskSpeed/speed) * 0.8f;

    }
  }

  public void ReturnToOriginal() {
    FixScale();
    // SetLayer(originalLayer);
    SetMass(originalMass);
  }

}
