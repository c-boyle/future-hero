using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.InputSystem;
using System;

public abstract class Watch : MonoBehaviour {
  [SerializeField] private Renderer watchRenderer;
  [SerializeField] private Color presentColour = new Color(0, 0, 1, 0); [SerializeField] private Color futureColour = new Color(1, 0, 0, 0);

  [SerializeField] private Image TimeLeftVisual;
  private float percentageTime = 0;
  protected bool lookingAt = false;

  protected float transitionTime = 0.0f; protected float transitionMax = 1.0f; protected float deltaTransition = 0.03f;

  protected int futureSeconds = 0;
  protected int futureMinutes = 0;

  protected int presentSeconds = 0;
  protected int presentMinutes = 0;

  protected const int minuteLength = 60;
  protected bool isFuture = false;

  protected bool notTransitioning = true;

  [SerializeField] protected float totalSeconds;


  private void Start() {
    totalSeconds = 240f;
    TimeLeftVisual.type = Image.Type.Filled;
    LevelTimer.TimerUpdated += SetPresent;
    if (watchRenderer != null) {
      watchRenderer.material.color = presentColour;
      SetGlow(false);
    }
  }

  // Update is called once per frame
  void Update() {
    if (notTransitioning) {
      UpdateWatch();
      UpdateTimeLeft();
    }
  }

  private void SetPresent(object sender, LevelTimer.TimerUpdateEventArgs e) {
    int secondsLeft = (int)e.SecondsLeft;
    percentageTime = (totalSeconds - secondsLeft) / totalSeconds;
    presentMinutes = (int)Math.Floor((double)(secondsLeft / minuteLength));
    presentSeconds = secondsLeft % minuteLength;
  }

  protected virtual void UpdateWatch() {
    return;
  }

  private void UpdateTimeLeft() {
    if(!isFuture) TimeLeftVisual.fillAmount = percentageTime;
    else TimeLeftVisual.fillAmount = 1;
  }

  public void toggleFutureTime(bool timeVision) {
    if (isFuture == timeVision) return;

    float prevTime = transitionTime;

    SetGlow(false);

    StopCoroutine(TransitionManager());
    StartCoroutine(TransitionManager());

    StopCoroutine(TransitionTime());
    StartCoroutine(TransitionTime());

    Color start = (isFuture) ? futureColour : presentColour;
    Color end = (isFuture) ? presentColour : futureColour;
    StopCoroutine(TransitionColour(end, start));
    StartCoroutine(TransitionColour(start, end));
    
    StopCoroutine(TransitionTimeLeft(prevTime));
    StartCoroutine(TransitionTimeLeft(prevTime));

    isFuture = timeVision;
  }

  protected IEnumerator TransitionManager() {
    transitionTime = 0.0f;
    var gamepad = Gamepad.current;
    if (gamepad != null) gamepad.SetMotorSpeeds(0.01f, 0.01f);
    notTransitioning = false;
    while (transitionTime < transitionMax) {
      transitionTime += deltaTransition;
      yield return null;
    }
    notTransitioning = true;
    if (gamepad != null) gamepad.SetMotorSpeeds(0.0f, 0.0f);
  }

  protected virtual IEnumerator TransitionTime() {
    Debug.Log("Time will change!");
    return null;
  }

  protected IEnumerator TransitionColour(Color start, Color end) {
    while ((watchRenderer != null) && !notTransitioning && (transitionTime < transitionMax)) {
      watchRenderer.material.color = Color.Lerp(start, end, transitionTime);
      yield return null;
    }
  }

  protected IEnumerator TransitionTimeLeft(float prevTime = 0f) {
    float start = (!isFuture) ? percentageTime : 1;
    float end = (!isFuture) ? 1 : percentageTime;
    start = Mathf.Lerp(start, end, prevTime/transitionMax);
    while (!notTransitioning && (transitionTime < transitionMax)) {
      TimeLeftVisual.fillAmount = Mathf.Lerp(start, end, transitionTime/transitionMax);
      yield return null;
    }
  }

  protected void SetGlow(bool glow) {
    if (watchRenderer == null) return;

    Material material = watchRenderer.material;
    if (glow) {
      material.EnableKeyword("_EMISSION");
      material.SetColor("_EmissionColor", material.color * 3.5f);
    } else {
      material.DisableKeyword("_EMISSION");
      material.SetColor("_EmissionColor", Color.black);
    }
  }

  public void TriggerNotification() {
    StartCoroutine(GlowNotification());
  }

  protected IEnumerator GlowNotification() {
    for (int i = 0; i < 7; i++) {
      SetGlow(true);
      yield return new WaitForSeconds(0.25f);
      SetGlow(false);
      yield return new WaitForSeconds(0.25f);
    }
  }

  public virtual void LookingAt(bool look) {
    StopAllCoroutines();
    lookingAt = look;
  } 
}
