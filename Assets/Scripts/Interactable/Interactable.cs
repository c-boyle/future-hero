using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class Interactable : MonoBehaviour {
  public UnityEvent interactionAction;
  [SerializeField] private List<UnityEvent> useCountDependentActions;
  [Tooltip("Set to none if no item is required to interact.")][SerializeField] private Item requireItem = null;
  [SerializeField] private bool destroyRequiredItemOnInteraction = false;
  [SerializeField] private Item giveItem = null;
  [SerializeField] private bool disableAfterFirstUse = false;
  [SerializeField] private GameObject rootObject = null;

  [SerializeField] private bool notYetOutlined = true;
  [SerializeField] private UnityEvent onFirstOutline;

  private Item _item = null;
  public Item Item { get => _item; }
  public Item RequiredItem { get => requireItem; }

  private int interactionCount = 0;

  // Shaders for items
  private Shader _regularShader;
  private List<Shader> _childRegularShaders;
  private Color? _regularOutlineColor;
  [SerializeField] private Shader _outlineShader;

  // Renderers
  private Renderer _rend;
  private List<Renderer> _childRends = new();

  public string promptText = "interact";

  // Booleans
  [ReadOnly] public bool shaderChanged = false;

  // public float character_size = 1f;
  // public int font_size = 0;


  private readonly static HashSet<Interactable> enabledInteractables = new();
  private static HashSet<Interactable> withinRangeInteractables = new();
  private static float lastRefreshTime = -1f;
  private const float withinRangeRefreshSeconds = 0.5f;

  private const float maxInteractionRange = 2.5f;

  private Color originalPromptColor;

  private void Start() {
    if (rootObject == null) {
      rootObject = gameObject;
    }
    // Parent
    _rend = rootObject.GetComponent<Renderer>();
    if (_rend != null) {
      _regularShader = _rend.material.shader;
    }

    // Children (if any)
    var childRenderers = rootObject.GetComponentsInChildren<Renderer>();
    _childRegularShaders = new List<Shader>();
    foreach (Renderer color in childRenderers) {
      if (color is not ParticleSystemRenderer) {
        _childRegularShaders.Add(color.material.shader);
        _childRends.Add(color);
      }
    }

    if (giveItem == null) {
      _regularOutlineColor = Color.blue;
    }

    TryGetComponent<Item>(out _item);
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

  protected virtual void OnInteract(ItemHolder itemHolder = null, bool grab = false) {

    if (!gameObject.activeSelf) {
      return;
    }
    if (grab) {
      if (MeetsGrabRequirement(itemHolder)){
        itemHolder.GrabItem(giveItem);
        interactionAction?.Invoke();
      }
    } else if (MeetsItemRequirement(itemHolder) && (giveItem == null || !giveItem.Rigidbody.isKinematic)) {
      if (destroyRequiredItemOnInteraction) {
        var itemToDestroy = itemHolder.HeldItem;
        itemHolder.DropItem();
        Destroy(itemToDestroy.gameObject);
      }
      if (Item is Cup cup && itemHolder.HeldItem is Cigarette cig) {
        cig.SafePosition();
        cup.OwnerIsAngry();
      }
      interactionAction?.Invoke();
      if (useCountDependentActions.Count > 0) {
        useCountDependentActions[Mathf.Min(interactionCount, useCountDependentActions.Count - 1)]?.Invoke();
      }
      interactionCount++;

      // Debug.Log(name + " interacted");

      if (disableAfterFirstUse) {
        this.enabled = false;
      }
    }
  }

  public static Interactable FindClosestInteractable(Vector3 interactorPosition, ItemHolder itemHolder) {
    float closestInteractableDist = Mathf.Infinity;
    Interactable closestInteractable = null;
    foreach (var interactable in withinRangeInteractables) {
      bool interactorIsHoldingThisInteractable = itemHolder != null && itemHolder.HeldItem != null && itemHolder.HeldItem.Interactable == interactable;
      if ((!interactorIsHoldingThisInteractable) && interactable._rend.isVisible) { // Skip an interactable if it's being held by the interactor or out of range
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
    bool refreshWithinRange = lastRefreshTime < 0 || currentTime - lastRefreshTime >= withinRangeRefreshSeconds;
    HashSet<Interactable> interactablesToCheck = refreshWithinRange ? enabledInteractables : withinRangeInteractables;
    if (refreshWithinRange) {
      lastRefreshTime = currentTime;
    }

    HashSet<Interactable> inRangeInteractables = new();

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
        bool inInteractionView = Vector3.Angle(cameraDirection, interactablePosition - cameraPosition) < 45f;
        if (distToLookingPoint < closestInteractableDist && inInteractionRange && inInteractionView) {
          closestInteractableDist = distToLookingPoint;
          closestInteractable = interactable;
        }
        if (inInteractionRange) {
          inRangeInteractables.Add(interactable);
        }
      }
    }

    withinRangeInteractables = inRangeInteractables;

    if (closestInteractable != null) {
      Debug.DrawLine(lookingPoint, closestInteractable.transform.position, Color.cyan);
    }

    return closestInteractable;
  }

  public static void UseInteractable(Interactable interactable, ItemHolder itemHolder, bool grab = false) {
    if (interactable != null) {
      interactable.OnInteract(itemHolder, grab);
    }
  }

  public static void UseClosestInteractable(Vector3 interactorPosition, ItemHolder itemHolder, bool grab = false) {
    Interactable closestInteractable = Interactable.FindClosestInteractable(interactorPosition, itemHolder);
    if (closestInteractable != null) {
      closestInteractable.OnInteract(itemHolder, grab);
    }
  }

  public static void UseClosestInteractableInView(Vector3 cameraPosition, Vector3 cameraDirection, ItemHolder itemHolder, bool grab = false) {
    Interactable closestInteractable = Interactable.FindClosestInteractableInView(cameraPosition, cameraDirection, itemHolder);
    if (closestInteractable != null) {
      closestInteractable.OnInteract(itemHolder, grab);
    }
  }

  public static Interactable GiveClosestInteractableOutline(Vector3 interactorPosition, ItemHolder itemHolder) {
    Interactable closestInteractable = Interactable.FindClosestInteractable(interactorPosition, itemHolder);
    if (closestInteractable != null) {
      if (!closestInteractable.shaderChanged) {
        closestInteractable.toggleOutlineShader(itemHolder);
      }
    }
    return closestInteractable;
  }

  public static Interactable GiveClosestInteractableInViewOutline(Vector3 cameraPosition, Vector3 cameraDirection, ItemHolder itemHolder) {
    Interactable closestInteractable = Interactable.FindClosestInteractableInView(cameraPosition, cameraDirection, itemHolder);
    if (closestInteractable != null) {
      if (!closestInteractable.shaderChanged) {
        closestInteractable.toggleOutlineShader(itemHolder);
      }
    }
    return closestInteractable;
  }

  public virtual bool MeetsItemRequirement(ItemHolder itemHolder) {
    return requireItem == null || (itemHolder != null && requireItem == itemHolder.HeldItem);
  }

  protected bool MeetsGrabRequirement(ItemHolder itemHolder) {
    return giveItem != null && itemHolder != null;
  }

  public void toggleOutlineShader(ItemHolder itemHolder = null) {
    bool meetsItemRequirement = MeetsItemRequirement(itemHolder);
    if (shaderChanged) {
      if (_rend != null) {
        _rend.material.shader = _regularShader;
      }
    } else {
      if (_rend != null) {
        _rend.material.shader = _outlineShader;
        if (!_regularOutlineColor.HasValue) {
          _regularOutlineColor = _rend.material.GetColor("_OutlineColor");
        }
        Color outlineColor = meetsItemRequirement ? _regularOutlineColor.Value : Color.red;
        _rend.material.SetColor("_OutlineColor", outlineColor);
        if(notYetOutlined) {
          notYetOutlined = false;
          onFirstOutline.Invoke();
        }
      }
    }

    int index = 0;
    foreach (Renderer colour in _childRends) {
      if (shaderChanged) {
        colour.material.shader = _childRegularShaders[index];
      } else {
        colour.material.shader = _outlineShader;
        if (!_regularOutlineColor.HasValue) {
          _regularOutlineColor = colour.material.GetColor("_OutlineColor");
        }
        Color outlineColor = meetsItemRequirement ? _regularOutlineColor.Value : Color.red;
        colour.material.SetColor("_OutlineColor", outlineColor);
      }
      index++;
    }

    shaderChanged = !shaderChanged;

  }

  public void SetPromptText(string promptText) {
    this.promptText = promptText;
  }
}
