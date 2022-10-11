using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {
  [SerializeField] private UnityEvent interactionAction;
  [Tooltip("Set to none if no item is required to interact.")][SerializeField] private Item requireItem = null;
  [SerializeField] private bool destroyRequiredItemOnInteraction = false;
  [SerializeField] private Item giveItem = null;
  [SerializeField] private bool disableAfterFirstUse = false;

  // Shaders for items
  private Shader _regularShader;
  private List<Shader> _childRegularShaders;
  [SerializeField] private Shader _outlineShader;

  // Renderers
  private Renderer _rend;
  private Renderer[] _childRends;

  // Booleans
  private bool firstUse = true;
  public bool shaderChanged = false;

  private readonly static HashSet<Interactable> enabledInteractables = new();
  private readonly static HashSet<Interactable> withinRangeInteractables = new();

  private const float maxInteractionRange = 15f;

  private void Start() {
    // Parent 
    _rend = GetComponent<Renderer>();
    _regularShader = _rend.material.shader;

    _childRends = GetComponentsInChildren<Renderer>();
    _childRegularShaders = new List<Shader>();
    foreach(Renderer color in _childRends){
      _childRegularShaders.Add(color.material.shader);
    }
  }

  private void OnEnable() {
    enabledInteractables.Add(this);
  }

  private void OnDisable() {
    enabledInteractables.Remove(this);
    
    if (shaderChanged){
      toggleOutlineShader();
    }
  }

  private void OnTriggerEnter(Collider collider) {
    if (collider.tag == "InteractReach"){ 
      withinRangeInteractables.Add(this);
    }
  }

  private void OnTriggerExit(Collider collider) {
    if (collider.tag == "InteractReach"){ 
      withinRangeInteractables.Remove(this);
     
      if (shaderChanged){
        toggleOutlineShader();
      }
    }
  }

  private void OnInteract(ItemHolder itemHolder) {

    if (!gameObject.activeSelf || (disableAfterFirstUse && !firstUse)) {
      return;
    }
    bool meetsItemRequirement = requireItem == null || (itemHolder != null && requireItem == itemHolder.HeldItem);
    if (meetsItemRequirement) {
      if (giveItem != null && itemHolder != null) {
        itemHolder.GrabItem(giveItem);
      }
      if (destroyRequiredItemOnInteraction && meetsItemRequirement) {
        var itemToDestroy = itemHolder.HeldItem;
        itemHolder.DropItem();
        Destroy(itemToDestroy.gameObject);
      }
      interactionAction?.Invoke();
      firstUse = !firstUse;
      // Debug.Log(name + " interacted");
    }
  }

  public static Interactable FindClosestInteractable(Vector3 interactorPosition, ItemHolder itemHolder){
    float closestInteractableDist = Mathf.Infinity;
    Interactable closestInteractable = null;
    foreach (var interactable in withinRangeInteractables) {
      bool interactorIsHoldingThisInteractable = itemHolder != null && itemHolder.HeldItem != null && itemHolder.HeldItem.Interactable == interactable;
      if ((!interactorIsHoldingThisInteractable)) { // Skip an interactable if it's being held by the interactor or out of range
        float dist = Vector3.Distance(interactorPosition, interactable.transform.position);
        if (dist < closestInteractableDist) {
          closestInteractableDist = dist;
          closestInteractable = interactable;
        }
      } else if (interactable.shaderChanged){
        interactable.toggleOutlineShader();
      }
    }
    return closestInteractable;
  }

  public static void UseClosestInteractable(Vector3 interactorPosition, ItemHolder itemHolder) {
    Interactable closestInteractable = Interactable.FindClosestInteractable(interactorPosition, itemHolder);
    if (closestInteractable != null) {
      closestInteractable.OnInteract(itemHolder);
    }
  }

  public static void GiveClosestItemOutline(Vector3 interactorPosition, ItemHolder itemHolder) {
    Interactable closestInteractable = Interactable.FindClosestInteractable(interactorPosition, itemHolder);
    if (closestInteractable != null && !closestInteractable.shaderChanged) {
      closestInteractable.toggleOutlineShader();
    }
  }

  public void toggleOutlineShader(){
    if (shaderChanged) {
      _rend.material.shader = _regularShader;
    } else {
      _rend.material.shader = _outlineShader;
    }

    int index = 0;
    foreach(Renderer colour in _childRends){
      if (shaderChanged) {
        colour.material.shader = _childRegularShaders[index];
      } else {
        colour.material.shader = _outlineShader;
      }
      index ++;
    }

    shaderChanged = !shaderChanged;

  }
}
