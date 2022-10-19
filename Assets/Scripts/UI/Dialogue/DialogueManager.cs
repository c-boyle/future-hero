using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    [SerializeField] private TextMesh field;
    public bool isDialoging = false;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialoging = true;
        Debug.Log("Just starting...");
        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        NextSentence();
    }

    public void NextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
        }
        
        if (!isDialoging) {
            Debug.Log("yeedogeee");
            return;
        }
        
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(DisplaySentence(sentence));
    }

    IEnumerator DisplaySentence(string sentence) {
        field.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            field.text += letter;
            yield return null;
        }
    }

    private void EndDialogue()
    {
        isDialoging = false;
        field.text = "";
        sentences.Clear();
        Debug.Log("end...");
    }

}
