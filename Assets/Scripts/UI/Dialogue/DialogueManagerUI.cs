using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManagerUI : DialogueManager
{
    [SerializeField] private TextMeshProUGUI field;

    private const string TEXT_BACKGROUND_HEX = "#00000088";

    protected override IEnumerator DisplaySentence(string sentence) {
        field.text = "";
        string displayedText = "";
        foreach (char letter in sentence.ToCharArray())
        {
            displayedText += letter;
            field.text = $"<mark={TEXT_BACKGROUND_HEX}>" + displayedText + "</mark>";
            yield return null;
        }
    }

    protected override void ClearSentence() {
        field.text = "";
        return;
    }
}
