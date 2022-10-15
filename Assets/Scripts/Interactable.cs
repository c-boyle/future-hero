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

  // Prompt related variables
  [SerializeField] private TextMesh Prompt;
  public String promptText = "interact";
  private Transform prompt_transform;
  private Vector3 promptScale = new Vector3(1, 1, 1);

  // public float character_size = 1f;
  // public int font_size = 0;


  private readonly static HashSet<Interactable> enabledInteractables = new();
  private readonly static HashSet<Interactable> withinRangeInteractables = new();
  private static float lastRefreshTime = -1f;
  private const float withinRangeRefreshSeconds = 1f;

  private const float maxInteractionRange = 20f;

  private void Start() {
    // Parent
    _rend = GetComponent<Renderer>();
    _regularShader = _rend.material.shader;

    // Children (if any)
    _childRends = GetComponentsInChildren<Renderer>();
    _childRegularShaders = new List<Shader>();
    foreach (Renderer color in _childRends) {
      _childRegularShaders.Add(color.material.shader);
    }

    // prompt
    // TextMesh Prompt = GetComponentInChildren<TextMesh>();
    Prompt.text = promptText;
    prompt_transform = Prompt.gameObject.GetComponent<Transform>();
    promptScale = prompt_transform.localScale;
    prompt_transform.localScale = new Vector3(0, 0, 0);

  }

  private void OnEnable() {
    enabledInteractables.Add(this);
  }

  private void OnDisable() {
    enabledInteractables.Remove(this);
    withinRangeInteractables.Remove(this);

    if (shaderChanged) {
      toggleOutlineShader();
    }
  }

  private void OnDestroy(){
    this.OnDisable();
  }

  private void OnTriggerEnter(Collider collider) {
    if (collider.tag == "InteractReach") {
      withinRangeInteractables.Add(this);
    }
  }

  private void OnTriggerExit(Collider collider) {
    if (collider.tag == "InteractReach") {
      withinRangeInteractables.Remove(this);

      if (shaderChanged) {
        toggleOutlineShader();
      }
    }
  }

  private void OnInteract(ItemHolder itemHolder = null) {

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

  public static Interactable FindClosestInteractable(Vector3 interactorPosition, ItemHolder itemHolder) {
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
      } else if (interactable.shaderChanged) {
        interactable.toggleOutlineShader();
      }
    }
    return closestInteractable;
  }

  public static Interactable FindClosestInteractableInView(Vector3 cameraPosition, Vector3 cameraDirection, ItemHolder itemHolder) {
    float closestInteractableDist = Mathf.Infinity;
    Interactable closestInteractable = null;

    bool hit = Physics.Raycast(cameraPosition, cameraDirection, out RaycastHit lookingHit, Mathf.Infinity);
    Vector3 lookingPoint = hit ? lookingHit.point : cameraPosition + (cameraDirection * (maxInteractionRange / 2));
    Debug.DrawLine(lookingPoint, cameraPosition, Color.red);

    float currentTime = Time.time;
    bool refreshWithinRange = lastRefreshTime < 0 || currentTime - lastRefreshTime <= withinRangeRefreshSeconds;
    HashSet<Interactable> interactablesToCheck = refreshWithinRange ? enabledInteractables : withinRangeInteractables;
    if (refreshWithinRange) {
      lastRefreshTime = currentTime;
    }

    foreach (var interactable in interactablesToCheck) {
      if (interactable.shaderChanged) {
        interactable.toggleOutlineShader();
      }
      bool interactorIsHoldingThisInteractable = itemHolder != null && itemHolder.HeldItem != null && itemHolder.HeldItem.Interactable == interactable;
      if (!interactorIsHoldingThisInteractable) { // Skip an interactable if it's being held by the interactor or out of range
        Vector3 interactablePosition = interactable.transform.position;
        float distToLookingPoint = Vector3.Distance(lookingPoint, interactablePosition);
        float distToCamera = Vector3.Distance(cameraPosition, interactablePosition);
        bool inInteractionRange = distToCamera <= maxInteractionRange;
        if (distToLookingPoint < closestInteractableDist && inInteractionRange) {
          closestInteractableDist = distToLookingPoint;
          closestInteractable = interactable;
        }
        if (inInteractionRange) {
          withinRangeInteractables.Add(interactable);
        } else {
          withinRangeInteractables.Remove(interactable);
        }
      }
    }

    if (closestInteractable != null) {
      Debug.DrawLine(lookingPoint, closestInteractable.transform.position, Color.cyan);
    }

    return closestInteractable;
  }

  public static void UseClosestInteractable(Vector3 interactorPosition, ItemHolder itemHolder) {
    Interactable closestInteractable = Interactable.FindClosestInteractable(interactorPosition, itemHolder);
    if (closestInteractable != null) {
      closestInteractable.OnInteract(itemHolder);
    }
  }

  public static void UseClosestInteractableInView(Vector3 cameraPosition, Vector3 cameraDirection, ItemHolder itemHolder) {
    Interactable closestInteractable = Interactable.FindClosestInteractableInView(cameraPosition, cameraDirection, itemHolder);
    if (closestInteractable != null) {
      closestInteractable.OnInteract(itemHolder);
    }
  }

  public static void GiveClosestInteractableOutline(Vector3 interactorPosition, ItemHolder itemHolder) {
    Interactable closestInteractable = Interactable.FindClosestInteractable(interactorPosition, itemHolder);
    if (closestInteractable != null) {
      if (!closestInteractable.shaderChanged) {
        closestInteractable.toggleOutlineShader();
      }
      closestInteractable.showPrompt();
    }
  }

  public static void GiveClosestInteractableInViewOutline(Vector3 cameraPosition, Vector3 cameraDirection, ItemHolder itemHolder) {
    Interactable closestInteractable = Interactable.FindClosestInteractableInView(cameraPosition, cameraDirection, itemHolder);
    if (closestInteractable != null) {
      if (!closestInteractable.shaderChanged) {
        closestInteractable.toggleOutlineShader();
      }
      closestInteractable.showPrompt();
    }
  }

  public void showPrompt() {
    // GameObject UItextGO = new GameObject("Text2");
    // UItextGO.transform.SetParent(canvas_transform);

    // RectTransform trans = UItextGO.AddComponent<RectTransform>();
    // trans.anchoredPosition = new Vector2(x, y);

    // Text text = UItextGO.AddComponent<Text>();
    // text.text = text_to_print;
    // text.fontSize = font_size;
    // text.color = text_color;
    prompt_transform.localScale = promptScale;
    Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    Quaternion current = prompt_transform.rotation;
    prompt_transform.rotation = Quaternion.Euler(new Vector3(current.eulerAngles.x, rotation.eulerAngles.y, current.eulerAngles.z));
    // prompt_transform.rotation = rotation;


  }

  public void removePrompt() {
    prompt_transform.localScale = new Vector3(0, 0, 0);
  }

  public void toggleOutlineShader() {
    if (shaderChanged) {
      _rend.material.shader = _regularShader;
      removePrompt();
    } else {
      _rend.material.shader = _outlineShader;
      showPrompt();
    }

    int index = 0;
    foreach (Renderer colour in _childRends) {
      if (shaderChanged || colour.gameObject == Prompt.gameObject) {
        colour.material.shader = _childRegularShaders[index];
      } else {
        colour.material.shader = _outlineShader;
      }
      index++;
    }

    shaderChanged = !shaderChanged;

  }
}
