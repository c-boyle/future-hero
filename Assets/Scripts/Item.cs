using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
  // Name acts as a key
  [SerializeField] private string itemName;
  
  [field: SerializeField] public Rigidbody Rigidbody { get; private set; }

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


}
