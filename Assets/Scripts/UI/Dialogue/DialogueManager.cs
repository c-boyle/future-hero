using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] private UnityEvent onDialogueOver;

    private Queue<string> sentences = new Queue<string>();
    private Queue<UnityEvent> events = new Queue<UnityEvent>();
    public bool isDialoging = false; // true when we are currently showing dialogue
    public bool finalDialogue = false; // true when the final piece of dialogue IS output
    protected string currentSentence = "";

    public void StartDialogue(Dialogue dialogue)
    {
        isDialoging = true;
        sentences.Clear();

        for(int i = 0; i < dialogue.sentences.Length; i++) {
            sentences.Enqueue(dialogue.sentences[i]);
            if (i < dialogue.events.Length) events.Enqueue(dialogue.events[i]);
        }

        NextSentence();
    }

    public void NextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
        }
        
        if (!isDialoging) {
            return;
        }

        if(currentSentence != "") {
            FinishSentence();
            return;
        }
        
        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(DisplaySentence(currentSentence));

        if(events.Count > 0) events.Dequeue().Invoke();

        if (sentences.Count == 0) {
            finalDialogue = true;
        }
    }

    protected virtual IEnumerator DisplaySentence(string sentence) {
        Debug.Log("sentence: " + sentence);
        currentSentence = "";
        return null;
    }

    protected virtual void FinishSentence() {
        Debug.Log("Finish!");
        return;  
    }

    protected virtual void ClearSentence() {
        Debug.Log("Clear!");
        return;
    }

    public void EndDialogue() {
        StopAllCoroutines();
        isDialoging = false;
        finalDialogue = false;
        ClearSentence();
        sentences.Clear();
        onDialogueOver?.Invoke();
    }

}
