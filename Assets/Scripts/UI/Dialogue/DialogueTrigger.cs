using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {
  public Dialogue dialogue;
  [SerializeField] private DialogueManager manager;

  void Start() {
    if (!manager) manager = FindObjectOfType<DialogueManager>();
  }

  public void TriggerDialogue() {
    if (!manager.isDialoging) {
      manager.StartDialogue(dialogue);
    } else {
      manager.NextSentence();
    }
  }

}
