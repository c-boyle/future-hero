using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
  // Name acts as a key
  [SerializeField] private string itemName;

  public override bool Equals(object other) {
    return base.Equals(other);
  }

  public override int GetHashCode() {
    return itemName.GetHashCode();
  }

  public static bool operator ==(Item a, Item b) {
    return a.itemName == b.itemName;
  }
  public static bool operator !=(Item a, Item b) {
    return a.itemName != b.itemName;
  }


}
