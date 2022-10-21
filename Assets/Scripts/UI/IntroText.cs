using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroText : MonoBehaviour
{
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private DialogueManager dialogueManager;

    void Start()
    {
        UIEventListener.Instance.PauseGame();
        dialogueTrigger.TriggerDialogue();
    }

    void Update()
    {
        if (!dialogueManager.isDialoging) {
           UIEventListener.Instance.UnpauseGame(); 
        }
    }
}
